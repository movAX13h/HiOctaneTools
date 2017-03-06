using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using LevelEditor.Engine.Core;


namespace LevelEditor.Engine.GUI
{
    public abstract class Sprite
    {
        public string Name = "Sprite";

        public Vector2 Pos = Vector2.Zero;
        public float Alpha = 1.0f;

        protected int positionVboHandle;
        protected int vaoHandle;
        protected int eboHandle;

        protected Vector3[] vertices;
        protected int[] indices;

        public Shader Shader; // the shader is attached by the Scene class in AddSprite()
        public Vector2 Size { get { return size; } }
        private Vector2 size;

        public bool SnapToPixel = true;
        public bool IsSolid = true; // only solids are checked for collision detection
        private float Deg2Rad = (float)Math.PI / 180f;

        public Sprite(Vector2 size)
        {
            this.size = size;
            setup();
        }

        private void setup() // load geometry and GL buffers
        {
            // load GL stuff
            loadGeometry();

            // create vbo
            GL.GenBuffers(1, out positionVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out eboHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indices.Length), indices, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            // create vaos
            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            //GL.BindAttribLocation(shader.ProgramHandle, 0, "in_pos"); // I don't really understand why everything is still working with this removed

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);

            GL.BindVertexArray(0);
        }

        public virtual void Resize(float w, float h)
        {
            size.X = w;
            size.Y = h;

            loadGeometry();
            updateGeometry();
        }

        public virtual void Rotate(float degrees)
        {
            float sin = (float)Math.Sin(degrees * Deg2Rad);
            float cos = (float)Math.Cos(degrees * Deg2Rad);

            //Matrix4 m = Matrix4.CreateRotationZ(degrees * Deg2Rad);

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 v = vertices[i];
                float tx = v.X;
                float ty = v.Y;
                v.X = (cos * tx) - (sin * ty);
                v.Y = (sin * tx) + (cos * ty);
                vertices[i] = v;
            }

            updateGeometry();
        }

        public virtual void Unload()
        {
            //TODO: check if this is enough:
            GL.DeleteBuffers(1, ref positionVboHandle);
            GL.DeleteBuffers(1, ref eboHandle);
            GL.DeleteVertexArrays(1, ref vaoHandle);
        }

        private void updateGeometry()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);
        }

        protected virtual void loadGeometry()
        {
            vertices = new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f), new Vector3(size.X, 0.0f, 0.0f), new Vector3(size.X, size.Y, 0.0f), new Vector3(0.0f, size.Y, 0.0f) };
            indices = new int[] { 0, 1, 2, 0, 2, 3 };
        }

        public abstract void Update(float time, float dtime);


        public virtual void Render(Shader shader)
        {
            if (SnapToPixel)
            {
                Vector2 pos = new Vector2((float)Math.Round(Pos.X), (float)Math.Round(Pos.Y));
                shader.SetUniform("translation", pos);
            }
            else shader.SetUniform("translation", Pos);

            shader.SetUniform("alpha", Alpha);

            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }

        public bool HitTest(Vector2 pos)
        {
            return pos.X > Pos.X && pos.X < Pos.X + Size.X && pos.Y > Pos.Y && pos.Y < Pos.Y + Size.Y;
        }

    }
}
