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

namespace LevelEditor.Engine.Models
{
    class TerrainCursor : Model
    {
        protected int vaoHandle;
        protected int positionVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected int[] indicesVboData;

        public DefaultMaterial material;
        private Vector4 color;

        private bool refreshBuffer = false;

        private float radius = 1;
        public float Radius
        {
            get
            {
                return radius;
            }

            set
            {
                radius = value;
                updateGeometry();
            }
        }

        public TerrainCursor(Vector4 col)
            : base()
        {
            Name = "Terrain Cursor";

            color = col;
            //Scale = new Vector3(0.2f, 0.2f, 0.2f);

            material = new DefaultMaterial();
            if (!material.Ready)
            {
                Log.WriteLine(Log.LOG_ERROR, "failed to load default material");
                return;
            }

            updateGeometry();
            createVBOs();
            createVAOs();

            ready = true;
        }

        private void updateGeometry()
        {
            positionVboData = new Vector3[] {
                -Vector3.UnitX * radius, Vector3.UnitX * radius,
                -Vector3.UnitZ * radius, Vector3.UnitZ * radius
            };

            indicesVboData = new int[] { 0, 1, 2, 3 };
            refreshBuffer = true;
        }

        protected override void createVBOs()
        {
            if (positionVboData == null || indicesVboData == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VBOs and VAOs without positionVboData and indicesVboData for model '" + Name + "'");
                return;
            }

            GL.GenBuffers(1, out positionVboHandle); // position vbo
            GL.GenBuffers(1, out eboHandle); // indices vbo (ebo)

            setBufferData();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void setBufferData()
        {
            refreshBuffer = false;
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(positionVboData.Length * Vector3.SizeInBytes), positionVboData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indicesVboData.Length), indicesVboData, BufferUsageHint.StaticDraw);
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

        public override void Update(float time)
        {
            if (refreshBuffer) setBufferData();
            base.Update(time);
        }

        protected override void render()
        {
            GL.Disable(EnableCap.DepthTest);
            material.Bind();

            material.SetUniform("projection_matrix", EngineBase.Renderer.Projection);
            material.SetUniform("modelview_matrix", Matrix4.Mult(transformation, EngineBase.Renderer.View));
            material.SetUniform("color", ref color);

            GL.LineWidth(3);
            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Lines, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            material.Unbind();
            GL.Enable(EnableCap.DepthTest);
        }

    }
}
