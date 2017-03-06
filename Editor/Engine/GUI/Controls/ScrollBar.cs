using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{

    public class ScrollBar : Control
    {
        public Color BackgroundColor;

        private Button buttonScrollUp;
        private Button buttonScrollDown;
        private ScrollBarGrip grip;

        private float gripWidth;
        private float lastScrollTime = 0;
        private float updateTime = 0;

        private float repeatDelay = 0.04f; // seconds

        public bool ScrollDown
        {
            get
            {
                if (buttonScrollDown.MouseDown && updateTime > lastScrollTime + repeatDelay)
                {
                    lastScrollTime = updateTime;
                    return true;
                }
                return false;
            }
        }

        public bool ScrollUp
        {
            get
            {
                if (buttonScrollUp.MouseDown && updateTime > lastScrollTime + repeatDelay)
                {
                    lastScrollTime = updateTime;
                    return true;
                }
                return false;
            }
        }

        private float scrollPos;

        public float ScrollPos
        {
            set
            {
                scrollPos = Math.Max(0, Math.Min(1, value));
                float d = Size.Y - grip.Size.Y - buttonScrollUp.Size.Y;
                grip.Pos.Y = d - (d - buttonScrollDown.Size.Y) * scrollPos;
                ScrollPosChanged = true;
            }
            get { return scrollPos; }
        }

        public bool ScrollPosChanged { get; private set; }

        public ScrollBar(Vector2 size) : base("scrollbar", size)
        {
            MouseEnabled = true;
            BackgroundColor = Color.FromArgb(62, 62, 66);
            gripWidth = Math.Max(3.0f, Size.X - 10);
            ScrollPosChanged = false;

            buttonScrollUp = new Button(new Vector2(Size.X, Size.X));
            if (buttonScrollUp.Ready)
            {
                buttonScrollUp.BackgroundColor1 = Color.FromArgb(104, 104, 104);
                buttonScrollUp.BackgroundColor2 = Color.FromArgb(51, 153, 255);
                buttonScrollUp.BackgroundColor3 = Color.FromArgb(140, 140, 140);
                buttonScrollUp.BorderSize = 4;
                AddChild(buttonScrollUp);
            }
            else Ready = false;

            buttonScrollDown = new Button(new Vector2(Size.X, Size.X));
            if (buttonScrollDown.Ready)
            {
                buttonScrollDown.BackgroundColor1 = Color.FromArgb(104, 104, 104);
                buttonScrollDown.BackgroundColor2 = Color.FromArgb(51, 153, 255);
                buttonScrollDown.BackgroundColor3 = Color.FromArgb(140, 140, 140);
                buttonScrollDown.BorderSize = 4;
                AddChild(buttonScrollDown);
            }
            else Ready = false;

            grip = new ScrollBarGrip(new Vector2(gripWidth, 100));
            if (grip.Ready)
            {
                grip.CornerRound = 5;
                AddChild(grip);
            }
            else Ready = false;

            if (Ready) layout();
        }

        private void layout()
        {
            buttonScrollUp.Pos.X = 0;
            buttonScrollUp.Pos.Y = Size.Y - buttonScrollUp.Size.Y;

            buttonScrollDown.Pos.X = 0;
            buttonScrollDown.Pos.Y = 0;

            grip.Pos.X = (Size.X - gripWidth) * 0.5f;
            grip.Pos.Y = Size.Y - grip.Size.Y - buttonScrollDown.Size.Y;
        }

        public override void Update(float time, float dtime)
        {
            updateTime = time;

            base.Update(time, dtime);

            if (grip.MouseDown)
            {
                if (Math.Abs(grip.DeltaY) > 0)
                {
                    ScrollPos -= grip.DeltaY / (Size.Y - grip.Size.Y - buttonScrollUp.Size.Y - buttonScrollDown.Size.Y);
                }
                grip.DeltaY = 0;
            }
        }

        public override void Resize(float w, float h)
        {
            base.Resize(w, h);
            layout();
        }

        public override void Draw(Vector2 resolution)
        {
            base.Draw(resolution);
            ScrollPosChanged = false;
        }

        protected override void ApplyUniforms()
        {
            material.SetUniform("backgroundColor", BackgroundColor);
        }
    }
}
