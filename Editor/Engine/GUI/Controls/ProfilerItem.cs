using System.Collections.Generic;
using System.Text;
using OpenTK;
using System.Drawing;
using System.Diagnostics;
using System;

namespace LevelEditor.Engine.GUI.Controls
{
    public class ProfilerItem : Control
    {
        public Color BackgroundColor;
        public Color BorderColor;
        public float BorderSize;
        public float CornerRound;

        public string Section { get; private set; }
        public float Scale;

        public bool Active { get { return idleCounter < Size.X; } }

        private Stopwatch timer = new Stopwatch();
        private List<double> samples = new List<double>();
        private double maxValue = 0;
        private bool adapt;

        private Label caption;
        private Label valueLabel;
        private Label maxLabel;
        private Label avgLabel;
        private DynamicImage plot;

        private int idleCounter = 0;

        public ProfilerItem(Vector2 size, string section, bool adaptScale, float scale) : base("panel", size)
        {
            Name = "ProfilerItem " + section;
            Section = section;
            Alpha = 0;

            Scale = scale;
            adapt = adaptScale;

            BackgroundColor = Color.FromArgb(0, 45, 45, 48);
            BorderColor = Color.FromArgb(63, 63, 70);
            BorderSize = 2.0f;
            CornerRound = 0.0f;

            caption = new Label(new Vector2(size.X, 20), 8);
            caption.Pos.X = 4;
            caption.Pos.Y = size.Y - caption.Size.Y - 4;
            caption.Text = section;
            AddChild(caption);

            valueLabel = new Label(new Vector2(100, 20), 8);
            //valueLabel.Shadow = true;
            valueLabel.Pos.X = size.X - valueLabel.Size.X;
            valueLabel.Pos.Y = caption.Pos.Y;
            AddChild(valueLabel);

            plot = new DynamicImage(new Vector2(size.X - 2 * BorderSize, size.Y - 22));
            plot.Pos.X = BorderSize;
            plot.Pos.Y = BorderSize;
            plot.Clear(Color.Blue);
            AddChild(plot);

            maxLabel = new Label(new Vector2(80, 20), 7, System.Drawing.Text.TextRenderingHint.AntiAliasGridFit);
            maxLabel.Shadow = true;
            maxLabel.TextColor = Color.DarkRed;
            maxLabel.Pos.X = 4;
            maxLabel.Pos.Y = size.Y - 42;
            AddChild(maxLabel);

            avgLabel = new Label(new Vector2(80, 10), 7, System.Drawing.Text.TextRenderingHint.AntiAliasGridFit);
            avgLabel.Shadow = true;
            //avgLabel.BackgroundColor = Color.FromArgb(150, Config.UI_COLOR_BLUE);
            avgLabel.TextColor = Config.UI_COLOR_BLUE;
            AddChild(avgLabel);

            samples.Add(0);
        }

        public void Start()
        {
            idleCounter = 0;
            timer.Reset();
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            idleCounter = 0;

            double value = timer.Elapsed.TotalMilliseconds;
            samples.Add(value);
            if (samples.Count > Size.X) samples.RemoveAt(0);

            valueLabel.Text = timer.Elapsed.TotalMilliseconds.ToString("0.000") + "ms";
            valueLabel.Pos.X = Size.X - valueLabel.TextSize.Width - 4;

            updateTexture();
        }

        private void updateTexture()
        {
            plot.Clear(Color.Black);

            Pen pen = new Pen(Color.Black, 1);

            maxValue = 0;
            float h;
            float maxHeight = 0.1f;
            int numSamples = samples.Count;
            double avgValue = 0;

            for (int x = 0; x < numSamples; x++)
            {
                double value = samples[x];
                avgValue += value;
                maxValue = Math.Max(maxValue, value);

                h = (float)Math.Floor(Scale * value);
                maxHeight = Math.Max(maxHeight, h);
                h = Math.Min(plot.Size.Y, h);

                float rel = h / plot.Size.Y;

                pen.Color = Color.FromArgb(255, (int)(255 * rel), (int)(255 * (1 - rel)), 0);

                int lx = x + (int)plot.Size.X - numSamples;
                plot.DrawLine(lx, 0, lx, (int)h, pen);
            }

            // average line
            avgValue = avgValue / numSamples;
            float avgHeight = (float)Math.Floor(Scale * avgValue);
            h = Math.Min(avgHeight, plot.Size.Y);

            pen.Width = 2;
            pen.Color = Color.FromArgb(150, Config.UI_COLOR_BLUE);
            plot.DrawLine(0, (int)h, (int)plot.Size.X, (int)h, pen);

            // update average label position and text
            avgLabel.Text = avgValue.ToString("0.000") + "ms";
            avgLabel.Pos.X = plot.Size.X - avgLabel.TextSize.Width;
            avgLabel.Pos.Y = plot.Pos.Y + Math.Min(h, plot.Size.Y - 10);

            // update maximum label text
            maxLabel.Text = maxValue.ToString("0.000") + "ms";

            // slowly adapt scale in both directions
            if (adapt)
            {
                if (avgHeight > plot.Size.Y * 0.4f) Scale *= 0.8f;
                if (avgHeight < plot.Size.Y * 0.25f) Scale += 1.2f;
                Scale = Math.Max(0.01f, Scale);
            }
        }

        public override void Update(float time, float dtime)
        {
            if (!Visible) return;

            Alpha += ((idleCounter > Size.X * 0.8f ? 0 : 1) - Alpha) * 6 * dtime;

            foreach (Control control in Children)
            {
                control.Alpha = Alpha;
                control.Update(time, dtime);
            }

            //base.Update(time, dtime);
            idleCounter++;

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
