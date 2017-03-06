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

namespace LevelEditor.Games.HiOctane.Models
{
    class BillboardAnimation : Model
    {
        private int vaoHandle;
        protected int positionVboHandle;
        private int textureCoordsVboHandle;
        protected int eboHandle;

        protected Vector3[] positionVboData;
        protected int[] indicesVboData;
        private Vector2[] texCoords;

        public BillboardAnimationMaterial material;

        private Vector2 frameSize;
        private int numFrames = 0;
        private int currentFrame = 0;
        private float currentTime = 0;
        private float fps = 10;

        public BillboardAnimation(string filename, float w, float h, int frameW, int frameH, float speed)
            : base()
        {
            Name = "Billboard Animation";

            fps = speed;

            Size = new Vector3(w, h, 0.01f);

            float w2 = w * 0.5f;
            float h2 = h * 0.5f;

            positionVboData = new Vector3[] {
                new Vector3(-w2, -h2, 0),
                new Vector3(w2, -h2, 0),
                new Vector3(w2, h2, 0),
                new Vector3(-w2, h2, 0)
            };

            texCoords = new Vector2[] {
                new Vector2(0,1),
                new Vector2(1,1),
                new Vector2(1,0),
                new Vector2(0,0)
            };

            indicesVboData = new int[] { 0, 1, 2, 3 };

            material = new BillboardAnimationMaterial(filename);
            if (!material.Ready) return;

            numFrames = material.TextureResource.Width / frameW;
            frameSize = new Vector2(1f / numFrames, 1f);

            createVBOs();
            createVAOs();

            ready = true;
        }

        protected override void createVBOs()
        {
            if (positionVboData == null || indicesVboData == null)
            {
                Log.WriteLine(Log.LOG_ERROR, "can not create VBOs without positionVboData and indicesVboData for model '" + Name + "'");
                return;
            }

            GL.GenBuffers(1, out positionVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, positionVboHandle);
            GL.BufferData<Vector3>(BufferTarget.ArrayBuffer, new IntPtr(positionVboData.Length * Vector3.SizeInBytes), positionVboData, BufferUsageHint.StaticDraw);

            GL.GenBuffers(1, out textureCoordsVboHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordsVboHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(texCoords.Length * Vector2.SizeInBytes), texCoords, BufferUsageHint.StaticDraw);

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

            if (material.UseTexture)
            {
                GL.EnableVertexAttribArray(1);
                GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordsVboHandle);
                GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, true, Vector2.SizeInBytes, 0);
                GL.BindAttribLocation(material.ProgramHandle, 1, "in_texcoord");
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, eboHandle);
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

            if (EngineBase.Renderer.ShadowFrameBuffer.TextureHandle > 0 && material.UseShadowMap)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, EngineBase.Renderer.ShadowFrameBuffer.TextureHandle);
                material.SetUniform("shadowMap", 1);
                material.SetUniform("shadowViewProjection", Matrix4.Mult(shadowViewProjection, EngineBase.Renderer.ShadowMapBias));
            }

            material.SetUniform("projectionMatrix", EngineBase.Renderer.Projection);
            material.SetUniform("modelViewMatrix", Matrix4.Mult(transformation, EngineBase.Renderer.View));



            material.SetUniform("frameSize", frameSize);
            material.SetUniform("offset", currentFrame * frameSize.X);

            GL.BindVertexArray(vaoHandle);
            GL.DrawElements(PrimitiveType.Quads, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);

            material.Unbind();
        }

        public override void Update(float time)
        {
            currentTime += time;
            currentFrame = (int)Math.Floor(fps * currentTime) % numFrames;
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
            GL.DrawElements(PrimitiveType.Quads, indicesVboData.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);
            GL.BindVertexArray(0);
        }
        */
    }
}
