using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using System.Drawing;

namespace LevelEditor.Engine.GUI.Controls
{
    public class Slider : Panel
    {
        private Label caption;
        private Label valueLabel;
        private Button knob;
        private Button track;
        private Action<float> callbackChanged;

        private float value = 0;

        public float Minimum = 0;
        public float Maximum = 1;

        public float Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = Math.Max(Minimum, Math.Min(Maximum, value));
                updateValueText();
                updateKnobPosition();
            }
        }

        public Slider(string text, float width, Action<float> cbChanged = null) : base(new Vector2(width, 34))
        {
            callbackChanged = cbChanged;

            MouseEnabled = true;

            value = 0;

            BackgroundColor = Color.Black;
            Alpha = 0.2f;

            caption = new Label(new Vector2(width-8, 14), 9);
            caption.Pos.X = 2;
            caption.Pos.Y = 18;
            caption.Text = text;
            AddChild(caption);

            valueLabel = new Label(new Vector2(60, 14), 9);
            valueLabel.Pos.X = width - 60;
            valueLabel.Pos.Y = 18;
            valueLabel.Text = "0";
            AddChild(valueLabel);

            track = new Button(new Vector2(width - 8, 10), trackClicked);
            track.BackgroundColor1 = Color.Black;
            track.BackgroundColor2 = Color.Black;
            track.BackgroundColor3 = Color.Black;
            track.BorderSize = 1;
            track.Alpha = 0.4f;
            track.Pos.X = 4;
            track.Pos.Y = 4;
            AddChild(track);

            knob = new Button(new Vector2(10, 10), null, null, knobDrag);
            knob.BorderSize = 1;
            knob.BackgroundColor1 = Color.Gray;
            knob.BackgroundColor2 = Color.White;
            knob.BackgroundColor3 = Color.LightGray;
            knob.Pos.X = 0;
            knob.Pos.Y = 0;
            track.AddChild(knob);
        }

        private void trackClicked(Button btn)
        {
            setValueFromNewKnobPosition(track.MousePosLocal.X - track.Pos.X);
        }

        private void updateKnobPosition()
        {
            float w = track.Size.X - knob.Size.X;
            float relValue = (Value - Minimum) / (Maximum - Minimum);
            knob.Pos.X = relValue * w;
        }

        private void updateValueText()
        {
            valueLabel.Text = value.ToString("0.00");
            valueLabel.Pos.X = Size.X - valueLabel.TextSize.Width - 2;
        }

        private void setValueFromNewKnobPosition(float x)
        {
            float end = track.Size.X - knob.Size.X;
            knob.Pos.X = (float)Math.Floor(Math.Min(end, Math.Max(0, x - 0.5f * knob.Size.X)));
            float rel = knob.Pos.X / end;
            value = Minimum + rel * (Maximum - Minimum);
            updateValueText();
            MouseConsumed = true;
            if (callbackChanged != null) callbackChanged(value);
        }

        private void knobDrag(Vector2 delta)
        {
            Vector2 mouse = Window.MousePos - track.WorldPositionOffset();
            setValueFromNewKnobPosition(mouse.X - track.Pos.X);
        }

    }
}
