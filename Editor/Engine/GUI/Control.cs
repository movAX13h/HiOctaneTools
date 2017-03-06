using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.GUI
{
    public abstract class Control : Sprite
    {
        public bool Ready { get; protected set; }
        protected ControlMaterial material;

        protected List<Control> Children;
        public Control Parent { get; protected set; }

        public bool Visible = true;

        public bool MouseEnabled = false;
        public bool MouseChildren = true;

        public Vector2 MousePosLocal { get; private set; }
        public bool MouseConsumed { get; protected set; }
        public bool MouseDown { get; private set; }
        public bool MouseOver { get; private set; }


        private Vector2 mousePos;
        private bool dragging = false;

        public Control(string shaderName, Vector2 size) : base(size)
        {
            material = new ControlMaterial(shaderName);
            Children = new List<Control>();

            MouseDown = false;

            if (material != null && material.Ready) Ready = true;
        }

        #region graph
        public bool AddChild(Control control)
        {
            return AddChild(control, Children.Count);
        }

        public bool AddChild(Control control, int index)
        {
            if (!control.Ready) return false;

            Children.Insert(index, control);
            if (control.Parent != null) control.Parent.RemoveChild(control);
            control.Parent = this;

            return true;
        }

        public void RemoveChild(Control control)
        {
            if (Children.Contains(control))
            {
                Children.Remove(control);
                control.Parent = null;
            }
        }

        public Vector2 WorldPositionOffset()
        {
            Vector2 offset = Vector2.Zero;
            Control p = Parent;
            while (p != null)
            {
                offset += p.Pos;
                p = p.Parent;
            }

            return offset;
        }
        #endregion

        public override void Update(float time, float dtime)
        {
            if (!Visible) return;
            foreach (Control control in Children)
            {
                control.Update(time, dtime);
            }
        }

        protected abstract void ApplyUniforms();

        public virtual void Draw(Vector2 resolution)
        {
            if (!Visible) return;

            material.Bind();
            material.SetUniform("resolution", resolution);
            material.SetUniform("size", Size);
            material.SetUniform("offset", WorldPositionOffset());

            ApplyUniforms();

            base.Render(material);
            material.Unbind();

            foreach (Control element in Children)
            {
                if (element.Visible) element.Draw(resolution);
            }
        }

        #region mouse
        protected virtual void OnMouseDown()
        {}

        protected virtual void OnMouseUp()
        {}

        protected virtual void OnMouseDrag(Vector2 delta)
        {}

        public virtual void OnMouseWheel(float delta)
        {}

        public void ProcessMouse()
        {
            MouseConsumed = false;

            if (!Visible)
            {
                MouseDown = false;
                MouseOver = false;
                return;
            }

            if (MouseChildren)
            {
                for (int i = Children.Count - 1; i >= 0; i--)
                {
                    Control control = Children[i];
                    control.ProcessMouse();
                    if (control.MouseConsumed)
                    {
                        MouseConsumed = true;
                        if (!MouseEnabled) break; // allow children on same level to update their state
                    }
                }
            }

            if (MouseEnabled && !MouseConsumed) checkMouse();
        }

        private void checkMouse()
        {
            MousePosLocal = Window.MousePos - WorldPositionOffset();
            MouseOver = HitTest(MousePosLocal);

            // mouse dragging
            if (MouseDown)
            {
                Vector2 delta = Window.MousePos - mousePos;
                if (delta.Length > 0.0f)
                {
                    dragging = true;
                    OnMouseDrag(delta);
                }
            }

            MouseConsumed = MouseOver || dragging;

            // mouse up / down
            if (!Window.MouseLeftDown && MouseDown)
            {
                MouseDown = false;
                OnMouseUp();
                dragging = false;
            }
            else if (Window.MouseLeftDown && MouseOver && !MouseDown)
            {
                MouseDown = true;
                OnMouseDown();
            }

            // mouse wheel
            if (Window.MouseWheelDelta != 0 && MouseOver)
            {
                OnMouseWheel(Window.MouseWheelDelta);
            }

            mousePos = Window.MousePos;
        }

        #endregion
    }
}
