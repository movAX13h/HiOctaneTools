using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{
    public class ListBox : Control
    {
        public Color BackgroundColor;
        public Color BorderColor;

        public float BorderSize { get; private set; }

        public int ItemWidth { get; private set; }
        public float ItemHeight { get; private set; }

        private float itemSpace = 0;
        private int numItemsFitting;

        private int scrollPos = 0;

        private ScrollBar scrollBar;
        private Panel list;

        public List<ListItem> Items { get; private set; }
        public ListItem SelectedItem { get; private set; }

        private bool locked = false;

        private Action<ListItem> callbackSelected;

        public ListBox(Vector2 size, float borderSize = 0, Action<ListItem> cbSelected = null) : base("listbox", size)
        {
            callbackSelected = cbSelected;
            //MouseEnabled = true;

            //Alpha = 0.8f;
            ItemHeight = 20;
            BackgroundColor = Color.FromArgb(37, 37, 38);
            BorderColor = Color.FromArgb(63, 63, 70);
            this.BorderSize = borderSize;

            Items = new List<ListItem>();

            list = new Panel(size);
            list.Alpha = 0;
            list.MouseEnabled = true;
            AddChild(list);

            scrollBar = new ScrollBar(new Vector2(18, Size.Y - 2 * borderSize));
            if (AddChild(scrollBar)) layout();
            else Ready = false;

        }

        private void layout()
        {
            ItemWidth = (int)Math.Round(Size.X - 2 * BorderSize - scrollBar.Size.X);

            numItemsFitting = (int)Math.Floor((Size.Y - 2 * BorderSize) / (ItemHeight + itemSpace));

            scrollBar.Pos.X = Size.X - scrollBar.Size.X - BorderSize;
            scrollBar.Pos.Y = BorderSize;

            updateItemPositions();
        }

        public override void Resize(float w, float h)
        {
            base.Resize(w, h);
            scrollBar.Resize(18, Size.Y - 2 * BorderSize);
            list.Resize(w, h);
            layout();
        }


        public void Lock()
        {
            locked = true;
        }

        public void Unlock()
        {
            locked = false;
            updateItemPositions();
        }

        private void itemSelected(ListItem item)
        {
            if (SelectedItem != null) SelectedItem.Selected = false;
            SelectedItem = item;
            if (callbackSelected != null) callbackSelected(item);
        }

        public ListItem AddItem(string caption, object tag = null)
        {
            ListItem item = new ListItem(new Vector2(ItemWidth, ItemHeight), caption, itemSelected);
            item.Tag = tag;
            addItem(item);
            return item;
        }
        /*
        public void AddItem(ListItem item)
        {
            addItem(item);
        }*/

        private void addItem(ListItem item)
        {
            Items.Add(item);
            list.AddChild(item, 0);
            if (!locked) updateItemPositions();
        }

        private void removeItem(ListItem item)
        {
            Items.Remove(item);
            list.RemoveChild(item);
            if (!locked) updateItemPositions();
        }

        /*
        public void SelectItemByTag(object tag)
        {
            foreach (ListItem item in Items)
            {
                if (item.Tag == tag)
                {
                    item.Selected = true;
                    return;
                }
            }
        }
        */

        public void Clear()
        {
            Lock();
            while(Items.Count > 0)
            {
                ListItem item = Items[0];
                removeItem(item);
                item.Unload();
            }
            scrollPos = 0;
            Unlock();
        }

        public override void Update(float time, float dtime)
        {
            base.Update(time, dtime);

            if (scrollBar.ScrollDown)
            {
                scrollPos++;

                if (Items.Count - scrollPos < numItemsFitting) scrollPos--;
                updateItemPositions();
            }

            if (scrollBar.ScrollUp)
            {
                scrollPos--;
                if (scrollPos < 0) scrollPos = 0;
                updateItemPositions();
            }

            if (scrollBar.ScrollPosChanged)
            {
                scrollPos = (int)Math.Round(scrollBar.ScrollPos * (Items.Count - numItemsFitting));
                updateItemPositions(true);
            }
        }

        private void updateItemPositions()
        {
            updateItemPositions(false);
        }

        private void updateItemPositions(bool skipScrollPos)
        {
            int i = 0;
            float y = Size.Y - BorderSize - ItemHeight;
            float dy = ItemHeight + itemSpace;

            if (Items.Count <= numItemsFitting) scrollBar.Visible = false;
            else
            {
                scrollBar.Visible = true;
                if (!skipScrollPos) scrollBar.ScrollPos = (float)scrollPos / (float)(Items.Count - numItemsFitting);
            }

            foreach(ListItem item in Items)
            {
                if (i >= scrollPos && y >= BorderSize)
                {
                    item.Visible = true;
                    item.Pos.X = BorderSize;
                    item.Pos.Y = y;
                    y -= dy;
                }
                else item.Visible = false;

                i++;
            }
        }

        public override void Draw(Vector2 resolution)
        {
            base.Draw(resolution);

        }

        protected override void ApplyUniforms()
        {
            material.SetUniform("backgroundColor", BackgroundColor);
            material.SetUniform("borderColor", BorderColor);
            material.SetUniform("borderSize", BorderSize);
        }

        /*
        private ListItem getItemAt(Vector2 pos)
        {
            if (pos.X > Size.X - scrollBar.Size.X - borderSize) return null;
            float dy = ItemHeight + itemSpace;

            int id = (int)Math.Floor((Size.Y - pos.Y - borderSize + scrollPos*dy) / dy);
            if (id >= 0 && id < Items.Count) return Items[id] as ListItem;
            return null;
        }*/


        #region mouse
        /*
        protected override void OnMouseDown()
        {
            ListItem item = getItemAt(Window.MousePos - WorldPositionOffset() - Pos);
            if (item != null)
            {
                if (SelectedItem != null) SelectedItem.Selected = false;
                item.Selected = true;
                SelectedItem = item;
            }
        }

        protected override void OnMouseUp()
        {

        }*/

        public override void OnMouseWheel(float delta)
        {
            if (scrollBar.Visible) scrollBar.ScrollPos -= 0.1f * delta;
            else scrollBar.ScrollPos = 0;
        }

        #endregion
    }
}
