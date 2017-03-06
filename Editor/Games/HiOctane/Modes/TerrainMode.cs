using System;
using System.Collections.Generic;
using System.Text;
using LevelEditor.Engine.Models;
using LevelEditor.Engine.Models.Primitives;
using LevelEditor.Games.HiOctane.Models;
using OpenTK;
using LevelEditor.Engine;
using LevelEditor.Engine.GUI.Controls;
using LevelEditor.Games.HiOctane.Resources;
using LevelEditor.Utils;
using LevelEditor.Games.HiOctane.Controls;
using System.Drawing;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class TerrainMode : EditMode
    {
        private TerrainCursor terrainEditCursor;

        private Panel toolsPanel;

        private Slider sizeSlider;
        private Slider intensitySlider;
        private Slider targetHeightSlider;

        private ImageButton lowerButton;
        private ImageButton raiseButton;
        private ImageButton flattenButton;

        private BrushSelector brushes;

        public TerrainMode() : base("Terrain")
        {
            showWalls = false;
            showWaypoints = false;
            showBuildings = true;
            showEntities = false;
            showTerrain = true;
            morphsEnabled = false;

            terrainEditCursor = new TerrainCursor(new Vector4(0, 0.8f, 0, 1));
        }

        public override void Update(float time, float dTime)
        {
            MouseUsed = Window.MouseLeftDown;

            Vector3 rayStart = EngineBase.Renderer.Unproject(Window.MousePos, 0f);
            Vector3 rayEnd = EngineBase.Renderer.Unproject(Window.MousePos, 1f);
            Vector3 dir = (rayEnd - rayStart).Normalized();

            Editor.Profiler.Begin("RAYCAST");
            RayCastResult rc = level.Collisions.RayCast(rayStart, dir);
            Editor.Profiler.End("RAYCAST");

            if (rc.Hit)
            {
                level.AddNode(terrainEditCursor);
                terrainEditCursor.Position = rc.PositionSnapped;
                if (Window.MouseLeftDown) applyBrush(rc.Position);
            }
            else level.RemoveNode(terrainEditCursor);

            base.Update(time, dTime);
        }

        private void applyBrush(Vector3 brushPos)
        {
            float intensity = lowerButton.Down ? intensitySlider.Value : -intensitySlider.Value;
            bool toHeight = !flattenButton.Down;

            Vector3 center = new Vector3(brushPos.X, 0, brushPos.Z);
            int radius = (int)Math.Ceiling(0.5f * sizeSlider.Value);

            List<MapEntry> refreshNormals = new List<MapEntry>();

            Bitmap brush = brushes.SelectedBrush.Icon.Bitmap;
            float imgSize = brush.Width - 1;
            float imgSizeHalf = imgSize * 0.5f;
            float scale = imgSizeHalf / radius;

            for (float z = brushPos.Z - radius; z < brushPos.Z + radius; z++)
            {
                for (float x = brushPos.X - radius; x < brushPos.X + radius; x++)
                {
                    if (x < 0 || z < 0 || x > level.Data.Width - 1 || z > level.Data.Height - 1) continue;

                    Vector3 pos = new Vector3(x, 0, z);
                    Vector3 dist = pos - center;

                    MapEntry entry = level.Terrain.GetMapEntry((int)Math.Round(x), (int)Math.Round(z));

                    float brushOpacity = brush.GetPixel(
                        (int)Math.Max(0, Math.Min(imgSize, Math.Floor(imgSizeHalf + dist.X * scale))),
                        (int)Math.Max(0, Math.Min(imgSize, Math.Floor(imgSizeHalf + dist.Z * scale)))).R / 255f;

                    if (toHeight) entry.Height += (targetHeightSlider.Value - entry.Height) * brushOpacity * intensitySlider.Value;
                    else entry.Height += intensity * brushOpacity;
                    refreshNormals.Add(entry);
                    level.Terrain.ApplyHeight(entry);
                }
            }

            foreach(MapEntry entry in refreshNormals)
            {
                level.Terrain.RecalculateNormals(entry);
            }

            List<Column> columns = level.ColumnsInRange((int)Math.Round(brushPos.X - radius) - 1, (int)Math.Round(brushPos.Z - radius) - 1, sizeSlider.Value + 2, sizeSlider.Value + 2);
            foreach (Column column in columns) column.UpdateGeometry();
        }

        public override void CreateControls(GUI gui)
        {
            toolsPanel = new Panel(new Vector2(200, 180));
            toolsPanel.Alpha = 0.6f;
            toolsPanel.MouseEnabled = true;
            toolsPanel.BorderSize = 1;
            toolsPanel.Visible = false;
            toolsPanel.Pos.Y = (float)Math.Floor(0.5f * (gui.Size.Y - toolsPanel.Size.Y));
            gui.AddChild(toolsPanel);

            lowerButton = new ImageButton(new Vector2(42, 32), "gui/images/downGray.png", lowerButtonClicked);
            lowerButton.Pos.X = 4;
            lowerButton.Pos.Y = toolsPanel.Size.Y - lowerButton.Size.Y - 10;
            lowerButton.Down = true;
            toolsPanel.AddChild(lowerButton);

            raiseButton = new ImageButton(new Vector2(42, 32), "gui/images/upGray.png", raiseButtonClicked);
            raiseButton.Pos.X = 54;
            raiseButton.Pos.Y = lowerButton.Pos.Y;
            toolsPanel.AddChild(raiseButton);

            flattenButton = new ImageButton(new Vector2(42, 32), "gui/images/midGray.png", flattenButtonClicked);
            flattenButton.Pos.X = 104;
            flattenButton.Pos.Y = lowerButton.Pos.Y;
            flattenButton.Down = true;
            toolsPanel.AddChild(flattenButton);


            sizeSlider = new Slider("Brush size", toolsPanel.Size.X - 8, brushSizeChanged);
            sizeSlider.Minimum = 1;
            sizeSlider.Maximum = 20;
            sizeSlider.Value = 5;
            sizeSlider.Pos.X = 4;
            sizeSlider.Pos.Y = toolsPanel.Size.Y - sizeSlider.Size.Y - 60;
            toolsPanel.AddChild(sizeSlider);

            intensitySlider = new Slider("Brush intensity", toolsPanel.Size.X - 8);
            intensitySlider.Minimum = 0.01f;
            intensitySlider.Maximum = 1f;
            intensitySlider.Value = 0.1f;
            intensitySlider.Pos.X = 4;
            intensitySlider.Pos.Y = sizeSlider.Pos.Y - 40;
            toolsPanel.AddChild(intensitySlider);

            targetHeightSlider = new Slider("Target height", toolsPanel.Size.X - 8);
            targetHeightSlider.Minimum = 0.0f;
            targetHeightSlider.Maximum = 40f;
            targetHeightSlider.Value = 10f;
            targetHeightSlider.Pos.X = 4;
            targetHeightSlider.Pos.Y = sizeSlider.Pos.Y - 80;
            // is added dynamically below

            brushes = new BrushSelector();
            brushes.Pos.X = toolsPanel.Size.X - brushes.Size.X - 4;
            brushes.Pos.Y = toolsPanel.Size.Y - brushes.Size.Y - 4;
            toolsPanel.AddChild(brushes);

            brushSizeChanged(2f * sizeSlider.Value);
        }


        private void brushSizeChanged(float value)
        {
            terrainEditCursor.Radius = 0.5f * value;
        }

        private void flattenButtonClicked(ImageButton obj)
        {
            lowerButton.Down = true;
            raiseButton.Down = true;
            flattenButton.Down = false;
            toolsPanel.AddChild(targetHeightSlider);
        }

        private void raiseButtonClicked(ImageButton obj)
        {
            lowerButton.Down = true;
            raiseButton.Down = false;
            flattenButton.Down = true;
            toolsPanel.RemoveChild(targetHeightSlider);
        }

        private void lowerButtonClicked(ImageButton obj)
        {
            lowerButton.Down = false;
            raiseButton.Down = true;
            flattenButton.Down = true;
            toolsPanel.RemoveChild(targetHeightSlider);
        }

        public override void Resize(GUI gui)
        {
            layout();
            toolsPanel.Pos.Y = (float)Math.Floor(0.5f * (gui.Size.Y - toolsPanel.Size.Y));
        }

        private void layout()
        {

        }

        protected override void enable()
        {
            toolsPanel.Visible = true;
            level.Terrain.Wireframe = 1.0f;
            level.AddNode(terrainEditCursor);
        }

        protected override void disable()
        {
            toolsPanel.Visible = false;
            level.Terrain.Wireframe = 0.0f;
            level.RemoveNode(terrainEditCursor);
        }
    }
}
