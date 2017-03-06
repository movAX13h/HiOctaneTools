using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using LevelEditor.Utils;

namespace LevelEditor.Engine.GUI.Controls
{
    public class Profiler : Control
    {
        public Color BackgroundColor = Color.FromArgb(45, 45, 48);
        public Color BorderColor = Color.FromArgb(63, 63, 70);
        public float BorderSize = 2;
        public float CornerRound = 0;

        private OrderedDictionary<string, ProfilerItem> items;

        public Profiler() : base("panel", new Vector2(200, 600))
        {
            Name = "Profiler";
            Alpha = 0;
            items = new OrderedDictionary<string, ProfilerItem>();
        }

        public void Begin(string name, float scale)
        {
            Begin(name, false, scale);
        }

        public void Begin(string name, bool adapt = true, float scale = 500)
        {
            if (!Visible) return;

            if (!items.ContainsKey(name))
            {
                ProfilerItem item = new ProfilerItem(new Vector2(Size.X, 60), name, adapt, scale);
                AddChild(item);
                items.Add(name, item);
            }

            items[name].Start();
        }

        public void End(string name)
        {
            if (!items.ContainsKey(name)) return; //throw new Exception("Trying to stop non-existant ProfilerItem!");

            items[name].Stop();
        }

        public override void Update(float time, float dtime)
        {
            if (!Visible) return;
            updateItems(dtime);
            base.Update(time, dtime);
        }

        private void updateItems(float dtime)
        {
            List<string> remove = new List<string>();
            float y = Size.Y;

            foreach (var itemPair in items)
            {
                ProfilerItem item = itemPair.Value;
                if (item.Active)
                {
                    float dy = (y - item.Size.Y - item.Pos.Y);

                    if (Math.Abs(dy) < 2) item.Pos.Y = y - item.Size.Y;
                    else item.Pos.Y += dy * 10 * dtime;

                    y -= 6 + item.Size.Y;
                }
                else remove.Add(itemPair.Key);
            }

            foreach (string key in remove)
            {
                RemoveChild(items[key]);
                items.Remove(key);
            }

        }

        public override void Resize(float w, float h)
        {
            //updateItems(0.02f);
 	        base.Resize(w, h);
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
