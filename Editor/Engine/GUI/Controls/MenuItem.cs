using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{
    public class MenuItem : Button
    {
        private Label label;
        public Action<MenuItem> Callback;
        public object Tag { get; private set; }

        private Dictionary<MenuItem,Action<MenuItem>> subItems;
        private List<Panel> separators;

        private bool open = false;
        public bool IsOpen { get { return open; } }
        public string Caption { get { return label.Text; } }

        public MenuItem(string caption, Vector2 size, Action<MenuItem> callback, bool centerText = false, bool autoSize = false) : base(size)
        {
            Ready = false;
            Callback = callback;
            SnapToPixel = true;

            subItems = new Dictionary<MenuItem, Action<MenuItem>>();
            separators = new List<Panel>();

            label = new Label(size, 9);
            if (!label.Ready) return;

            label.Text = caption;
            if (autoSize)
            {
                centerText = true;
                Resize(label.TextSize.Width + 20, size.Y); // updates Size
            }
            label.Pos.X = centerText ? 0.5f * (Size.X - label.TextSize.Width) : 20;
            label.Pos.Y = -6;
            AddChild(label);

            BorderSize = 4;
            //BackgroundColor1 = Color.FromArgb(63, 63, 70);
            //BackgroundColor2 = Color.FromArgb(30, 30, 30);
            BorderColor = Color.FromArgb(45, 45, 48);

            Ready = true;


        }

        public MenuItem AddItem(string caption, Action<MenuItem> cb, object tag = null)
        {
            MenuItem item = new MenuItem(caption, new Vector2(200, 26), itemClicked);
            item.BorderSize = 1;
            item.Tag = tag;
            item.Pos.X = 2;
            item.Pos.Y = -25 * subItems.Count - 2 * separators.Count - 26;
            item.Visible = open;
            subItems.Add(item, cb);
            item.Callback = itemClicked;
            AddChild(item);
            return item;
        }

        public void AddSeparator()
        {
            Panel separator = new Panel(new Vector2(200, 2));
            separator.Visible = false;
            separator.Pos.X = 2;
            separator.Pos.Y = -25 * subItems.Count - separators.Count - 3;
            separator.BackgroundColor = Color.FromArgb(140, 140, 140);
            AddChild(separator);
            separators.Add(separator);
        }

        private void itemClicked(MenuItem item)
        {
            if (subItems[item] != null) subItems[item](item);
            Close();
        }

        public void Open()
        {
            open = true;
            foreach (var item in subItems)
            {
                item.Key.Visible = true;
            }

            foreach (Panel separator in separators) separator.Visible = true;
        }
        public void Close()
        {
            open = false;
            foreach (var item in subItems)
            {
                item.Key.Visible = false;
            }

            foreach (Panel separator in separators) separator.Visible = false;
        }

        protected override void OnMouseUp()
        {
            if (Callback != null) Callback(this);
        }


    }
}
