using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{
    public class FloatPanel : Panel
    {
        private Panel titleBar;
        private ResizeGrip grip;
        private Label title;
        public bool AllowResize;
        public bool AllowDragging;

        public Vector2 MinSize = new Vector2(250, 100);

        public FloatPanel(Vector2 size, string caption, bool allowDragging = true, bool allowResize = true) : base(size)
        {
            MouseEnabled = true; // avoid click-through

            AllowResize = allowResize;
            AllowDragging = allowDragging;

            BackgroundColor = Color.FromArgb(37, 37, 38);
            BorderColor = Color.FromArgb(63, 63, 70);
            BorderSize = 1;

            titleBar = new Panel(new Vector2(size.X, 20));
            titleBar.BorderColor = Color.FromArgb(63, 63, 70);
            titleBar.BorderSize = 1;
            titleBar.BackgroundColor = Config.UI_COLOR_ORANGE;
            AddChild(titleBar);

            grip = new ResizeGrip(new Vector2(20, 20), "gui/images/winResizeGrip.png", resizeGripDragged);
            grip.Visible = allowResize;
            AddChild(grip);

            title = new Label(new Vector2(size.X - 8, 16), 9);
            title.Text = caption;
            title.Pos.X = 4;
            title.TextColor = Color.White;
            title.BackgroundColor = titleBar.BackgroundColor;
            AddChild(title);

            layout();
        }

        private void resizeGripDragged(Vector2 delta)
        {
            Vector2 newSize = new Vector2(Size.X + delta.X, Size.Y - delta.Y);
            newSize.X = Math.Max(newSize.X, MinSize.X);
            newSize.Y = Math.Max(newSize.Y, MinSize.Y);

            delta.Y = Size.Y - newSize.Y;
            Resize(newSize.X, newSize.Y);
            Pos.Y += delta.Y;
        }

        private void layout()
        {
            titleBar.Pos.Y = Size.Y - titleBar.Size.Y;
            title.Pos.Y = Size.Y - 19;
            grip.Pos.X = Size.X - grip.Size.X;
        }

        protected override void OnMouseDrag(Vector2 delta)
        {
            if (AllowDragging) Pos += delta;
        }

        public override void Resize(float w, float h)
        {
            base.Resize(w, h);

            titleBar.Resize(Size.X, titleBar.Size.Y);

            layout();
        }

        public override void Update(float time, float dTime)
        {
            grip.Visible = AllowResize;

            if (AllowDragging)
            {
                //NOTE: menu and status bar margins are hardcoded here

                if (Pos.X < 3) Pos.X += (3f - Pos.X) * 10f * dTime;
                if (Pos.Y < 25) Pos.Y += (25f - Pos.Y) * 10f * dTime;

                float target = Parent.Size.X - Size.X - 3;
                if (Pos.X > target) Pos.X += (target - Pos.X) * 10f * dTime;

                target = Parent.Size.Y - Size.Y - 35;
                if (Pos.Y > target) Pos.Y += (target - Pos.Y) * 10f * dTime;


                Pos.X = (float)Math.Round(Pos.X);
                Pos.Y = (float)Math.Round(Pos.Y);
            }

            base.Update(time, dTime);
        }
    }
}
