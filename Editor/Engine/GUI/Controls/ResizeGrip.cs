using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{
    public class ResizeGrip : ImageButton
    {
        private Action<Vector2> callbackDragStart;
        private Action callbackDragEnd;
        public bool IsDragged { get; private set; }


        public ResizeGrip(Vector2 size, string image, Action<Vector2> cbDragStart = null, Action cbDragEnd = null)
            : base(size, image)
        {
            IsDragged = false;
            callbackDragStart = cbDragStart;
            callbackDragEnd = cbDragEnd;
        }

        protected override void OnMouseDrag(Vector2 delta)
        {
            IsDragged = true;
            if (callbackDragStart != null) callbackDragStart(delta);
        }

        protected override void OnMouseUp()
        {
            if (IsDragged && callbackDragEnd != null) callbackDragEnd();
            IsDragged = false;
        }
    }
}
