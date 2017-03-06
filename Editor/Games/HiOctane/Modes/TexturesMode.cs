using LevelEditor.Engine.GUI.Controls;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class TexturesMode : EditMode
    {
        private FloatPanel panel;

        public TexturesMode() : base("Textures")
        {
            showWalls = false;
            showWaypoints = false;
            showBuildings = false;
            showEntities = false;
            showTerrain = false;

            morphsEnabled = false;
        }

        public override void CreateControls(GUI gui)
        {
            panel = new FloatPanel(new Vector2(400, 200), "MY SUPER FLOATING PANEL");
            panel.Pos.X = 10;
            panel.Pos.Y = 40;
            panel.Visible = false;
            gui.AddChild(panel);

            //ListBox list = new ListBox(new Vector2(panel.Size.X, panel.Size.Y - 20));
            //gui.AddChild(list);
        }
        public override void Resize(GUI gui)
        {
            //win.Pos.X = (float)Math.Floor(0.5f * (gui.Size.X - win.Size.X));
            //win.Pos.Y = (float)Math.Floor(0.5f * (gui.Size.Y - win.Size.Y));

        }

        protected override void enable()
        {
            panel.Visible = true;
        }

        protected override void disable()
        {
            panel.Visible = false;
        }
    }
}
