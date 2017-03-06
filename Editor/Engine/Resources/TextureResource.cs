using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Utils;
using LevelEditor.Engine;

using LevelEditor.Engine.Resources.Loaders;

namespace LevelEditor.Engine.Resources
{
    public class TextureResource : Resource
    {
        protected int id;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Bitmap Bitmap { get; private set; }

        public int Id
        {
            get { return id; }
        }

        public TextureResource(string name, Bitmap bmp = null) : base(name)
        {
            id = GL.GenTexture();

            if (bmp != null)
            {
                SetBitmap(bmp);
                return;
            }

            string filepath = Config.DATA_FOLDER + name;

            if (!File.Exists(filepath))
            {
                Log.WriteLine(Log.LOG_ERROR, "texture '" + name + "' not found");
                return;
            }

            SetBitmap(new Bitmap(filepath));
        }

        public void SetBitmap(Bitmap bmp)
        {
            Bitmap = bmp;

            Width = Bitmap.Width;
            Height = Bitmap.Height;

            BitmapData bmpData = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmpData.Width, bmpData.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmpData.Scan0);

            Bitmap.UnlockBits(bmpData);

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            ready = true;
            Log.WriteLine("uploaded texture '" + Name + "'");
        }

    }
}
