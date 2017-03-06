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
    public class LevelTerrain : Model
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


        public AtlasMaterial material;
        public DirectionalLight light;
        public Vector4 TintColor;
        public float Opacity = 1;
        public float Grayscale = 0;
        public float Wireframe = 0;

        private LevelFile levelRes;

        private const float segmentSize = 1.0f; // must be 1 for Hi-Octane !!
        private float Time = 0.0f;

        private bool dirtyPositions = false;
        private bool dirtyNormals = false;
        private bool dirtyUVs = false;

        public LevelTerrain(string name, LevelFile levelRes, AtlasMaterial material) : base()
        {
            Name = name;
            this.levelRes = levelRes;

            TintColor = new Vector4(1, 1, 1, 0);

            this.material = material;
            if (!material.Ready) return;

            if (setupGeometry())
            {
                Log.WriteLine("HiOctane Terrain '" + name + "' loaded " +
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


        #region Mesh creation/modification

        public MapEntry GetMapEntry(int x, int y)
        {
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            if (x > levelRes.Width - 1) x = levelRes.Width - 1;
            if (y > levelRes.Height - 1) y = levelRes.Height - 1;
            return levelRes.Map[x, y];
        }

        public float GetHeightInterpolated(float x, float z)
        {
            //NOTE: x and z have to be valid grid coordinates

            // world to grid coords
            int ix = (int)Math.Round(x);
            int iz = (int)Math.Round(z);

            // check bounds
            if (ix < 0) ix = 0;
            if (iz < 0) iz = 0;

            if (ix > levelRes.Width - 1) ix = levelRes.Width - 1;
            if (iz > levelRes.Height - 1) iz = levelRes.Height - 1;

            int xPlusOne = Math.Min(ix + 1, levelRes.Width - 1);
            int zPlusOne = Math.Min(iz + 1, levelRes.Height - 1);

            // determine exact height
            int ia = 4 * (ix       + iz       * levelRes.Width);
            int ib = 4 * (xPlusOne + iz       * levelRes.Width);
            int ic = 4 * (xPlusOne + zPlusOne * levelRes.Width);
            int id = 4 * (ix       + zPlusOne * levelRes.Width);

            // get heights and proceed...
            float a = positionVboData[ia].Y;
            float b = positionVboData[ib].Y;
            float c = positionVboData[ic].Y;
            float d = positionVboData[id].Y;

            float height = 0.0f;

            float sqX = x - ix;
            float sqZ = z - iz;

            if (sqX + sqZ < 1)
            {
                height = a;
                height += (b - a) * sqX;
                height += (d - a) * sqZ;
            }
            else
            {
                height = c;
                height += (b - c) * (1.0f - sqZ);
                height += (d - c) * (1.0f - sqX);
            }
            return height;
        }

        public void ApplyHeight(MapEntry entry)
        {
            // set y positions for all vertices touching entry position

            int perRow = levelRes.Width;

            // update positions
            int id = 4 * (entry.X + entry.Z * perRow);
            positionVboData[id].Y = entry.Height;

            // ends of map must have same height as entry
            // otherwise we lose a row and column of the map

            bool onLastX = entry.X == levelRes.Width - 1;
            bool onLastZ = entry.Z == levelRes.Height - 1;

            if (onLastX)
            {
                positionVboData[id + 1].Y = entry.Height;
            }

            if (onLastZ)
            {
                positionVboData[id + 3].Y = entry.Height;
            }

            if (onLastX && onLastZ)
            {
                positionVboData[id + 2].Y = entry.Height;
            }

            if (entry.X - 1 >= 0)
            {
                id = 4 * (entry.X - 1 + entry.Z * perRow);  // left entry
                positionVboData[id + 1].Y = entry.Height;   // top/right vertex
                if (onLastZ) positionVboData[id + 2].Y = entry.Height;      // bottom/left
            }

            if (entry.Z - 1 >= 0)
            {
                id = 4 * (entry.X + (entry.Z - 1) * perRow); // prev row
                positionVboData[id + 3].Y = entry.Height;    // bottom/left vertex of entry on prev row
                if (onLastX) positionVboData[id + 2].Y = entry.Height;      // bottom/right
            }

            if (entry.X - 1 >= 0 && entry.Z - 1 >= 0)
            {
                id = 4 * (entry.X - 1 + (entry.Z - 1) * perRow); // left entry on prev row
                positionVboData[id + 2].Y = entry.Height; // bottom/right vertex
            }

            dirtyPositions = true;
        }

        public void Morph(Morph morph)
        {
            if (morph.Progress == morph.LastProgress) return;

            dirtyPositions = true;

            int xSource = (int)morph.Source.Pos.X - 1;
            int zSource = (int)morph.Source.Pos.Z - 1;
            int xTarget = (int)morph.Target.Pos.X - 1;
            int zTarget = (int)morph.Target.Pos.Z - 1;
            List<MapEntry> refreshNormals = new List<MapEntry>();

            // check if UVs need an update
            bool sourceEnabled = morph.Progress > 0.01f;
            bool updateUVs = morph.UVsFromSource != sourceEnabled;
            morph.UVsFromSource = sourceEnabled;
            if (updateUVs) dirtyUVs = true;

            // apply morph to positions and UVs
            for (int dz = 0; dz < morph.Height + 1; dz++)
            {
                for (int dx = 0; dx < morph.Width + 1; dx++)
                {
                    // source entries
                    int x = xSource + dx;
                    int z = zSource + dz;

                    MapEntry a = GetMapEntry(x, z);
                    MapEntry b = GetMapEntry(x + 1, z);
                    MapEntry c = GetMapEntry(x + 1, z + 1);
                    MapEntry d = GetMapEntry(x, z + 1);

                    // target entries
                    x = xTarget + dx;
                    z = zTarget + dz;

                    MapEntry e = GetMapEntry(x, z);
                    MapEntry f = GetMapEntry(x + 1, z);
                    MapEntry g = GetMapEntry(x + 1, z + 1);
                    MapEntry h = GetMapEntry(x, z + 1);

                    // collect entries to update normals below
                    if (!refreshNormals.Contains(a)) refreshNormals.Add(a);
                    if (!refreshNormals.Contains(e)) refreshNormals.Add(e);

                    // calculate and set new Y values
                    int id = 4 * (x + z * levelRes.Width);

                    if (dx > 0 && dz > 0)
                        positionVboData[id].Y = e.Height * (1f - morph.Progress) + a.Height * morph.Progress;
                    if (dx < morph.Width && dz > 0)
                        positionVboData[id + 1].Y = f.Height * (1f - morph.Progress) + b.Height * morph.Progress;
                    if (dx < morph.Width && dz < morph.Height)
                        positionVboData[id + 2].Y = g.Height * (1f - morph.Progress) + c.Height * morph.Progress;
                    if (dx > 0 && dz < morph.Height)
                        positionVboData[id + 3].Y = h.Height * (1f - morph.Progress) + d.Height * morph.Progress;

                    // set UVs either from source or from target
                    if (updateUVs && dx > 0 && dz > 0)
                    {
                        List<Vector2> uvs = sourceEnabled && !morph.Permanent ?
                            material.MakeUVs(a.TextureId, a.TextureModification) :
                            material.MakeUVs(e.TextureId, e.TextureModification);

                        uvVboData[id] = uvs[0];
                        uvVboData[id + 1] = uvs[1];
                        uvVboData[id + 2] = uvs[2];
                        uvVboData[id + 3] = uvs[3];
                    }
                }
            }

            foreach (MapEntry entry in refreshNormals)
            {
                RecalculateNormals(entry);
            }
        }


        private bool setupGeometry()
        {
            int x, z, i = 0;

            // generate vertices
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();

            float max = 0.0f;
            List<int> indices = new List<int>();

            for (z = 0; z < levelRes.Height; z++)
            {
                for (x = 0; x < levelRes.Width; x++)
                {
                    // 4 vertices - need separate UVs so cannot share
                    MapEntry a = GetMapEntry(x, z);
                    MapEntry b = GetMapEntry(x + 1, z);
                    MapEntry c = GetMapEntry(x + 1, z + 1);
                    MapEntry d = GetMapEntry(x, z + 1);

                    vertices.Add(new Vector3(x       * segmentSize, a.Height, z * segmentSize));
                    vertices.Add(new Vector3((x + 1) * segmentSize, b.Height, z * segmentSize));
                    vertices.Add(new Vector3((x + 1) * segmentSize, c.Height, (z + 1) * segmentSize));
                    vertices.Add(new Vector3(x       * segmentSize, d.Height, (z + 1) * segmentSize));

                    // texture atlas 4 UVs
                    uvs.AddRange(material.MakeUVs(a.TextureId, a.TextureModification));

                    // add normals
                    normals.Add(computeNormalFromMapEntries(x    , z    , 1.0f));
                    normals.Add(computeNormalFromMapEntries(x + 1, z    , 1.0f));
                    normals.Add(computeNormalFromMapEntries(x + 1, z + 1, 1.0f));
                    normals.Add(computeNormalFromMapEntries(x    , z + 1, 1.0f));

                    // add indices for the 2 tris
                    indices.Add(i);
                    indices.Add(i + 1);
                    indices.Add(i + 3);

                    indices.Add(i + 1);
                    indices.Add(i + 2);
                    indices.Add(i + 3);

                    i += 4;

                    // determine max height
                    max = Math.Max(max, a.Height);
                }
            }

            positionVboData = vertices.ToArray();
            normalVboData = normals.ToArray();
            indicesVboData = indices.ToArray();
            uvVboData = uvs.ToArray();

            Size = new Vector3(levelRes.Width * segmentSize, max, levelRes.Height * segmentSize);

            return true;
        }

        #endregion

        #region Normals
        public void RecalculateNormals(MapEntry entry)
        {
            int max = positionVboData.Length;
            int perRow = levelRes.Width;

            // update normals
            for (int z = entry.Z - 1; z <= entry.Z + 1; z++)
            {
                for (int x = entry.X - 1; x <= entry.X + 1; x++)
                {
                    if (x > levelRes.Width - 1 || z > levelRes.Height - 1) continue;

                    int id = 4 * (x + z * perRow);
                    if (id < 0 || id >= max) continue;

                    normalVboData[id    ] = computeNormalFromPositionsBuffer(x, z, 1.0f);
                    normalVboData[id + 1] = computeNormalFromPositionsBuffer(x + 1, z, 1.0f);
                    normalVboData[id + 2] = computeNormalFromPositionsBuffer(x + 1, z + 1, 1.0f);
                    normalVboData[id + 3] = computeNormalFromPositionsBuffer(x, z + 1, 1.0f);
                }
            }

            dirtyNormals = true;
        }

        private Vector3 computeNormalFromPositionsBuffer(int x, int z, float intensity)
        {
            if (x < 0 || x > levelRes.Width - 1 || z < 0 || z > levelRes.Height - 1) return Vector3.UnitY;

            int perLine = 4 * levelRes.Width;
            int id = 4 * x + z * perLine;

            float h = positionVboData[id].Y;

            float a = z - 1 >= 0 ? positionVboData[id - perLine].Y : h;
            float b = x + 1 < levelRes.Width ? positionVboData[id + 4].Y : h;
            float c = z + 1 < levelRes.Height ? positionVboData[id + perLine].Y : h;
            float d = x - 1 >= 0 ? positionVboData[id - 4].Y : h;

            Vector3 nA = new Vector3(0f, a - h, -1f) * intensity;
            Vector3 nB = new Vector3(1f, b - h, 0f) * intensity;
            Vector3 nC = new Vector3(0f, c - h, 1f) * intensity;
            Vector3 nD = new Vector3(-1f, d - h, 0f) * intensity;

            Vector3 normal = -0.25f * (Vector3.Cross(nA, nB) + Vector3.Cross(nB, nC) + Vector3.Cross(nC, nD) + Vector3.Cross(nD, nA));
            normal.Normalize();

            return normal;
        }

        private Vector3 computeNormalFromMapEntries(int x, int z, float intensity)
        {
            MapEntry a = GetMapEntry(x    , z - 1);
            MapEntry b = GetMapEntry(x + 1, z);
            MapEntry c = GetMapEntry(x    , z + 1);
            MapEntry d = GetMapEntry(x - 1, z);

            MapEntry n = GetMapEntry(x, z);

            Vector3 nA = new Vector3( 0f, a.Height - n.Height, -1f) * intensity;
            Vector3 nB = new Vector3( 1f, b.Height - n.Height,  0f) * intensity;
            Vector3 nC = new Vector3( 0f, c.Height - n.Height,  1f) * intensity;
            Vector3 nD = new Vector3(-1f, d.Height - n.Height,  0f) * intensity;

            Vector3 normal = -0.25f * (Vector3.Cross(nA, nB) + Vector3.Cross(nB, nC) + Vector3.Cross(nC, nD) + Vector3.Cross(nD, nA));
            normal.Normalize();

            return normal;
        }

        #endregion

        #region Buffers
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
            setNormalsBuffer();

            GL.GenBuffers(1, out uvVboHandle);
            setUVsBuffer();
//            GL.BindBuffer(BufferTarget.ArrayBuffer, uvVboHandle);
            //GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(uvVboData.Length * Vector2.SizeInBytes), uvVboData, BufferUsageHint.StaticDraw);

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

        private void setNormalsBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(normalVboData.Length * Vector3.SizeInBytes), normalVboData, BufferUsageHint.StaticDraw);

            ErrorCode e = GL.GetError();
            if (e != ErrorCode.NoError) Log.WriteLine(Log.LOG_ERROR, "Failed to set normals buffer data " + e.ToString());

            dirtyNormals = false;
        }

        private void setPositionsBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(positionVboData.Length * Vector3.SizeInBytes), positionVboData, BufferUsageHint.StaticDraw);

            ErrorCode e = GL.GetError();
            if (e != ErrorCode.NoError) Log.WriteLine(Log.LOG_ERROR, "Failed to set positions buffer data " + e.ToString());

            dirtyPositions = false;
        }

        private void setUVsBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, uvVboHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(uvVboData.Length * Vector2.SizeInBytes), uvVboData, BufferUsageHint.StaticDraw);

            ErrorCode e = GL.GetError();
            if (e != ErrorCode.NoError) Log.WriteLine(Log.LOG_ERROR, "Failed to set UVs buffer data " + e.ToString());

            dirtyUVs = false;
        }
        #endregion

        #region Rendering
        protected override void render()
        {
            if (material == null) return;

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
            material.SetUniform("model", transformation);
            material.SetUniform("modelView", mv);
            material.SetUniform("modelViewProjection", Matrix4.Mult(mv, EngineBase.Renderer.Projection));

            if (light == null) light = (DirectionalLight)EngineBase.Scene.GetLight(0);
            material.SetUniform("lightDirection", light.Direction);

            material.SetUniform("color", ref TintColor);
            material.SetUniform("opacity", Opacity);
            material.SetUniform("grayscale", Grayscale);
            material.SetUniform("wireframe", Wireframe);
            material.SetUniform("time", Time);

            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Triangles, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            material.Unbind();
        }

        public override void Update(float time)
        {
            Time += time;

            if (dirtyPositions)
            {
                Editor.Profiler.Begin("GEOMETRY UPD (GPU)", true, 5);
                setPositionsBuffer();
                Editor.Profiler.End("GEOMETRY UPD (GPU)");
            }

            if (dirtyNormals)
            {
                Editor.Profiler.Begin("NORMALS UPD (GPU)", true, 5);
                setNormalsBuffer();
                Editor.Profiler.End("NORMALS UPD (GPU)");
            }

            if (dirtyUVs)
            {
                //Editor.Profiler.Begin("MORPH UVS(GPU)", true, 5);
                setUVsBuffer();
                //Editor.Profiler.End("MORPH UVS(GPU)");
            }

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
