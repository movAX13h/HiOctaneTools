using System;

using LevelEditor.Engine;
using LevelEditor.Engine.Core;
using LevelEditor.Engine.Lights;
using LevelEditor.Engine.Materials;
using LevelEditor.Utils;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace LevelEditor.Games.HiOctane.Models
{
    class Box : Model
    {
        private int vaoHandle;
        protected int positionVboHandle;
        protected int normalVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected Vector3[] normalVboData;
        protected int[] indicesVboData;

        public LightMaterial material;
        public DirectionalLight light;
        public Vector4 Color;

        //private Line[] normalLines;

        public Box(float size, Vector4 col)
            : this(0, 0, 0, size, size, size, col)
        { }
        public Box(float size, Vector3 pos, Vector4 col)
            : this(0, 0, 0, size, size, size, col)
        {
            Position = pos;
        }

        public Box(float negX, float negY, float negZ, float posX, float posY, float posZ, Vector4 col):base()
        {
            Name = "Box";

            Color = col;
            Size = new Vector3(posX - negX, posY - negY, posZ - negZ);

            positionVboData = new Vector3[]
            {
                new Vector3(negX, negY, posZ), new Vector3(negX, posY, posZ), new Vector3(posX, posY, posZ), new Vector3(posX, negY, posZ), // front
                new Vector3(posX, negY, posZ), new Vector3(posX, posY, posZ), new Vector3(posX, posY, negZ), new Vector3(posX, negY, negZ), // right
                new Vector3(posX, negY, negZ), new Vector3(posX, posY, negZ), new Vector3(negX, posY, negZ), new Vector3(negX, negY, negZ), // back
                new Vector3(negX, negY, negZ), new Vector3(negX, posY, negZ), new Vector3(negX, posY, posZ), new Vector3(negX, negY, posZ), // left
                new Vector3(negX, posY, posZ), new Vector3(negX, posY, negZ), new Vector3(posX, posY, negZ), new Vector3(posX, posY, posZ), // top
                new Vector3(negX, negY, negZ), new Vector3(negX, negY, posZ), new Vector3(posX, negY, posZ), new Vector3(posX, negY, negZ)  // bottom
            };

            normalVboData = new Vector3[]
            {
                new Vector3( 0f,  0f,  1f), new Vector3( 0f,  0f,  1f), new Vector3( 0f,  0f,  1f), new Vector3( 0f,  0f,  1f), // front
                new Vector3( 1f,  0f,  0f), new Vector3( 1f,  0f,  0f), new Vector3( 1f,  0f,  0f), new Vector3( 1f,  0f,  0f), // right
                new Vector3( 0f,  0f, -1f), new Vector3( 0f,  0f, -1f), new Vector3( 0f,  0f, -1f), new Vector3( 0f,  0f, -1f), // back
                new Vector3(-1f,  0f,  0f), new Vector3(-1f,  0f,  0f), new Vector3(-1f,  0f,  0f), new Vector3(-1f,  0f,  0f), // left
                new Vector3( 0f,  1f,  0f), new Vector3( 0f,  1f,  0f), new Vector3( 0f,  1f,  0f), new Vector3( 0f,  1f,  0f), // top
                new Vector3( 0f, -1f,  0f), new Vector3( 0f, -1f,  0f), new Vector3( 0f, -1f,  0f), new Vector3( 0f, -1f,  0f)  // bottom
            };

            indicesVboData = new int[]
            {
                 0,  1,  2,  0,  2,  3, // front face
                 4,  5,  6,  4,  6,  7, // right face
                 8,  9, 10,  8, 10, 11, // back face
                12, 13, 14, 12, 14, 15, // left face
                16, 17, 18, 16, 18, 19, // top face
                20, 21, 22, 20, 22, 23  // bottom face
            };

            material = new LightMaterial();
            if (!material.Ready) return;

            createVBOs();
            createVAOs();

            ready = true;
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

        protected override void render()
        {
            material.Bind();

            if (EngineBase.Renderer.ShadowFrameBuffer.TextureHandle > 0)
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

            material.SetUniform("color", ref Color);

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

    }
}
