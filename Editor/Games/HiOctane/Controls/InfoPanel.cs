using LevelEditor.Engine.GUI.Controls;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor.Games.HiOctane.Controls
{
    public class InfoPanel : Panel
    {
        public Label Headline;
        public Label LevelName;

        public InfoPanel() : base(new Vector2(300, 80))
        {
            Name = "Infopanel";
            Alpha = 0.8f;
            MouseEnabled = true; // avoid click-through
            BorderColor = Color.FromArgb(63, 63, 70);
            BorderSize = 1;

            Headline = new Label(new Vector2(Size.X - 10, 30), 20, System.Drawing.Text.TextRenderingHint.AntiAlias);
            Headline.Pos = new Vector2(9, 12);
            Headline.TextColor = Config.UI_COLOR_BLUE;
            Headline.Text = "INSPECT";
            AddChild(Headline);

            LevelName = new Label(new Vector2(Size.X - 12, 30), 12);
            LevelName.Pos = new Vector2(12, 34);
            LevelName.Text = "Level";
            AddChild(LevelName);

        }

        protected override void ApplyUniforms()
        {
            base.ApplyUniforms();
        }
    }
}
