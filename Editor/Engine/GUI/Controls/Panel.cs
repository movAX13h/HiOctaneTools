using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using System.Drawing;

namespace LevelEditor.Engine.GUI.Controls
{
    public class Panel : Control
    {
        public Color BackgroundColor;
        public Color BorderColor;
        public float BorderSize;
        public float CornerRound;


        public Panel(Vector2 size):base("panel", size)
        {
            BackgroundColor = Color.FromArgb(45, 45, 48);
            BorderColor = Color.FromArgb(63, 63, 70);
            BorderSize = 0.0f;
            CornerRound = 0.0f;
        }

        protected override void ApplyUniforms()
        {
            material.SetUniform("backgroundColor", BackgroundColor);
            material.SetUniform("borderColor", BorderColor);
            material.SetUniform("borderSize", BorderSize);
            material.SetUniform("cornerRound", CornerRound);
        }
    }
}
