using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using LevelEditor.Engine.Lights;
using LevelEditor.Utils;
using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.FrameBuffers
{
    class DefaultFrameBuffer : FrameBuffer
    {
        private uint depthRenderBufferHandle;

        public DefaultFrameBuffer(int w, int h) : base(w, h)
        {
            Name = "Default";
        }

        public override bool Create()
        {
            // Create Color Texture
            GL.GenTextures(1, out textureHandle);
            GL.BindTexture(TextureTarget.Texture2D, textureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);

            // Test for GL Error (might be unsupported format)
            ErrorCode e = GL.GetError();
            if (e != ErrorCode.NoError)
            {
                Log.WriteLine(Log.LOG_ERROR, "FBO color texture error code: " + e.ToString());
                return false;
            }

            GL.BindTexture(TextureTarget.Texture2D, 0); // prevent feedback

            // Create Depth Renderbuffer
            GL.Ext.GenRenderbuffers(1, out depthRenderBufferHandle);
            GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, depthRenderBufferHandle);
            GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage)All.DepthComponent32, width, height);

            // Test for GL Error (might be unsupported format)
            e = GL.GetError();
            if (e != ErrorCode.NoError)
            {
                Log.WriteLine(Log.LOG_ERROR, "FBO depth render buffer error code: " + GL.GetError().ToString());
                return false;
            }

            // Create a FBO and attach texture
            GL.Ext.GenFramebuffers(1, out fboHandle);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fboHandle);

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);

            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.ColorAttachment0Ext, TextureTarget.Texture2D, textureHandle, 0);
            GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, depthRenderBufferHandle);

            Ready = CheckFboStatus();

            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // return to visible framebuffer
            return Ready;
        }

        public override void Delete()
        {
            GL.Ext.DeleteFramebuffer(fboHandle);

            ErrorCode e = GL.GetError();
            if (e != ErrorCode.NoError) Log.WriteLine("Failed to delete default framebuffer: " + e.ToString());

            GL.Ext.DeleteRenderbuffer(depthRenderBufferHandle);
            e = GL.GetError();
            if (e != ErrorCode.NoError) Log.WriteLine(Log.LOG_ERROR, "Failed to delete default renderbuffer: " + e.ToString());

            GL.DeleteTexture(textureHandle);
            e = GL.GetError();
            if (e != ErrorCode.NoError) Log.WriteLine(Log.LOG_ERROR, "Failed to delete default framebuffer texture: " + e.ToString());

        }

        public override void Bind()
        {
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fboHandle);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0Ext); // important!
        }

        public override void Unbind()
        {
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // return to visible framebuffer
        }
    }
}
