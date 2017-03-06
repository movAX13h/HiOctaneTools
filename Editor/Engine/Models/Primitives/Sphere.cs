using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Collections.Generic;

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
    class Sphere : Model
    {
        protected int vaoHandle;
        protected int positionVboHandle;
        protected int normalVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected Vector3[] normalVboData;
        protected int[] indicesVboData;

        public LightMaterial material;
        public DirectionalLight light;
        private Vector4 color;

        public float Radius { get; private set; }
        public int Detail { get; private set; }

        public Sphere(string name, float radius, int detail) : base()
        {
            Name = name;
            Detail = detail;

            Radius = radius;
            color = new Vector4(0.3f, 0.1f, 0.9f, 1.0f);

            if (setupGeometry())
            {
                Log.WriteLine("Sphere '" + name + "' loaded " +
                    positionVboData.Length + " vertices, " +
                    normalVboData.Length + " normals, " +
                    indicesVboData.Length + " indices");

                material = new LightMaterial();
                if (!material.Ready) return;

                createVBOs();
                createVAOs();

                ready = true;
            }
            else Log.WriteLine(Log.LOG_ERROR, "failed setting up environment model '" + Name + "'");
        }



        #region Creation

        private List<Vector3> vertices = new List<Vector3>();
        private List<Vector3> normals = new List<Vector3>();
        private List<int> indices = new List<int>();

        private int index;
        private Dictionary<Int64, int> middlePointIndexCache = new Dictionary<long, int>();

        private bool setupGeometry()
        {
            this.index = 0;

            // create 12 vertices of a icosahedron
            float t = (1.0f + (float)Math.Sqrt(5.0f)) / 2.0f;

            addVertex(new Vector3(-1f, t, 0f));
            addVertex(new Vector3(1f, t, 0f));
            addVertex(new Vector3(-1f, -t, 0f));
            addVertex(new Vector3(1f, -t, 0f));

            addVertex(new Vector3(0f, -1f, t));
            addVertex(new Vector3(0f, 1f, t));
            addVertex(new Vector3(0f, -1f, -t));
            addVertex(new Vector3(0f, 1f, -t));

            addVertex(new Vector3(t, 0f, -1f));
            addVertex(new Vector3(t, 0f, 1f));
            addVertex(new Vector3(-t, 0f, -1f));
            addVertex(new Vector3(-t, 0f, 1f));


            // create 20 triangles of the icosahedron
            var faces = new List<TriangleIndices>();

            // 5 faces around point 0
            faces.Add(new TriangleIndices(0, 11, 5));
            faces.Add(new TriangleIndices(0, 5, 1));
            faces.Add(new TriangleIndices(0, 1, 7));
            faces.Add(new TriangleIndices(0, 7, 10));
            faces.Add(new TriangleIndices(0, 10, 11));

            // 5 adjacent faces
            faces.Add(new TriangleIndices(1, 5, 9));
            faces.Add(new TriangleIndices(5, 11, 4));
            faces.Add(new TriangleIndices(11, 10, 2));
            faces.Add(new TriangleIndices(10, 7, 6));
            faces.Add(new TriangleIndices(7, 1, 8));

            // 5 faces around point 3
            faces.Add(new TriangleIndices(3, 9, 4));
            faces.Add(new TriangleIndices(3, 4, 2));
            faces.Add(new TriangleIndices(3, 2, 6));
            faces.Add(new TriangleIndices(3, 6, 8));
            faces.Add(new TriangleIndices(3, 8, 9));

            // 5 adjacent faces
            faces.Add(new TriangleIndices(4, 9, 5));
            faces.Add(new TriangleIndices(2, 4, 11));
            faces.Add(new TriangleIndices(6, 2, 10));
            faces.Add(new TriangleIndices(8, 6, 7));
            faces.Add(new TriangleIndices(9, 8, 1));

            // refine triangles
            for (int i = 0; i < Detail; i++)
            {
                var faces2 = new List<TriangleIndices>();
                foreach (var tri in faces)
                {
                    // replace triangle by 4 triangles
                    int a = getMiddlePoint(tri.v1, tri.v2);
                    int b = getMiddlePoint(tri.v2, tri.v3);
                    int c = getMiddlePoint(tri.v3, tri.v1);

                    faces2.Add(new TriangleIndices(tri.v1, a, c));
                    faces2.Add(new TriangleIndices(tri.v2, b, a));
                    faces2.Add(new TriangleIndices(tri.v3, c, b));
                    faces2.Add(new TriangleIndices(a, b, c));
                }
                faces = faces2;
            }

            // done, now add triangles to mesh
            foreach (var tri in faces)
            {
                indices.Add(tri.v1);
                indices.Add(tri.v2);
                indices.Add(tri.v3);
            }

            positionVboData = vertices.ToArray();
            normalVboData = normals.ToArray();
            indicesVboData = indices.ToArray();

            return true;
        }

        // add vertex to mesh, fix position to be on unit sphere, return index
        private int addVertex(Vector3 p)
        {
            Vector3 n = p.Normalized();
            vertices.Add(Radius * n);
            normals.Add(n);
            return index++;
        }

        // return index of point in the middle of p1 and p2
        private int getMiddlePoint(int p1, int p2)
        {
            // first check if we have it already
            bool firstIsSmaller = p1 < p2;
            Int64 smallerIndex = firstIsSmaller ? p1 : p2;
            Int64 greaterIndex = firstIsSmaller ? p2 : p1;
            Int64 key = (smallerIndex << 32) + greaterIndex;
            int ret;
            if (this.middlePointIndexCache.TryGetValue(key, out ret))
            {
                return ret;
            }

            // not in cache, calculate it
            Vector3 point1 = vertices[p1];
            Vector3 point2 = vertices[p2];
            Vector3 middle = new Vector3(
                (point1.X + point2.X) / 2.0f,
                (point1.Y + point2.Y) / 2.0f,
                (point1.Z + point2.Z) / 2.0f);

            // add vertex makes sure point is on unit sphere
            int i = addVertex(middle);

            // store it, return index
            this.middlePointIndexCache.Add(key, i);
            return i;
        }


        protected override void createVBOs()
        {
            if (positionVboData == null || normalVboData == null || indicesVboData == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VBOs without position/normal/indices VBO data for model '" + Name + "'");
                return;
            }

            GL.GenBuffers(1, out positionVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(positionVboData.Length * Vector3.SizeInBytes), positionVboData, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out normalVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(normalVboData.Length * Vector3.SizeInBytes), normalVboData, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out eboHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indicesVboData.Length), indicesVboData, BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        protected override void createVAOs()
        {
            if (material == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VAOs without material set for model '" + Name + "'");
                return;
            }

            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(material.ProgramHandle, 0, "in_position");

            GL.EnableVertexAttribArray(1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(material.ProgramHandle, 1, "in_normal");

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);

            GL.BindVertexArray(0);
        }
        #endregion

        #region Rendering
        protected override void render()
        {
            if (material == null) return;

            material.Bind();

            if (EngineBase.Renderer.ShadowFrameBuffer.TextureHandle > 0)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, EngineBase.Renderer.ShadowFrameBuffer.TextureHandle);
                material.SetUniform("shadowMap", 0);
                material.SetUniform("shadowViewProjection", Matrix4.Mult(shadowViewProjection, EngineBase.Renderer.ShadowMapBias));
            }

            Matrix4 mv = Matrix4.Mult(transformation, EngineBase.Renderer.View);
            Matrix4 nm = Matrix4.Invert(mv);
            nm.Transpose();
            material.SetUniform("normalMatrix", nm);
            material.SetUniform("model", transformation);
            material.SetUniform("modelView", mv);
            material.SetUniform("modelViewProjection", Matrix4.Mult(mv, EngineBase.Renderer.Projection));

            if (light == null) light = (DirectionalLight)EngineBase.Scene.GetLight(0);
            material.SetUniform("lightDirection", light.Direction);

            material.SetUniform("color", ref color);
            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Triangles, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            material.Unbind();
        }


        protected override void renderShadow()
        {
            Matrix4 mv = Matrix4.Mult(transformation, EngineBase.Renderer.View);
            EngineBase.Scene.ShadowMaterial.SetUniform("model", transformation);
            EngineBase.Scene.ShadowMaterial.SetUniform("modelView", mv);

            shadowViewProjection = Matrix4.Mult(mv, EngineBase.Renderer.ShadowProjection);
            EngineBase.Scene.ShadowMaterial.SetUniform("modelViewProjection", shadowViewProjection);

            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Triangles, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }
        #endregion
    }
}
