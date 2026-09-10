using LevelEditor.Engine;
using LevelEditor.Engine.GUI.Controls;
using LevelEditor.Engine.Models;
using LevelEditor.Engine.Resources;
using LevelEditor.Games.HiOctane.Resources;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class TexturesMode : EditMode
    {
        private FloatPanel panel;
        private ImageButton preview;
        private Label selectionLabel, pageLabel, hint;
        private Button rotateButton, flipButton, paintButton, pickButton, undoButton;
        private readonly List<ImageButton> tiles = new List<ImageButton>();
        private readonly List<Panel> tileFrames = new List<Panel>();
        private readonly BuildingsCursor cursor;
        private int selectedTexture, modification, page, pageSize = 16;
        private bool picking, painting, sampleHeld;
        private MapEntry lastPainted;
        private Level paletteLevel;
        private readonly Dictionary<MapEntry, TextureState> undo = new Dictionary<MapEntry, TextureState>();

        private struct TextureState { public int Id, Modification; }

        public TexturesMode() : base("Textures")
        {
            showWalls = showWaypoints = showEntities = false;
            showTerrain = showBuildings = true;
            morphsEnabled = false;
            cursor = new BuildingsCursor(new Vector4(0, 0.8f, 1, 0.25f));
        }

        public override void CreateControls(GUI gui)
        {
            panel = new FloatPanel(new Vector2(300, 500), "TERRAIN TEXTURES", false, false);
            panel.Pos.Y = 22;
            AddPanel(gui, panel);
            preview = new ImageButton(new Vector2(48, 48), new TextureResource("texture-preview", new Bitmap(64, 64)));
            preview.MouseEnabled = false;
            preview.Pos.X = 8;
            panel.AddChild(preview);
            selectionLabel = AddLabel(new Vector2(226, 20), 64, 0);
            rotateButton = AddButton("Rotate", 64, 0, 72, delegate { TransformSelection(true); });
            flipButton = AddButton("Flip", 140, 0, 60, delegate { TransformSelection(false); });
            undoButton = AddButton("Undo", 204, 0, 86, delegate { UndoStroke(); });
            paintButton = AddButton("Paint", 8, 0, 136, delegate { picking = false; RefreshSelection(); });
            pickButton = AddButton("Pick tile", 148, 0, 142, delegate { picking = true; RefreshSelection(); });

            for (int i = 0; i < 16; i++)
            {
                var frame = new Panel(new Vector2(68, 68));
                frame.BorderSize = 2;
                frame.BackgroundColor = Color.FromArgb(37, 37, 38);
                panel.AddChild(frame);
                tileFrames.Add(frame);
                var tile = new ImageButton(new Vector2(64, 64), new TextureResource("texture-tile-" + i, new Bitmap(64, 64)), null, TileClicked);
                tile.Pos = new Vector2(2, 2);
                frame.AddChild(tile);
                tiles.Add(tile);
            }
            AddButton("<", 8, 52, 40, delegate { ChangePage(-1); });
            AddButton(">", 250, 52, 40, delegate { ChangePage(1); });
            pageLabel = AddLabel(new Vector2(194, 20), 54, 54);
            hint = AddLabel(new Vector2(282, 36), 8, 8);
            hint.Text = "Drag: paint | Middle click: pick\nColumn tiles are protected.";
            Resize(gui);
        }

        private Label AddLabel(Vector2 size, float x, float y)
        {
            var label = new Label(size, 9);
            label.Pos = new Vector2(x, y);
            panel.AddChild(label);
            return label;
        }

        private Button AddButton(string caption, float x, float y, float width, Action action)
        {
            var button = new Button(new Vector2(width, 24), caption, null, delegate { action(); });
            button.Pos = new Vector2(x, y);
            panel.AddChild(button);
            return button;
        }

        public override void Resize(GUI gui)
        {
            panel.Resize(300, Math.Max(266, Math.Min(500, gui.Size.Y - 134)));
            preview.Pos.Y = panel.Size.Y - 76;
            selectionLabel.Pos.Y = panel.Size.Y - 48;
            rotateButton.Pos.Y = flipButton.Pos.Y = undoButton.Pos.Y = panel.Size.Y - 78;
            paintButton.Pos.Y = pickButton.Pos.Y = panel.Size.Y - 108;
            int rows = Math.Max(1, Math.Min(4, (int)((panel.Size.Y - 200) / 68)));
            pageSize = rows * 4;
            for (int i = 0; i < tiles.Count; i++)
                tileFrames[i].Pos = new Vector2(8 + (i % 4) * 70, panel.Size.Y - 188 - (i / 4) * 68);
            page = selectedTexture / pageSize;
            if (paletteLevel != null && paletteLevel == level) RefreshPalette();
        }

        private int TextureCount { get { return level.Atlas.NumTexturesX * level.Atlas.NumTexturesY; } }

        private void ChangePage(int delta)
        {
            page = Math.Max(0, Math.Min((TextureCount - 1) / pageSize, page + delta));
            RefreshPalette();
        }

        private void TileClicked(ImageButton tile)
        {
            selectedTexture = (int)tile.Tag;
            picking = false;
            RefreshSelection();
        }

        private void RefreshPalette()
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                int id = page * pageSize + i;
                tileFrames[i].Visible = i < pageSize && id < TextureCount;
                if (!tileFrames[i].Visible) continue;
                tiles[i].Tag = id;
                tiles[i].Icon.SetBitmap(level.Atlas.Get(id));
            }
            pageLabel.Text = "Page " + (page + 1) + "/" + ((TextureCount + pageSize - 1) / pageSize);
            RefreshSelection();
        }

        private void RefreshSelection()
        {
            if (level == null) return;
            selectionLabel.Text = "Tile " + selectedTexture + " | Transform " + modification;
            preview.Icon.SetBitmap(level.Atlas.Get(selectedTexture, modification));
            foreach (var frame in tileFrames) frame.BorderColor = Color.FromArgb(63, 63, 70);
            int index = selectedTexture - page * pageSize;
            if (index >= 0 && index < pageSize) tileFrames[index].BorderColor = Config.UI_COLOR_BLUE;
            paintButton.BackgroundColor1 = picking ? Color.FromArgb(45, 45, 48) : Config.UI_COLOR_BLUE;
            pickButton.BackgroundColor1 = picking ? Config.UI_COLOR_BLUE : Color.FromArgb(45, 45, 48);
        }

        private void TransformSelection(bool rotate)
        {
            // Compose with the eight rotations/reflections used by the level format.
            int[] rotation = { 5, 4, 7, 6, 2, 3, 0, 1 };
            int[] flip = { 1, 0, 3, 2, 6, 7, 4, 5 };
            modification = (rotate ? rotation : flip)[modification];
            RefreshSelection();
        }

        public override void Update(float time, float dTime)
        {
            MouseUsed = Window.MouseLeftDown || Window.MouseMiddleDown;
            if (!Window.MouseLeftDown) EndStroke();
            bool sample = Window.MouseMiddleDown || (picking && Window.MouseLeftDown);
            if (!sample) sampleHeld = false;
            Vector3 start = EngineBase.Renderer.Unproject(Window.MousePos, 0);
            Vector3 end = EngineBase.Renderer.Unproject(Window.MousePos, 1);
            var hit = level.Collisions.RayCast(start, (end - start).Normalized());
            if (!hit.Hit || hit.Position.X < 0 || hit.Position.Z < 0 || hit.Position.X >= level.Data.Width || hit.Position.Z >= level.Data.Height)
            {
                level.RemoveNode(cursor);
                lastPainted = null;
                base.Update(time, dTime);
                return;
            }
            MapEntry entry = hit.PositionEntry;
            float h = entry.Height;
            cursor.Configure(0, level.Terrain.GetMapEntry(entry.X + 1, entry.Z).Height - h,
                level.Terrain.GetMapEntry(entry.X + 1, entry.Z + 1).Height - h,
                level.Terrain.GetMapEntry(entry.X, entry.Z + 1).Height - h, 1,
                entry.Column == null ? new Vector4(0, 0.8f, 1, 0.25f) : new Vector4(1, 0.2f, 0, 0.25f));
            cursor.Position = new Vector3(entry.X, h + 0.02f, entry.Z);
            level.AddNode(cursor);
            if (sample && !sampleHeld)
            {
                selectedTexture = Math.Max(0, Math.Min(TextureCount - 1, entry.TextureId));
                modification = entry.TextureModification & 7;
                page = selectedTexture / pageSize;
                RefreshPalette();
                sampleHeld = true;
                EndStroke();
            }
            else if (!sample && !picking && Window.MouseLeftDown)
            {
                if (!painting) { undo.Clear(); painting = true; }
                PaintTo(entry);
            }
            base.Update(time, dTime);
        }

        private void PaintTo(MapEntry entry)
        {
            // Fill skipped cells during fast drags, without bridging across the UI.
            int x = lastPainted == null ? entry.X : lastPainted.X;
            int z = lastPainted == null ? entry.Z : lastPainted.Z;
            int dx = Math.Abs(entry.X - x), dz = Math.Abs(entry.Z - z);
            int sx = x < entry.X ? 1 : -1, sz = z < entry.Z ? 1 : -1, error = dx - dz;
            while (true)
            {
                PaintCell(level.Data.Map[x, z]);
                if (x == entry.X && z == entry.Z) break;
                int twice = 2 * error;
                if (twice > -dz) { error -= dz; x += sx; }
                if (twice < dx) { error += dx; z += sz; }
            }
            lastPainted = entry;
        }

        private void PaintCell(MapEntry entry)
        {
            // Replacing the negative column reference would delete the building.
            if (entry.Column != null) return;
            if (entry.TextureId == selectedTexture && entry.TextureModification == modification) return;
            if (!undo.ContainsKey(entry)) undo.Add(entry, new TextureState { Id = entry.TextureId, Modification = entry.TextureModification });
            entry.TextureId = selectedTexture;
            entry.TextureModification = modification;
            level.Terrain.ApplyTexture(entry);
        }

        private void UndoStroke()
        {
            EndStroke();
            foreach (var item in undo)
            {
                item.Key.TextureId = item.Value.Id;
                item.Key.TextureModification = item.Value.Modification;
                level.Terrain.ApplyTexture(item.Key);
            }
            undo.Clear();
        }

        private void EndStroke() { painting = false; lastPainted = null; }

        public override void SuspendMouse()
        {
            EndStroke();
            sampleHeld = false;
            level.RemoveNode(cursor);
            base.SuspendMouse();
        }

        protected override void enable()
        {
            if (paletteLevel != level)
            {
                paletteLevel = level;
                selectedTexture = modification = page = 0;
                undo.Clear();
            }
            RefreshPalette();
            panel.Visible = true;
        }

        protected override void disable() { SuspendMouse(); }
    }
}
