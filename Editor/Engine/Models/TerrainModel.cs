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

// some code from http://gamedev.stackexchange.com/questions/24572/how-does-terrain-following-work-on-height-map

namespace LevelEditor.Engine.Models
{
    class TerrainModel : Model
    {
        protected int vaoHandle;
        protected int positionVboHandle;
        protected int normalVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected Vector3[] normalVboData;
        protected int[] indicesVboData;

        public TerrainMaterial material;
        public DirectionalLight light;
        private Vector4 color;

        private HeightmapResource heightmap;
        private float[,] heightsArray;
        //private Vector3[,] normalsArray;

        private float segmentSize = 2.0f;
        private float maxHeight = 1.0f;

        private float Time = 0.0f;

        //public Vector3 Size { get { return size; } }
        //private Vector3 size;

        public float Far { set { material.Far = value; } get { return material.Far; } }

        public TerrainModel(string name, HeightmapResource heightmap) : base()
        {
            Name = name;
            this.heightmap = heightmap;

            color = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);

            if (setupGeometry())
            {
                Log.WriteLine("Terrain '" + name + "' loaded " +
                    positionVboData.Length + " vertices, " +
                    normalVboData.Length + " normals, " +
                    indicesVboData.Length + " indices");

                material = new TerrainMaterial();
                if (!material.Ready) return;

                createVBOs();
                createVAOs();

                ready = true;
            }
            else Log.WriteLine(Log.LOG_ERROR, "failed setting up environment model '" + Name + "'");
        }

        public float GetHeight(float x, float z)
        {
            // translate from world to grid coords
            int ix = (int)Math.Round(x / segmentSize);
            int iz = (int)Math.Round(z / segmentSize);
            // check bounds
            if (ix < 0) ix = 0;
            if (iz < 0) iz = 0;
            if (ix >= heightmap.Width - 1) ix = heightmap.Width - 1;
            if (iz >= heightmap.Height - 1) iz = heightmap.Height - 1;

            // determine exact height
            int xPlusOne = ix + 1;
            int zPlusOne = iz + 1;

            float triZ0 = (heightsArray[ix, iz]);
            float triZ1 = (heightsArray[xPlusOne, iz]);
            float triZ2 = (heightsArray[ix, zPlusOne]);
            float triZ3 = (heightsArray[xPlusOne, zPlusOne]);

            float height = 0.0f;
            float sqX = (x / segmentSize) - ix;
            float sqZ = (z / segmentSize) - iz;
            if ((sqX + sqZ) < 1)
            {
                height = triZ0;
                height += (triZ1 - triZ0) * sqX;
                height += (triZ2 - triZ0) * sqZ;
            }
            else
            {
                height = triZ3;
                height += (triZ1 - triZ3) * (1.0f - sqZ);
                height += (triZ2 - triZ3) * (1.0f - sqX);
            }
            return height;
        }

        /*
        public Vector3 GetNormal(float xPos, float zPos, float scaleFactor)
        {
            int x = (int)(xPos / scaleFactor);
            int z = (int)(zPos / scaleFactor);

            int xPlusOne = x + 1;
            int zPlusOne = z + 1;

            Vector3 triZ0 = (this.normals[x, z]);
            Vector3 triZ1 = (this.normals[xPlusOne, z]);
            Vector3 triZ2 = (this.normals[x, zPlusOne]);
            Vector3 triZ3 = (this.normals[xPlusOne, zPlusOne]);

            Vector3 avgNormal;
            float sqX = (xPos / scaleFactor) - x;
            float sqZ = (zPos / scaleFactor) - z;
            if ((sqX + sqZ) < 1)
            {
                avgNormal = triZ0;
                avgNormal += (triZ1 - triZ0) * sqX;
                avgNormal += (triZ2 - triZ0) * sqZ;
            }
            else
            {
                avgNormal = triZ3;
                avgNormal += (triZ1 - triZ3) * (1.0f - sqZ);
                avgNormal += (triZ2 - triZ3) * (1.0f - sqX);
            }
            return avgNormal;
        }
         *
        private void SetupTerrainNormals(float scaleFactor)
        {
            VertexTerrain[] terrainVertices = new VertexTerrain[this.size * this.size];
            this.normals = new Vector3[this.size, this.size];

            // Determine vertex positions so we can figure out normals in section below.
            for(int x = 0; x < this.size; ++x)
                for(int z = 0; z < this.size; ++z)
                {
                    terrainVertices[x + z * this.size].Position = new Vector3(x * scaleFactor, this.heightData[x, z], z * scaleFactor);
                }

            // Setup normals for lighting and physics (Credit: Riemer's method)
            int sizeMinusOne = this.size - 1;
            for (int x = 1; x < sizeMinusOne; ++x)
                for (int z = 1; z < sizeMinusOne; ++z)
                {
                    int ZTimesSize = (z * this.size);
                    Vector3 normX = new Vector3((terrainVertices[x - 1 + ZTimesSize].Position.Y - terrainVertices[x + 1 + ZTimesSize].Position.Y) / 2, 1, 0);
                    Vector3 normZ = new Vector3(0, 1, (terrainVertices[x + (z - 1) * this.size].Position.Y - terrainVertices[x + (z + 1) * this.size].Position.Y) / 2);

                    // We inline the normalize method here since it is used alot, this is faster than calling Vector3.Normalize()
                    Vector3 normal = normX + normZ;
                    float length = (float)Math.Sqrt( (float)((normal.X * normal.X) + (normal.Y * normal.Y) + (normal.Z * normal.Z)) );
                    float num = 1f / length;
                    normal.X *= num;
                    normal.Y *= num;
                    normal.Z *= num;

                    this.normals[x, z] = terrainVertices[x + ZTimesSize].Normal = normal;    // Stored for use in physics and for the
                                                                                             // quad-tree component to reference.
                }
        }
        */

        #region Creation
        private float getHeightRaw(int x, int y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > heightmap.Width - 1) x = heightmap.Width - 1;
            if (y > heightmap.Height - 1) y = heightmap.Height - 1;
            return translateHeight(heightmap.Heights[x, y]); // possible optimization: store heights in setupGeometry() instead of recalculating heights from bitmap data
        }

        private float translateHeight(float raw)
        {
            //return raw;
            return maxHeight * raw / 255f;
        }

        private bool setupGeometry()
        {
            int x, z;
            //float halfWidth = bitmap.Width / 2f;
            //float halfHeight = bitmap.Height / 2f;

            // generate vertices
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();

            heightsArray = new float[heightmap.Width,heightmap.Height];
            float max = 0.0f;

            for (z = 0; z < heightmap.Height; z++)
            {
                for (x = 0; x < heightmap.Width; x++)
                {
                    // add vertex
                    float s11 = getHeightRaw(x,z);
                    vertices.Add(new Vector3(x * segmentSize, s11, z * segmentSize));

                    heightsArray[x, z] = s11;
                    if (s11 > max) max = s11;

                    // calculate and add normal
                    float s01 = getHeightRaw(x - 1, z);
                    float s21 = getHeightRaw(x + 1, z);
                    float s10 = getHeightRaw(x, z - 1);
                    float s12 = getHeightRaw(x, z + 1);

                    Vector3 va = new Vector3(2.0f, 0.0f, s21 - s01).Normalized();
                    Vector3 vb = new Vector3(0.0f, 2.0f, s12 - s10).Normalized();
                    Vector3 normal = Vector3.Cross(va, vb);
                    normal = normal.Xzy;
                    normals.Add(normal);
                }
            }

            // generate faces
            int index1, index2, index3, index4;
            List<int> indices = new List<int>();
            for (z = 0; z < heightmap.Height - 1; z++)
            {
                for (x = 0; x < heightmap.Width - 1; x++)
                {
                    // indices
                    index1 = x + z * heightmap.Width;
                    index2 = index1 + 1;
                    index3 = index1 + heightmap.Width;
                    index4 = index2 + heightmap.Width;

                    // tri 1
                    indices.Add(index1);
                    indices.Add(index2);
                    indices.Add(index3);

                    // tri 2
                    indices.Add(index2);
                    indices.Add(index4);
                    indices.Add(index3);
                }
            }

            positionVboData = vertices.ToArray();
            normalVboData = normals.ToArray();
            indicesVboData = indices.ToArray();

            Size = new Vector3(heightmap.Width * segmentSize, max, heightmap.Height * segmentSize);

            return true;
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
