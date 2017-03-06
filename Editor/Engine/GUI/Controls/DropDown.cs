using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using LevelEditor.Engine.Resources;
using OpenTK.Graphics.OpenGL4;

namespace LevelEditor.Engine.GUI.Controls
{
    public class DropDown : Control
    {
        public Color Tint = Color.White;
        private TextureResource Icon;
        private Action<ListItem> callbackSelected;

        private ListBox list;

        public DropDown(Action<ListItem> cbSelected = null) : base("imagebutton", new Vector2(14, 14))
        {
            Icon = EngineBase.Manager.GetTexture("gui/images/dropDown.png");
            MouseEnabled = true;

            list = new ListBox(new Vector2(200, 300), 1, listItemSelected);
            list.Pos.X = 14;
            list.Visible = false;
            AddChild(list);

            callbackSelected = cbSelected;

            layout();
        }

        private void layout()
        {
            list.Pos.Y = -list.Size.Y + 4;
        }

        public override void Resize(float w, float h)
        {
            //base.Resize(w, h);
            //list.Resize(list.Size.X, h);
            layout();
        }


        public ListItem AddItem(string caption, object tag)
        {
            ListItem item = list.AddItem(caption, tag);
            item.BackgroundColor2 = Color.Transparent;
            list.Resize(list.Size.X, list.Items.Count * list.ItemHeight + 2 * list.BorderSize);
            layout();
            return item;
        }

        private void listItemSelected(ListItem item)
        {
            list.Visible = false;
            callbackSelected(item);
            item.Selected = false;
        }

        public override void Update(float time, float dtime)
        {
            base.Update(time, dtime);
        }

        protected override void OnMouseDown()
        {
        }

        protected override void OnMouseUp()
        {
            list.Visible = !list.Visible;
        }


        protected override void ApplyUniforms()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Icon.Id);
            material.SetUniform("icon", 0);
            material.SetUniform("down", MouseDown);
            material.SetUniform("tint", Tint);
        }

    }
}
