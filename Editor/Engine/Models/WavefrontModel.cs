using System;
using System.Runtime.InteropServices;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Resources.Loaders;
using LevelEditor.Engine.Core;
using LevelEditor.Utils;
using LevelEditor.Engine.Materials;
using LevelEditor.Engine.Lights;

namespace LevelEditor.Engine.Models
{
    public class WavefrontModel : Model
    {
        private int vaoHandle;
        private int eboHandle;

        private int positionVboHandle;
        private int normalVboHandle;
        private int textureCoordsVboHandle;

        private Vector3[] vertices;
        private int[] indices;
        private Vector2[] texCoords;
        private Vector3[] normals;

        public PrimitiveType PolygonType;

        public Vector3[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        public Vector3[] Normals
        {
            get { return normals; }
            set { normals = value; }
        }

        public int[] Indices
        {
            get { return indices; }
            set { indices = value; }
        }

        public Vector2[] TexCoords
        {
            get { return texCoords; }
            set { texCoords = value; }
        }

        public DirectionalLight light;
        private TextureMaterial material;
        public Vector4 TintColor;
        public float Opacity = 1;
        public float Grayscale = 0;

        public WavefrontModel(string name, string filename)
            : this(name, filename, "", "")
        { }

        public WavefrontModel(string name, string filename, string textureFilename)
            : this(name, filename, textureFilename, "")
        { }

        public WavefrontModel(string name, string filename, string textureFilename, string normalsFilename)
        {
            Name = name;
            TintColor = new Vector4(1, 1, 0, 0); // alpha is intensity

            // load model
            if (WavefrontLoader.Load(this, filename))
            {
                Log.WriteLine(Log.LOG_INFO, "WavefrontModel '" + name + "' loaded " + vertices.Length + " vertices, " + normals.Length + " normals, " + texCoords.Length + " texture coords, " +
                              indices.Length + " indices, " + "using " + PolygonType.ToString() + " -> " + (indices.Length / (PolygonType == PrimitiveType.Quads ? 4 : 3)) + " polygons");
            }
            else
            {
                Log.WriteLine(Log.LOG_ERROR, "WavefrontModel '" + name + "' failed loading '" + filename + "' - might be incompatible");
                return;
            }

            // load material
            material = new TextureMaterial(textureFilename, normalsFilename, normals.Length > 0, true);
            if (!material.Ready)
            {
                Log.WriteLine(Log.LOG_ERROR, "failed to load WavefrontModel material");
                return;
            }

            // sanity check for normal information
            /*
            if (!material.UseNormalMap && normals.Length == 0)
            {
                Log.WriteLine(Log.LOG_ERROR, "WavefrontModel '" + name + "' is not using a normal map and does not have vertex normals");
                return;
            }*/

            // create GL objects
            createVBOs();
            createVAOs();

            ready = true;
        }

        protected override void createVBOs()
        {
            if (vertices == null || indices == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VBOs without positionVboData and indicesVboData for model '" + Name + "'");
                return;
            }

            GL.GenBuffers(1, out positionVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(vertices.Length * Vector3.SizeInBytes), vertices, BufferUsageHint.StaticDraw);

            if (normals.Length > 0)
            {
                GL.GenBuffers(1, out normalVboHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
                GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(normals.Length * Vector3.SizeInBytes), normals, BufferUsageHint.StaticDraw);
            }

            GL.GenBuffers(1, out textureCoordsVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordsVboHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(texCoords.Length * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out eboHandle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indices.Length), indices, BufferUsageHint.StaticDraw);

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

            if (normals.Length > 0)
            {
                index++;
                GL.EnableVertexAttribArray(index);
                GL.BindBuffer(BufferTarget.ArrayBuffer, normalVboHandle);
                GL.VertexAttribPointer(index, 3, VertexAttribPointerType.Float, true, Vector3.SizeInBytes, 0);
                GL.BindAttribLocation(material.ProgramHandle, index, "in_normal");
            }

            if (material.UseTexture || material.UseNormalMap)
            {
                index++;
                GL.EnableVertexAttribArray(index);
                GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordsVboHandle);
                GL.VertexAttribPointer(index, 2, VertexAttribPointerType.Float, true, Vector2.SizeInBytes, 0);
                GL.BindAttribLocation(material.ProgramHandle, index, "in_texcoord");
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle); // don't know why this is here...
            GL.BindVertexArray(0);
        }

        protected override void render()
        {
            material.Bind();

            if (material.UseTexture)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, material.TextureHandle);
                material.SetUniform("colorMap", 0);
            }

            if (material.UseNormalMap)
            {
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, material.NormalsHandle);
                material.SetUniform("normalMap", 1);
            }

            if (EngineBase.Renderer.ShadowFrameBuffer.TextureHandle > 0)
            {
                GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture2D, EngineBase.Renderer.ShadowFrameBuffer.TextureHandle);
                material.SetUniform("shadowMap", 2);
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


            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PolygonType, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
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
            GL.DrawElements(PolygonType, indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }

    }
}
