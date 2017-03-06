using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{
    public class ScrollBarGrip : Panel
    {
        public float DeltaY; // gets reset to 0 by ScrollBar class

        public Color BackgroundColor1 = Color.FromArgb(104, 104, 104);
        public Color BackgroundColor2 = Color.FromArgb(51, 153, 255);
        public Color BackgroundColor3 = Color.FromArgb(140, 140, 140);

        public ScrollBarGrip(Vector2 size) : base(size)
        {
            MouseEnabled = true;
            BorderSize = 0;
            BackgroundColor = BackgroundColor1;
        }

        protected override void OnMouseDown()
        {
            DeltaY = 0;
        }

        protected override void OnMouseUp()
        {
            DeltaY = 0;
        }

        protected override void OnMouseDrag(Vector2 delta)
        {
            DeltaY = delta.Y;
        }

        protected override void ApplyUniforms()
        {
            BackgroundColor = MouseDown ? BackgroundColor2 : MouseOver ? BackgroundColor3 : BackgroundColor1;
            base.ApplyUniforms();
        }
    }
}
