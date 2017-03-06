using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using LevelEditor.Engine.Resources;
using System.Drawing;

namespace LevelEditor.Engine.GUI.Controls
{
    public class Menu : Panel
    {
        private List<MenuItem> items;
        private MenuItem currentItem;
        private ImageButton icon = null;

        public Menu(Vector2 size, string iconFile = ""):base(size)
        {
            MouseEnabled = true;

            BorderColor = Color.FromArgb(63, 63, 70);
            BorderSize = 1;

            Ready = false;
            items = new List<MenuItem>();

            if (iconFile != "")
            {
                icon = new ImageButton(new Vector2(32, 32), iconFile);
                if (icon.Ready)
                {
                    icon.Pos.X = 2;
                    icon.Pos.Y = 0;
                    AddChild(icon);
                }
                else icon = null;
            }

            Ready = true;
        }

        public override void Update(float time, float dtime)
        {
            icon.Tint = Color.FromArgb(
                (int)(100f + 155f * 0.5f * (Math.Sin(time * 0.3f) + 1)),
                (int)(100f + 155f * 0.5f * (Math.Sin(-time * 0.73f) + 1)),
                (int)(100f + 155f * 0.5f * (Math.Sin(time) + 1)));

            base.Update(time, dtime);
        }

        protected override void OnMouseDrag(Vector2 delta)
        {
            Window.Drag(delta);
        }

        public MenuItem AddItem(string caption)
        {
            MenuItem item = new MenuItem(caption, new Vector2(100, 26), itemClicked, true, true);
            float x = icon == null ? 2 : icon.Pos.X + icon.Size.X + 4;
            if (items.Count > 0) x = (float)Math.Floor(items[items.Count - 1].Pos.X + items[items.Count - 1].Size.X);
            item.Pos.X = x;
            item.Pos.Y = 2;
            items.Add(item);
            AddChild(item);
            return item;
        }

        public void Close()
        {
            if (currentItem != null) currentItem.Close();
            currentItem = null;
        }

        private void itemClicked(MenuItem item)
        {
            if (item == currentItem)
            {
                if (item.IsOpen) item.Close();
                else item.Open();
            }
            else
            {
                if (currentItem != null) currentItem.Close();
                currentItem = item;
                item.Open();
            }
        }

    }
}
