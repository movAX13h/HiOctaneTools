using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using LevelEditor.Engine;
using LevelEditor.Engine.GUI.Controls;
using LevelEditor.Engine.Models;
using LevelEditor.Utils;
using LevelEditor.Engine.Core;
using LevelEditor.Games.HiOctane.Models;
using LevelEditor.Games.HiOctane.Resources;
using LevelEditor.Engine.Resources;
using System.Drawing;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class BuildingsMode : EditMode
    {
        private BuildingsCursor cursor;
        private List<Vector4> cursorColors;

        //private FloatPanel selectionPanel;
        //private ListBox columnsList;
        //private ListBox blocksList;

        private FloatPanel editPanel;

        private MapEntry selectedEntry;

        private Bitmap emptyBlockTopBitmap;
        private Bitmap emptyBlockBottomBitmap;
        private ImageButton[] blockButtons;

        private Label columnInfo;
        private Button editPanelClose;

        public BuildingsMode() : base("Buildings")
        {
            showWalls = false;
            showWaypoints = false;
            showBuildings = true;
            showEntities = false;
            showTerrain = true;

            fadeTerrain = true;

            cursor = new BuildingsCursor(new Vector4(1, 1, 0, 1));
            cursorColors = new List<Vector4>();
            cursorColors.Add(new Vector4(1, 1, 0, 0.98f));
            cursorColors.Add(new Vector4(0, 1, 0, 0.5f));
        }

        public override void CreateControls(GUI gui)
        {
            /*
            selectionPanel = new FloatPanel(new Vector2(300, 200), "DEFINITIONS", false, false);
            selectionPanel.Pos.X = 0;
            selectionPanel.Pos.Y = 22;
            selectionPanel.Visible = false;
            gui.AddChild(selectionPanel);

            columnsList = new ListBox(new Vector2(120, selectionPanel.Size.Y - 24), 1, columnItemSelected);
            columnsList.Pos.X = 2;
            columnsList.Pos.Y = 2;
            selectionPanel.AddChild(columnsList);

            blocksList = new ListBox(new Vector2(120, selectionPanel.Size.Y - 24), 1, blockItemSelected);
            blocksList.Pos.X = 124;
            blocksList.Pos.Y = 2;
            selectionPanel.AddChild(blocksList);
            */

            editPanel = new FloatPanel(new Vector2(400, 400), "COLUMN CONFIGURATION", false, false);
            editPanel.MouseEnabled = false;
            editPanel.Pos.Y = gui.Size.Y - editPanel.Size.Y - 100;
            editPanel.Visible = false;
            gui.AddChild(editPanel);

            emptyBlockTopBitmap = new Bitmap(Config.DATA_FOLDER + "gui/images/isoCubeTop.png");
            emptyBlockBottomBitmap = new Bitmap(Config.DATA_FOLDER + "gui/images/isoCubeBottom.png");

            blockButtons = new ImageButton[16];
            for (int i = 0; i < 8; i++)
            {
                TextureResource tex = new TextureResource("blockButtonTextureT-" + i, emptyBlockTopBitmap);
                ImageButton btn = new ImageButton(new Vector2(64, 64), tex, null, blockButtonClicked);
                btn.Tag = i;
                btn.Pos.X = 30;
                btn.Pos.Y = 30 + 40 * i;
                editPanel.AddChild(btn);
                blockButtons[i * 2] = btn;

                tex = new TextureResource("blockButtonTextureB-" + i, emptyBlockBottomBitmap);
                btn = new ImageButton(new Vector2(64, 64), tex, null, blockButtonClicked);
                btn.Tag = i;
                btn.Pos.X = editPanel.Size.X - btn.Size.X - 30;
                btn.Pos.Y = 30 + 40 * i;
                editPanel.AddChild(btn, 0);

                blockButtons[i * 2 + 1] = btn;
            }

            columnInfo = new Label(new Vector2(200, 14), 9);
            columnInfo.Pos.X = 120;
            columnInfo.Pos.Y = editPanel.Size.Y - 70;
            editPanel.AddChild(columnInfo);

            editPanelClose = new Button(new Vector2(100, 22), "DONE", null, closeEditPanel);
            editPanelClose.BorderSize = 1;
            editPanelClose.Pos.X = (float)Math.Floor(0.5f * (editPanel.Size.X - editPanelClose.Size.X));
            editPanelClose.Pos.Y = 50;
            editPanel.AddChild(editPanelClose);
        }

        private void closeEditPanel(Button obj)
        {
            editPanel.Visible = false;
            selectedEntry = null;
        }

        private void blockButtonClicked(ImageButton btn)
        {

        }

        private void blockItemSelected(ListItem obj)
        {

        }

        private void columnItemSelected(ListItem obj)
        {

        }

        public override void Resize(GUI gui)
        {
            /*
            selectionPanel.Resize(selectionPanel.Size.X, gui.Size.Y - 134);

            columnsList.Resize(columnsList.Size.X, selectionPanel.Size.Y - 24);
            blocksList.Resize(blocksList.Size.X, selectionPanel.Size.Y - 24);
            */

            //layout(gui);
        }
        private void layout(GUI gui)
        {
            //editPanel.Pos.X = (float)Math.Floor(0.5f * (gui.Size.X - editPanel.Size.X));
            //editPanel.Pos.Y = (float)Math.Floor(0.5f * (gui.Size.Y - editPanel.Size.Y));
        }

        public override void Update(float time, float dTime)
        {
            MouseUsed = Window.MouseLeftDown;

            if (selectedEntry != null) return;

            Vector3 rayStart = EngineBase.Renderer.Unproject(Window.MousePos, 0f);
            Vector3 rayEnd = EngineBase.Renderer.Unproject(Window.MousePos, 1f);
            Vector3 dir = (rayEnd - rayStart).Normalized();

            Editor.Profiler.Begin("RAYCAST");
            RayCastResult rc = level.Collisions.RayCast(rayStart, dir);
            Editor.Profiler.End("RAYCAST");

            if (rc.Hit)
            {
                level.AddNode(cursor);

                float h = rc.PositionOfEntry.Y;
                int mode = rc.PositionEntry.Column == null ? 1 : 0;

                cursor.Configure(0,
                    level.Terrain.GetMapEntry(rc.PositionEntry.X + 1, rc.PositionEntry.Z).Height - h,
                    level.Terrain.GetMapEntry(rc.PositionEntry.X + 1, rc.PositionEntry.Z + 1).Height - h,
                    level.Terrain.GetMapEntry(rc.PositionEntry.X, rc.PositionEntry.Z + 1).Height - h,
                    mode, cursorColors[mode]
                );

                cursor.Position = rc.PositionOfEntry;

                if (Window.MouseLeftDown && selectedEntry == null)
                {
                    selectedEntry = rc.PositionEntry;
                    level.FocusMapEntry(rc.PositionEntry);
                    showEditPanel();
                    /*
                    foreach(ListItem item in columnsList.Items)
                    {
                        KeyValuePair<int, ColumnDefinition> pair = (KeyValuePair<int, ColumnDefinition>)item.Tag;
                        if (pair.Key == selectedEntry.Column.ID)
                        {
                            item.Selected = true;
                            return;
                        }
                    }
                    */
                    //columnsList.SelectItemByTag(selectedEntry.Column);
                }

                //if (Window.MouseLeftDown) editPanel.Visible = true;
            }
            else level.RemoveNode(cursor);

            base.Update(time, dTime);
        }

        private void showEditPanel()
        {
            for (int i = 0; i < 8; i++)
            {
                ImageButton btn1 = blockButtons[i * 2];
                ImageButton btn2 = blockButtons[i * 2 + 1];

                Bitmap bmp1 = emptyBlockTopBitmap;
                Bitmap bmp2 = emptyBlockBottomBitmap;

                if (selectedEntry.Column != null)
                {
                    int blockId = selectedEntry.Column.Blocks[i];
                    if (blockId > 0)
                    {
                        BlockDefinition block = level.Data.BlockDefinitions.GetValue(blockId);
                        bmp1 = block.PreviewImageTop(level.Atlas, new Size(128, 128));
                        bmp2 = block.PreviewImageBottom(level.Atlas, new Size(128, 128));
                    }
                }

                btn1.Icon.SetBitmap(bmp1);
                btn2.Icon.SetBitmap(bmp2);
            }

            columnInfo.Text = selectedEntry.Column == null ? "This spot contains no column." : "Column: " + selectedEntry.Column.ID.ToString();

            editPanel.Visible = true;
        }

        protected override void enable()
        {
            /*
            columnsList.Clear();
            columnsList.Lock();
            foreach (var column in level.Data.ColumnDefinitions)
            {
                columnsList.AddItem("Column " + column.Value.ID.ToString(), column);
            }
            columnsList.Unlock();

            blocksList.Clear();
            blocksList.Lock();
            foreach (var block in level.Data.BlockTexDefinitions)
            {
                blocksList.AddItem("Block " + block.Value.ID.ToString(), block);
            }
            blocksList.Unlock();

            selectionPanel.Visible = true;
            */
            level.AddNode(cursor);
        }

        protected override void disable()
        {
            selectedEntry = null;
            //selectionPanel.Visible = false;
            level.RemoveNode(cursor);
        }

    }
}
