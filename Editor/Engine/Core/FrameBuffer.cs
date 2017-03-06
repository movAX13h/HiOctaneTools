using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using LevelEditor.Engine.Lights;
using LevelEditor.Utils;

namespace LevelEditor.Engine.Core
{
    abstract class FrameBuffer
    {
        public string Name { get; protected set; }
        public bool Ready { get; protected set; }
        public uint TextureHandle { get { return textureHandle; } }

        public Vector2 Size { get; private set; }

        protected uint textureHandle;
        protected uint fboHandle;
        protected int width;
        protected int height;

        public FrameBuffer(int w, int h)
        {
            width = w;
            height = h;

            Name = "Unnamed";

            Size = new Vector2(w, h);

            Ready = false;
        }

        public virtual bool Create()
        {
            return Ready;
        }

        public virtual void Delete()
        {

        }

        public virtual void Bind()
        {
        }

        public virtual void Unbind()
        {
        }

        protected bool CheckFboStatus()
        {
            string id = "FBO '" + Name + "' (" + width + "/" + height + "): ";

            switch (GL.Ext.CheckFramebufferStatus(FramebufferTarget.FramebufferExt))
            {
                case FramebufferErrorCode.FramebufferCompleteExt:
                    {
                        Log.WriteLine(id + "The framebuffer is complete and valid for rendering.");
                        return true;
                    }
                case FramebufferErrorCode.FramebufferIncompleteAttachmentExt:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "One or more attachment points are not framebuffer attachment complete. This could mean there’s no texture attached or the format isn’t renderable. For color textures this means the base format must be RGB or RGBA and for depth textures it must be a DEPTH_COMPONENT format. Other causes of this error are that the width or height is zero or the z-offset is out of range in case of render to volume.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteMissingAttachmentExt:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "There are no attachments.");
                        break;
                    }
                /* case  FramebufferErrorCode.GL_FRAMEBUFFER_INCOMPLETE_DUPLICATE_ATTACHMENT_EXT:
                     {
                         Trace.WriteLine(id + "An object has been attached to more than one attachment point.");
                         break;
                     }*/
                case FramebufferErrorCode.FramebufferIncompleteDimensionsExt:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "Attachments are of different size. All attachments must have the same width and height.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteFormatsExt:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "The color attachments have different format. All color attachments must have the same format.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteDrawBufferExt:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "An attachment point referenced by GL.DrawBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferIncompleteReadBufferExt:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "The attachment point referenced by GL.ReadBuffers() doesn’t have an attachment.");
                        break;
                    }
                case FramebufferErrorCode.FramebufferUnsupportedExt:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "This particular FBO configuration is not supported by the implementation.");
                        break;
                    }
                default:
                    {
                        Log.WriteLine(Log.LOG_ERROR, id + "Status unknown. (yes, this is really bad.)");
                        break;
                    }
            }
            return false;
        }

    }
}
