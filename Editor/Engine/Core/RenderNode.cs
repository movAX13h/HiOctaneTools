using System;

using OpenTK;

namespace LevelEditor.Engine.Core
{
    public class RenderNode : SceneNode
    {
        protected Matrix4 transformation;
        protected Vector3 position;
        protected Vector3 scale;
        protected Vector3 rotation;

        private bool dirtyPosition = true;
        private bool dirtyScale = true;
        private bool dirtyRotation = true;

        public Vector3 Position
        {
            get { return position; }
            set { dirtyPosition = true; position = value; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { dirtyRotation = true; rotation = value; }
        }

        public Vector3 Scale
        {
            get { return scale; }
            set { dirtyScale = true; scale = value; }
        }

        public Matrix4 Transformation
        {
            get { return transformation; }
        }

        public RenderNode()
        {
            position = new Vector3(0, 0, 0);
            rotation = new Vector3(0, 0, 0);
            scale = new Vector3(1, 1, 1);
            transformation = new Matrix4();
        }

        public virtual void Render()
        {
            if (dirtyPosition || dirtyRotation || dirtyScale) transformation = Matrix4.Identity;
            else return;

            if (dirtyScale) transformation *= Matrix4.CreateScale(scale);

            if (dirtyRotation)
            {
                transformation *= Matrix4.CreateRotationX(rotation.X);
                transformation *= Matrix4.CreateRotationY(rotation.Y);
                transformation *= Matrix4.CreateRotationZ(rotation.Z);
            }

            if (dirtyPosition) transformation *= Matrix4.CreateTranslation(position);
        }

    }
}
