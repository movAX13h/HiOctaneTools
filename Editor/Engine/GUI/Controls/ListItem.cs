using LevelEditor.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{
    public class ListItem : Control
    {
        public object Tag;

        public Color BackgroundColor1;
        public Color BackgroundColor2;
        public Color BackgroundColor3;

        public Color TextColor;

        public Label Label { get; private set; }
        public int ID = 0;

        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }

            set
            {
                if (value == selected) return;
                selected = value;
                if (selected)
                {
                    if (callbackSelected != null) callbackSelected(this);
                    OnSelected();
                }
                else OnDeselected();
            }
        }

        private Action<ListItem> callbackSelected;

        public ListItem(Vector2 size, string text, Action<ListItem> cbSelected = null) : base("listitem", size)
        {
            MouseEnabled = true;
            MouseChildren = false;
            callbackSelected = cbSelected;

            BackgroundColor1 = Color.FromArgb(37, 37, 38);
            BackgroundColor2 = Color.FromArgb(51, 153, 255);
            BackgroundColor3 = Color.FromArgb(63, 63, 70);

            TextColor = Color.White;

            Label = new Label(new Vector2(size.X - 12, size.Y - 4), 9);
            if (Label.Ready)
            {
                Label.Text = text;
                Label.Pos.X = 10;
                Label.Pos.Y = 1;
                AddChild(Label);
            }
            else Ready = false;
        }

        public override void Update(float time, float dtime)
        {
            base.Update(time, dtime);
//            Alpha = Selected ? 1 : 0;
        }


        protected override void ApplyUniforms()
        {
            material.SetUniform("backgroundColor", Selected ? BackgroundColor2 : MouseOver ? BackgroundColor3 : BackgroundColor1);
        }

        protected override void OnMouseUp()
        {
            Selected = true;
        }

        public override void OnMouseWheel(float delta)
        {
            Parent.Parent.OnMouseWheel(delta);

        }

        protected virtual void OnSelected()
        {
            Label.BackgroundColor = BackgroundColor2;
        }

        protected virtual void OnDeselected()
        {
            Label.BackgroundColor = Color.Transparent;
        }

        public override void Resize(float w, float h)
        {
            base.Resize(w, h);


        }
    }
}
