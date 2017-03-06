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
using System.Collections.Generic;

namespace LevelEditor.Engine.Models
{
    class BuildingsCursor : Model
    {
        protected int vaoHandle;
        protected int positionVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected int[] indicesVboData;

        public DefaultMaterial material;
        private Vector4 color;

        private bool refreshBuffer = false;
        private int mode = 0;

        public BuildingsCursor(Vector4 col) : base()
        {
            Name = "Buildings Cursor";

            color = col;
            //Scale = new Vector3(0.2f, 0.2f, 0.2f);

            material = new DefaultMaterial();
            if (!material.Ready)
            {
                Log.WriteLine(Log.LOG_ERROR, "failed to load default material");
                return;
            }

            columnGeometry(0, 0, 0, 0);
            createVBOs();
            createVAOs();

            ready = true;
        }


        public void Configure(float a, float b, float c, float d, int m, Vector4 col)
        {
            color = col;
            mode = m;
            if (mode == 0) columnGeometry(a, b, c, d);
            else floorTileGeometry(a, b, c, d);
        }

        private void floorTileGeometry(float oa, float ob, float oc, float od)
        {
            positionVboData = new Vector3[] { new Vector3(0, oa, 0), new Vector3(1, ob, 0), new Vector3(1, oc, 1), new Vector3(0, od, 1) };
            indicesVboData = new int[] { 0, 1, 2, 0, 2, 3 };

            refreshBuffer = true;
        }

        private void columnGeometry(float oa, float ob , float oc, float od)
        {
            List<Vector3> positions = new List<Vector3>();
            List<int> indices = new List<int>();

            int ia = 0, ib = 0, ic = 0, id = 0;

            for (int i = 0; i < 8; i++)
            {
                Vector3 a = new Vector3(0, i + oa, 0);
                Vector3 b = new Vector3(1, i + ob, 0);
                Vector3 c = new Vector3(1, i + oc, 1);
                Vector3 d = new Vector3(0, i + od, 1);

                if (!positions.Contains(a)) positions.Add(a);
                if (!positions.Contains(b)) positions.Add(b);
                if (!positions.Contains(c)) positions.Add(c);
                if (!positions.Contains(d)) positions.Add(d);

                int nia = positions.IndexOf(a);
                int nib = positions.IndexOf(b);
                int nic = positions.IndexOf(c);
                int nid = positions.IndexOf(d);

                indices.Add(nia);
                indices.Add(nib);
                indices.Add(nib);
                indices.Add(nic);
                indices.Add(nic);
                indices.Add(nid);
                indices.Add(nid);
                indices.Add(nia);

                if (i > 0)
                {
                    indices.Add(ia);
                    indices.Add(nia);
                    indices.Add(ib);
                    indices.Add(nib);
                    indices.Add(ic);
                    indices.Add(nic);
                    indices.Add(id);
                    indices.Add(nid);
                }

                ia = nia;
                ib = nib;
                ic = nic;
                id = nid;
            }

            positionVboData = positions.ToArray();
            indicesVboData = indices.ToArray();

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

            GL.LineWidth(1);
            GL.BindVertexArray(vaoHandle);
            if (mode == 0) GL.DrawElements(PrimitiveType.Lines, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            else GL.DrawElements(PrimitiveType.Triangles, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            material.Unbind();
            GL.Enable(EnableCap.DepthTest);
        }

    }
}
