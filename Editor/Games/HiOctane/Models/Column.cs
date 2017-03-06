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
using LevelEditor.Engine.Resources;
using LevelEditor.Games.HiOctane.Materials;
using LevelEditor.Games.HiOctane.Resources;


namespace LevelEditor.Games.HiOctane.Models
{
    public class Column : Model
    {
        protected int vaoHandle;
        protected int positionVboHandle;
        protected int normalVboHandle;
        private int uvVboHandle;
        protected int eboHandle;

        protected int[] indicesVboData;
        protected Vector3[] positionVboData;
        protected Vector3[] normalVboData;
        private Vector2[] uvVboData;

        public Vector3[] OriginalVertices { get; private set; } // never modified, used as reference for morphs
        public Column MorphSource = null;

        public AtlasMaterial material;
        public DirectionalLight light;
        public Vector4 TintColor;
        public float Opacity = 1;
        public float Grayscale = 0;
        public bool DestroyOnMorph = false;

        private bool hidden = false;

        private LevelFile levelRes;

        private float segmentSize = 1.0f;
        private float Time = 0.0f;

        public ColumnDefinition Definition { get; private set; }

        public Column(ColumnDefinition def, Vector3 pos, LevelFile levelRes, AtlasMaterial material)
            : base()
        {
            Name = "Column at " + pos.X + "/" + pos.Z;
            this.levelRes = levelRes;
            Definition = def;
            Position = pos;

            TintColor = new Vector4(1.0f, 0.0f, 0.0f, 0.0f); // alpha is intensity

            this.material = material;
            if (!material.Ready) return;

            if (setupGeometry())
            {
                Log.WriteLine("HiOctane '" + Name + "' loaded " +
                    positionVboData.Length + " vertices, " +
                    normalVboData.Length + " normals, " +
                    uvVboData.Length + " UVs, " +
                    indicesVboData.Length + " indices");

                createVBOs();
                createVAOs();

                ready = true;
            }
            else Log.WriteLine(Log.LOG_ERROR, "failed setting up game model '" + Name + "'");
        }

        public void Morph(float progress)
        {
            if (DestroyOnMorph)
            {
                hidden = progress > 0.01f;
                return;
            }

            if (MorphSource == null)
            {
                UpdateGeometry();
                return;
            }

            if (MorphSource.OriginalVertices.Length != OriginalVertices.Length) throw new Exception("Column morph - vertice count mismatch");

            for(int i = 0; i < OriginalVertices.Length; i++)
            {
                Vector3 source = MorphSource.OriginalVertices[i];
                Vector3 home = OriginalVertices[i];
                float h = home.Y * (1f - progress) + source.Y * progress;
                positionVboData[i] = new Vector3(home.X, h, home.Z);
            }

            setPositionsBuffer();
        }

        public void UpdateGeometry() // adapts to terrain height
        {
            if (setupGeometry(true)) setPositionsBuffer();
        }

        #region Creation
        private MapEntry getMapEntry(int x, int y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > levelRes.Width - 1) x = levelRes.Width - 1;
            if (y > levelRes.Height - 1) y = levelRes.Height - 1;

            return levelRes.Map[x, y];
        }

        private bool setupGeometry(bool positionsOnly = false)
        {
            int x, z, i = 0;

            // generate vertices
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();

            List<int> indices = new List<int>();

            x = (int)Position.X;
            z = (int)Position.Z;

            // 4 corners
            float a = getMapEntry(x, z).Height;
            float b = getMapEntry(x + 1, z).Height;
            float c = getMapEntry(x + 1, z + 1).Height;
            float d = getMapEntry(x, z + 1).Height;

            float h = 0f;

            for (int bitNum = 0; bitNum < 8; bitNum++) // all blocks of this column
            {
                if ((Definition.Shape & (1 << bitNum)) == 0) continue;

                int blockDefId = BitConverter.ToInt16(Definition.Bytes, 6 + bitNum * 2);
                BlockDefinition blockDef = levelRes.BlockDefinitions.GetValue(blockDefId);

                // bottom vertices
                h = bitNum * segmentSize;
                Vector3 A = new Vector3(0f, a + h, 0f);
                Vector3 B = new Vector3(segmentSize, b + h, 0f);
                Vector3 C = new Vector3(segmentSize, c + h, segmentSize);
                Vector3 D = new Vector3(0f, d + h, segmentSize);

                // top vertices
                h += segmentSize;
                Vector3 E = new Vector3(0f, a + h, 0f);
                Vector3 F = new Vector3(segmentSize, b + h, 0f);
                Vector3 G = new Vector3(segmentSize, c + h, segmentSize);
                Vector3 H = new Vector3(0f, d + h, segmentSize);

                // faces
                vertices.Add(F); vertices.Add(E); vertices.Add(A); vertices.Add(B); // north
                vertices.Add(G); vertices.Add(F); vertices.Add(B); vertices.Add(C); // east
                vertices.Add(H); vertices.Add(G); vertices.Add(C); vertices.Add(D); // south
                vertices.Add(E); vertices.Add(H); vertices.Add(D); vertices.Add(A); // west
                vertices.Add(E); vertices.Add(F); vertices.Add(G); vertices.Add(H); // top
                vertices.Add(D); vertices.Add(C); vertices.Add(B); vertices.Add(A); // bottom

                if (!positionsOnly)
                {
                    // texture atlas UVs
                    uvs.AddRange(material.MakeUVs(blockDef.N, blockDef.NMod));
                    uvs.AddRange(material.MakeUVs(blockDef.E, blockDef.EMod));
                    uvs.AddRange(material.MakeUVs(blockDef.S, blockDef.SMod));
                    uvs.AddRange(material.MakeUVs(blockDef.W, blockDef.WMod));
                    uvs.AddRange(material.MakeUVs(blockDef.T, blockDef.TMod));
                    uvs.AddRange(material.MakeUVs(blockDef.B, blockDef.BMod));

                    // normals
                    Vector3 nN = new Vector3(0f, 0f, -1f);
                    Vector3 nE = new Vector3(1f, 0f, 0f);
                    Vector3 nS = new Vector3(0f, 0f, 1f);
                    Vector3 nW = new Vector3(-1f, 0f, 0f);
                    Vector3 nT = new Vector3(0f, 1f, 0f);
                    Vector3 nB = new Vector3(0f, -1f, 0f);

                    normals.Add(nN); normals.Add(nN); normals.Add(nN); normals.Add(nN);
                    normals.Add(nE); normals.Add(nE); normals.Add(nE); normals.Add(nE);
                    normals.Add(nS); normals.Add(nS); normals.Add(nS); normals.Add(nS);
                    normals.Add(nW); normals.Add(nW); normals.Add(nW); normals.Add(nW);
                    normals.Add(nT); normals.Add(nT); normals.Add(nT); normals.Add(nT);
                    normals.Add(nB); normals.Add(nB); normals.Add(nB); normals.Add(nB);

                    //Vector3 p = new Vector3(x, A.Y, z);
                    //AddNode(new Line(p, p + nN, new Vector4(1, 0, 0, 1)));

                    // indices for 2 tris
                    for (int j = 0; j < 6; j++)
                    {
                        indices.Add(i);
                        indices.Add(i + 1);
                        indices.Add(i + 3);

                        indices.Add(i + 1);
                        indices.Add(i + 2);
                        indices.Add(i + 3);
                        i += 4;
                    }
                }
            }

            positionVboData = vertices.ToArray();
            OriginalVertices = vertices.ToArray();

            if (!positionsOnly)
            {
                normalVboData = normals.ToArray();
                indicesVboData = indices.ToArray();
                uvVboData = uvs.ToArray();
            }

            Size = new Vector3(segmentSize, h, segmentSize);

            return true;
        }

        protected override void createVBOs()
        {
            if (positionVboData == null || normalVboData == null || indicesVboData == null || uvVboData == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VBOs without position/normal/indices/uvs VBO data for model '" + Name + "'");
                return;
            }

            GL.GenBuffers(1, out positionVboHandle);
            setPositionsBuffer();

            GL.GenBuffers(1, out normalVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(normalVboData.Length * Vector3.SizeInBytes), normalVboData, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out uvVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvVboHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(uvVboData.Length * Vector2.SizeInBytes), uvVboData, BufferUsageHint.StaticDraw);

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

            GL.GenVertexArrays(1, out vaoHandle);
            GL.BindVertexArray(vaoHandle);

            int index = -1;

            index++;
            GL.EnableVertexAttribArray(index);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
            GL.BindAttribLocation(material.ProgramHandle, index, "in_position");

            if (normalVboData.Length > 0)
            {
                index++;
                GL.EnableVertexAttribArray(index);
                GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
                GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
                GL.BindAttribLocation(material.ProgramHandle, index, "in_normal");
            }

            if (material.UseTexture)
            {
                index++;
                GL.EnableVertexAttribArray(index);
                GL.BindBuffer(BufferTarget.ArrayBuffer, uvVboHandle);
                GL.VertexAttribPointer(index, 2, VertexAttribPointerType.Float, true, Vector2.SizeInBytes, 0);
                GL.BindAttribLocation(material.ProgramHandle, index, "in_texcoord");
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle); // don't know why this is here...
            GL.BindVertexArray(0);
        }
        #endregion

        #region Rendering
        protected override void render()
        {
            if (material == null || hidden) return;

            material.Bind();

            if (material.UseTexture)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, material.TextureHandle);
                material.SetUniform("colorMap", 0);
            }

            if (material.ReceiveShadows && EngineBase.Renderer.ShadowFrameBuffer.TextureHandle > 0)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, EngineBase.Renderer.ShadowFrameBuffer.TextureHandle);
                material.SetUniform("shadowMap", 0);
                material.SetUniform("shadowViewProjection", Matrix4.Mult(shadowViewProjection, EngineBase.Renderer.ShadowMapBias));
            }

            Matrix4 mv = Matrix4.Mult(transformation, EngineBase.Renderer.View);
            //Matrix4 nm = Matrix4.Invert(mv);
            //nm.Transpose();
            //material.SetUniform("normalMatrix", nm);
            material.SetUniform("model", transformation);
            material.SetUniform("modelView", mv);
            material.SetUniform("modelViewProjection", Matrix4.Mult(mv, EngineBase.Renderer.Projection));

            if (light == null) light = (DirectionalLight)EngineBase.Scene.GetLight(0);
            material.SetUniform("lightDirection", light.Direction);

            material.SetUniform("color", ref TintColor);
            material.SetUniform("opacity", Opacity);
            material.SetUniform("grayscale", Grayscale);
            material.SetUniform("time", Time);

            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Triangles, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            material.Unbind();
        }

        public override void Update(float time)
        {
            Time += time;
            base.Update(time);
        }

        /*
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
        }*/
        #endregion
    }
}
