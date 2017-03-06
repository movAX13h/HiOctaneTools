using System;
using OpenTK;
using LevelEditor.Engine.Models.Primitives;

namespace LevelEditor.Engine.Core
{
    public class LightNode : RenderNode
    {
        protected Vector3 color;

        public Vector3 Color
        {
            get { return color; }
            set { color = value; }
        }

        public LightNode(Vector3 pos, Vector3 col) : base()
        {
            position = pos;
            color = col;
        }
    }
}
