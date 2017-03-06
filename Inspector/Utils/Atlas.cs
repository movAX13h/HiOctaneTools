using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelInspector.Utils
{
    public class Atlas
    {
        private string filename;
        private Image image;

        private Dictionary<string, Bitmap> cache;

        public Atlas(string file)
        {
            filename = file;
            image = Bitmap.FromFile(file);
            cache = new Dictionary<string, Bitmap>();
        }

        public Bitmap Get(int id, int mod = 0)
        {
            string key = id + "/" + mod;
            if (cache.ContainsKey(key)) return cache[key];

            int numX = image.Width / 64;
            int y = id / numX;
            int x = id - y * numX;

            Bitmap bmp = new Bitmap(64, 64);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(image, 0, 0, new Rectangle(64 * x, 64 * y, 64, 64), GraphicsUnit.Pixel);

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

            switch(mod)
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
    }
}
