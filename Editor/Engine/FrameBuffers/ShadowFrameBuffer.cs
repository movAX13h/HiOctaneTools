using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using LevelEditor.Engine.Lights;
using LevelEditor.Utils;
using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.FrameBuffers
{
    class ShadowFrameBuffer : FrameBuffer
    {
        public ShadowFrameBuffer(int w, int h):base(w, h)
        {
            Name = "Shadow";
        }

        public override bool Create()
        {
            // Create Color Texture
            GL.GenTextures(1, out textureHandle);
            GL.BindTexture(TextureTarget.Texture2D, TextureHandle);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);

            // test for GL Error (might be unsupported format)
            ErrorCode e = GL.GetError();
            if (e != ErrorCode.NoError)
            {
                Log.WriteLine(Log.LOG_ERROR, "FBO color texture error code: " + GL.GetError().ToString());
                return false;
            }

            GL.BindTexture(TextureTarget.Texture2D, 0); // prevent feedback

            // Create Depth Renderbuffer
            //GL.Ext.GenRenderbuffers(1, out DepthRenderbuffer);
            //GL.Ext.BindRenderbuffer(RenderbufferTarget.RenderbufferExt, DepthRenderbuffer);
            //GL.Ext.RenderbufferStorage(RenderbufferTarget.RenderbufferExt, (RenderbufferStorage)All.DepthComponent32, FboWidth, FboHeight);
            // test for GL Error (might be unsupported format)
            //e = GL.GetError();
            //if (e != ErrorCode.NoError) Log.WriteLine("FBO depth render buffer error code: " + GL.GetError().ToString());

            // Create a FBO and attach texture
            GL.Ext.GenFramebuffers(1, out fboHandle);
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fboHandle);

            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);

            GL.Ext.FramebufferTexture2D(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, textureHandle, 0);
            //GL.Ext.FramebufferRenderbuffer(FramebufferTarget.FramebufferExt, FramebufferAttachment.DepthAttachmentExt, RenderbufferTarget.RenderbufferExt, DepthRenderbuffer);

            Ready = CheckFboStatus();

            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // return to visible framebuffer
            return Ready;
        }

        public override void Delete()
        {
            GL.Ext.DeleteFramebuffer(fboHandle);
            GL.DeleteTexture(textureHandle);
        }

        public override void Bind()
        {
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, fboHandle);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.DrawBuffer((DrawBufferMode)FramebufferAttachment.ColorAttachment0Ext);

            GL.PushAttrib(AttribMask.ViewportBit); // stores GL.Viewport() parameters
            GL.Viewport(0, 0, width, height);
            GL.CullFace(CullFaceMode.Front);
        }

        public override void Unbind()
        {
            GL.PopAttrib(); // restores GL.Viewport() parameters
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0); // return to visible framebuffer
            GL.CullFace(CullFaceMode.Back);
        }
    }
}
