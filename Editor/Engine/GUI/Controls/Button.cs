using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;

namespace LevelEditor.Engine.GUI.Controls
{
    public class Button : Control
    {
        public Color BackgroundColor1;
        public Color BackgroundColor2;
        public Color BackgroundColor3;
        public Color BorderColor;
        public float BorderSize;

        private Action<Button> callbackDown;
        private Action<Button> callbackUp;
        private Action<Vector2> callbackDrag;

        private Label label;

        public Button(Vector2 size, string caption, Action<Button> cbDown = null, Action<Button> cbUp = null, Action<Vector2> cbDrag = null) : this(size, cbDown, cbUp, cbDrag)
        {
            label = new Label(new Vector2(size.X - 8, 12), 9);
            label.Text = caption;
            label.Pos.X = (float)Math.Floor(0.5f * (size.X - label.TextSize.Width));
            label.Pos.Y = (float)Math.Floor(0.5f * (size.Y - label.TextSize.Height)) + 3;
            AddChild(label);
        }

        public Button(Vector2 size, Action<Button> cbDown = null, Action<Button> cbUp = null, Action<Vector2> cbDrag = null) : base("button", size)
        {
            SnapToPixel = true;
            MouseEnabled = true;
            BackgroundColor1 = Color.FromArgb(45, 45, 48);
            BackgroundColor2 = Color.FromArgb(75, 75, 78);
            BackgroundColor3 = Color.FromArgb(63, 63, 70);
            BorderColor = Color.FromArgb(63, 63, 70);
            BorderSize = 2.0f;
            callbackDown = cbDown;
            callbackUp = cbUp;
            callbackDrag = cbDrag;
        }

        protected override void OnMouseDown()
        {
            if (callbackDown != null) callbackDown(this);
        }

        protected override void OnMouseUp()
        {
            if (callbackUp != null) callbackUp(this);
        }

        protected override void OnMouseDrag(Vector2 delta)
        {
            if (callbackDrag != null) callbackDrag(delta);
        }

        protected override void ApplyUniforms()
        {
            material.SetUniform("backgroundColor", MouseDown ? BackgroundColor2 : MouseOver ? BackgroundColor3 : BackgroundColor1);
            material.SetUniform("borderColor", BorderColor);
            material.SetUniform("borderSize", BorderSize);
        }
    }
}
