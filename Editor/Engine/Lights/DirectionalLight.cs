using System;

using OpenTK;
using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Lights
{
    public class DirectionalLight : LightNode
    {
        public Vector3 Direction;

        public DirectionalLight(Vector3 dir, Vector3 col) : base(Vector3.Zero, col)
        {
            Name = "Directional Light";
            Direction = dir;
        }
    }
}
