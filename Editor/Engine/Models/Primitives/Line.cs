using System;
using System.Diagnostics;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;
using LevelEditor.Engine.Materials;
using LevelEditor.Utils;
using LevelEditor.Engine.Models.Primitives;
using LevelEditor.Engine;
using LevelEditor.Engine.Lights;

namespace LevelEditor.Engine.Models.Primitives
{
    class Line : Model
    {
        protected int vaoHandle;
        protected int positionVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected int[] indicesVboData;

        public DefaultMaterial material;
        private Vector4 color;

        public Vector3 A { get; private set; }
        public Vector3 B { get; private set; }

        public Line(Vector3 a, Vector3 b, Vector4 col) : base()
        {
            Name = "Line";

            A = a;
            B = b;
            color = col;

            positionVboData = new Vector3[] { a, b };

            indicesVboData = new int[]
            {
                0, 1
            };

            material = new DefaultMaterial();
            if (!material.Ready)
            {
                Log.WriteLine(Log.LOG_ERROR, "failed to load default material");
                return;
            }

            createVBOs();
            createVAOs();

            ready = true;
        }

        public void SetPoints(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
            positionVboData = new Vector3[] { a, b };
            setPositionsBuffer();
        }

        public Line Extend(Vector3 target)
        {
            Line l = new Line(B, target, color);
            AddNode(l);
            return l;
        }

        protected override void createVBOs()
        {
            if (positionVboData == null || indicesVboData == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VBOs and VAOs without positionVboData and indicesVboData for model '" + Name + "'");
                return;
            }

            // position vbo
            GL.GenBuffers(1, out positionVboHandle);
            setPositionsBuffer();

            // indices vbo (ebo)
            GL.GenBuffers(1, out eboHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indicesVboData.Length), indicesVboData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void setPositionsBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(positionVboData.Length * Vector3.SizeInBytes), positionVboData, BufferUsageHint.StaticDraw);
        }

        protected override void createVAOs()
        {
            if (material == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VAOs without material set for model '" + Name + "'");
                return;
            }

            // create vaos
            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);

            // position vao
            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(material.ProgramHandle, 0, "in_position");

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);

            GL.BindVertexArray(0);
        }

        protected override void render()
        {
            material.Bind();

            material.SetUniform("projection_matrix", EngineBase.Renderer.Projection);
            material.SetUniform("modelview_matrix", Matrix4.Mult(transformation, EngineBase.Renderer.View));
            material.SetUniform("color", ref color);

            GL.LineWidth(2);
            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Lines, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            material.Unbind();
        }

    }
}
