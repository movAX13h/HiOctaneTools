using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace LevelEditor.Engine.GUI
{

    public class DynamicTexture : IDisposable
    {
        private Bitmap bmp;
        private Graphics gfx;
        private int texture;
        private Rectangle dirtyRegion;
        private bool disposed;
        public TextureUnit Unit { get; private set; }
        public Size PixelSize { get { return bmp.Size; } }

        #region Constructors

        /// <summary>
        /// Constructs a new instance.
        /// </summary>
        /// <param name="width">The width in 96-DPI logical units.</param>
        /// <param name="height">The height in 96-DPI logical units.</param>
        /// <param name="scale">Backing-store pixels per logical unit.</param>
        public DynamicTexture(float width, float height, TextureUnit unit, TextRenderingHint renderingHint = TextRenderingHint.ClearTypeGridFit, float scale = 1f)
        {
            Unit = unit;

            if (width <= 0) throw new ArgumentOutOfRangeException("width");
            if (height <= 0) throw new ArgumentOutOfRangeException("height ");
            if (scale <= 0 || float.IsNaN(scale) || float.IsInfinity(scale)) throw new ArgumentOutOfRangeException("scale");
            if (GraphicsContext.CurrentContext == null) throw new InvalidOperationException("No GraphicsContext is current on the calling thread.");

            int pixelWidth = (int)Math.Ceiling(width * scale);
            int pixelHeight = (int)Math.Ceiling(height * scale);
            bmp = new Bitmap(pixelWidth, pixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            // Never inherit the desktop DPI: font measurements and control sizes
            // share a 96-DPI coordinate system, with a denser backing store only.
            bmp.SetResolution(96, 96);
            gfx = Graphics.FromImage(bmp);
            // Extra edge pixels are padding, not a reason to stretch the font.
            gfx.ScaleTransform(scale, scale);

            gfx.TextRenderingHint = renderingHint;
            //gfx.TextRenderingHint = TextRenderingHint.AntiAlias;
            //gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            //gfx.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

            texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, pixelWidth, pixelHeight, 0, PixelFormat.Rgba, PixelType.UnsignedByte, IntPtr.Zero);
        }

        #endregion

        #region Public Members

        public void Bind()
        {
            GL.ActiveTexture(Unit);
            GL.BindTexture(TextureTarget.Texture2D, Texture);
        }

        /// <summary>
        /// Clears the backing store to the specified color.
        /// </summary>
        /// <param name="color">A <see cref="System.Drawing.Color"/>.</param>
        public void Clear(Color color)
        {
            gfx.Clear(color);
            dirtyRegion = new Rectangle(0, 0, bmp.Width, bmp.Height);
        }

        /// <summary>
        /// Draws the specified string to the backing store.
        /// </summary>
        /// <param name="text">The <see cref="System.String"/> to draw.</param>
        /// <param name="font">The <see cref="System.Drawing.Font"/> that will be used.</param>
        /// <param name="brush">The <see cref="System.Drawing.Brush"/> that will be used.</param>
        /// <param name="point">The location of the text on the backing store, in 2d pixel coordinates.
        /// The origin (0, 0) lies at the top-left corner of the backing store.</param>
        public SizeF DrawString(string text, Font font, Brush brush, PointF point)
        {
            gfx.DrawString(text, font, brush, point);

            // Layout must not change with rasterization density or font hinting.
            var state = gfx.Save();
            gfx.ResetTransform();
            SizeF size = gfx.MeasureString(text, font);
            gfx.Restore(state);
            // Upload full rows, including antialiasing outside the measured bounds.
            dirtyRegion = new Rectangle(0, 0, bmp.Width, bmp.Height);
            return size;
        }

        public void DrawRectangle(int x, int y, int w, int h, Brush brush)
        {
            gfx.FillRectangle(brush, x, y, w, h);
            dirtyRegion = new Rectangle(0, 0, bmp.Width, bmp.Height); // always everything for now
        }

        public void DrawLine(int x1, int y1, int x2, int y2, Pen pen)
        {
            gfx.DrawLine(pen, x1, y1, x2, y2);
            dirtyRegion = new Rectangle(0, 0, bmp.Width, bmp.Height); // always everything for now
        }

        /// <summary>
        /// Gets a <see cref="System.Int32"/> that represents an OpenGL 2d texture handle.
        /// The texture contains a copy of the backing store. Bind this texture to TextureTarget.Texture2d
        /// in order to render the drawn text on screen.
        /// </summary>
        public int Texture
        {
            get
            {
                uploadBitmap();
                return texture;
            }
        }

        #endregion

        #region Private Members

        // Uploads the dirty regions of the backing store to the OpenGL texture.
        void uploadBitmap()
        {
            if (dirtyRegion != RectangleF.Empty)
            {
                System.Drawing.Imaging.BitmapData data = bmp.LockBits(dirtyRegion,
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.TexSubImage2D(TextureTarget.Texture2D, 0,
                    dirtyRegion.X, dirtyRegion.Y, dirtyRegion.Width, dirtyRegion.Height,
                    PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

                bmp.UnlockBits(data);

                dirtyRegion = Rectangle.Empty;
            }
        }

        #endregion

        #region IDisposable Members

        private void Dispose(bool manual)
        {
            if (!disposed)
            {
                if (manual)
                {
                    bmp.Dispose();
                    gfx.Dispose();
                    if (GraphicsContext.CurrentContext != null)
                        GL.DeleteTexture(texture);
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DynamicTexture()
        {
            //Log.WriteLine(Log.LOG_WARNING, "[DynamicTexture] Resource leaked: " + typeof(DynamicTexture).ToString());
        }

        #endregion
    }
}
