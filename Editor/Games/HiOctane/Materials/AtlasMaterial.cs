using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;
using LevelEditor.Engine.Resources;
using System.Collections.Generic;
using LevelEditor.Engine;
using System;
using System.Drawing;

namespace LevelEditor.Games.HiOctane.Materials
{
    // Hi-Octane specific atlas material and helper

    public class AtlasMaterial : Material
    {
        public static List<Vector2> ApplyTexMod(Vector2 uvA, Vector2 uvB, Vector2 uvC, Vector2 uvD, int mod)
        {
            List<Vector2> uvs = new List<Vector2>();
            switch (mod)
            {
                case 1: //RotateNoneFlipX
                    uvs.Add(uvB);
                    uvs.Add(uvA);
                    uvs.Add(uvD);
                    uvs.Add(uvC);
                    break;
                case 2: //RotateNoneFlipY
                    uvs.Add(uvD);
                    uvs.Add(uvC);
                    uvs.Add(uvB);
                    uvs.Add(uvA);
                    break;
                case 3: //Rotate180FlipNone
                    uvs.Add(uvC);
                    uvs.Add(uvD);
                    uvs.Add(uvA);
                    uvs.Add(uvB);
                    break;
                case 4: //Rotate270FlipY
                    uvs.Add(uvA);
                    uvs.Add(uvD);
                    uvs.Add(uvC);
                    uvs.Add(uvB);
                    break;
                case 5: //Rotate90FlipNone
                    uvs.Add(uvD);
                    uvs.Add(uvA);
                    uvs.Add(uvB);
                    uvs.Add(uvC);
                    break;
                case 6: //Rotate270FlipNone
                    uvs.Add(uvB);
                    uvs.Add(uvC);
                    uvs.Add(uvD);
                    uvs.Add(uvA);
                    break;
                case 7: //Rotate90FlipY
                    uvs.Add(uvC);
                    uvs.Add(uvB);
                    uvs.Add(uvA);
                    uvs.Add(uvD);
                    break;
                case 0:
                default:
                    uvs.Add(uvA);
                    uvs.Add(uvB);
                    uvs.Add(uvC);
                    uvs.Add(uvD);
                    break;
            }
            return uvs;
        }


        private TextureResource textureResource;
        public int TextureHandle { get { return textureResource.Id; } }

        public int Width { get { return textureResource.Width; } }
        public int Height { get { return textureResource.Height; } }

        public int NumTexturesX { get; private set; }
        public int NumTexturesY { get; private set; }

        public bool UseTexture { get; private set; }
        public bool ReceiveShadows { get; private set; }

        private Dictionary<string, Bitmap> cache;

        public AtlasMaterial(string textureFilename, bool receiveShadows)
        {
            ready = true;
            UseTexture = false;
            ReceiveShadows = receiveShadows;

            if (textureFilename != "")
            {
                textureResource = EngineBase.Manager.GetTexture(textureFilename);
                if (textureResource.Ready)
                {
                    UseTexture = true;
                    NumTexturesX = (int)Math.Floor(textureResource.Width / 64f);
                    NumTexturesY = (int)Math.Floor(textureResource.Height / 64f);
                }
                else
                {
                    ready = false;
                    return;
                }
            }

            // defines that need to be enabled in the shader
            List<string> defines = new List<string>();
            if (UseTexture) defines.Add("USE_COLOR_MAP");
            if (receiveShadows) defines.Add("USE_SHADOW_MAP");
            defines.Add("USE_VERTEX_NORMALS");

            shaderResource = EngineBase.Manager.GetShader("texture", defines);
            if (!shaderResource.Ready) ready = false;

            cache = new Dictionary<string, Bitmap>();
        }

        public List<Vector2> MakeUVs(int textureId, int texMod)
        {
            int tX = textureId % NumTexturesX;
            int tY = textureId / NumTexturesX;
            float sX = 1f / (float)NumTexturesX;
            float sY = 1f / (float)NumTexturesY;

            float uvX = tX * sX + 0.0006f; // UVs inset to avoid flickering edges of atlas textures
            float uvY = tY * sY + 0.0006f;
            sX -= 0.0012f;
            sY -= 0.0012f;

            Vector2 uvA = new Vector2(uvX, uvY);
            Vector2 uvB = new Vector2(uvX + sX, uvY);
            Vector2 uvC = new Vector2(uvX + sX, uvY + sY);
            Vector2 uvD = new Vector2(uvX, uvY + sY);

            return ApplyTexMod(uvA, uvB, uvC, uvD, texMod);
        }

        public Bitmap Get(int id, int mod = 0)
        {
            string key = id + "/" + mod;
            if (cache.ContainsKey(key)) return cache[key];

            int numX = textureResource.Bitmap.Width / 64;
            int y = id / numX;
            int x = id - y * numX;

            Bitmap bmp = new Bitmap(64, 64);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(textureResource.Bitmap, 0, 0, new Rectangle(64 * x, 64 * y, 64, 64), GraphicsUnit.Pixel);

            //FontFamily fontFamily = new FontFamily("Arial");
            //Font font = new Font(fontFamily, 40, FontStyle.Regular, GraphicsUnit.Pixel);
            //g.DrawString(mod.ToString(), font, Brushes.White, 4, 4);

            g.Dispose();

            /*
            0000 0 none
            0001 1 mirror
            0010 2 flip
            0011 3 mirror, flip
            0100 4 mirror, 90° CCW
            0101 5 90° CW
            0110 6 90° CCW
            0111 7 mirror, 90° CW
            */

            switch (mod)
            {
                case 1:
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 2:
                    bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    break;
                case 3:
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 4:
                    bmp.RotateFlip(RotateFlipType.Rotate270FlipY);
                    break;
                case 5:
                    bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 6:
                    bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case 7:
                    bmp.RotateFlip(RotateFlipType.Rotate90FlipY);
                    break;
                case 0:
                default:
                    break;
            }

            cache.Add(key, bmp);
            return bmp;
        }

        /*
        public override void Bind()
        {
            base.Bind();
        }

        public override void UnBind()
        {
            base.UnBind();
        }
        */
    }
}
