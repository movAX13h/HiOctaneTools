using LevelEditor.Games.HiOctane.Materials;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace LevelEditor.Games.HiOctane.Resources
{
    public class BlockDefinition : TableItem
    {
        public int N { get { return Bytes[0]; } }
        public int E { get { return Bytes[1]; } }
        public int S { get { return Bytes[2]; } }
        public int W { get { return Bytes[3]; } }
        public int T { get { return Bytes[4]; } }
        public int B { get { return Bytes[5]; } }

        public int NMod { get { return Bytes[6]; } }
        public int EMod { get { return Bytes[7]; } }
        public int SMod { get { return Bytes[8]; } }
        public int WMod { get { return Bytes[9]; } }
        public int TMod { get { return Bytes[10]; } }
        public int BMod { get { return Bytes[11]; } }



        public BlockDefinition(int id, int offset, byte[] bytes)
        {
            ID = id;
            Bytes = bytes;
            Offset = offset;
        }

        /*
        public Bitmap PreviewImage(AtlasMaterial atlas, Size size)
        {
            Bitmap result = new Bitmap(size.Width, size.Height);
            Size halfSize = new Size((int)Math.Floor((float)size.Width * 0.5f), size.Height);

            Graphics gfx = Graphics.FromImage(result);
            gfx.Clear(Color.Transparent);

            gfx.DrawImage(PreviewImageTop(atlas, halfSize), new Point(0, 0));
            gfx.DrawImage(PreviewImageBottom(atlas, halfSize), new Point(halfSize.Width, 0));

            gfx.Dispose();

            return result;
        }*/

        public Bitmap PreviewImageTop(AtlasMaterial atlas, Size size)
        {
            // top view
            Bitmap result = new Bitmap(size.Width, size.Height);

            Graphics gfx = Graphics.FromImage(result);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //gfx.Clear(Color.Transparent);

            Point center = new Point(size.Width / 2, size.Height / 2);
            Point[] corners = new Point[3];

            corners[0] = center;
            corners[1] = new Point(center.X + 55, center.Y - 32);
            corners[2] = new Point(center.X, center.Y + 64);
            gfx.DrawImage(atlas.Get(S, SMod), corners);

            corners[0] = new Point(center.X - 55, center.Y - 32);
            corners[1] = center;
            corners[2] = new Point(center.X - 55, center.Y + 32);
            gfx.DrawImage(atlas.Get(W, WMod), corners);

            corners[0] = center;
            corners[1] = new Point(center.X - 55, center.Y - 32);
            corners[2] = new Point(center.X + 55, center.Y - 32);
            gfx.DrawImage(atlas.Get(T, TMod), corners);

            gfx.Dispose();

            return result;
        }


        public Bitmap PreviewImageBottom(AtlasMaterial atlas, Size size)
        {
            // bottom view
            Bitmap result = new Bitmap(size.Width, size.Height);

            Graphics gfx = Graphics.FromImage(result);

            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //gfx.Clear(Color.Transparent);

            Point center = new Point(size.Width / 2, size.Height / 2);
            Point[] corners = new Point[3];

            corners[0] = new Point(center.X, center.Y - 64);
            corners[1] = new Point(center.X + 55, center.Y - 32);
            corners[2] = center;
            gfx.DrawImage(atlas.Get(E, EMod), corners);

            corners[0] = new Point(center.X - 55, center.Y - 32);
            corners[1] = new Point(center.X, center.Y - 64);
            corners[2] = new Point(center.X - 55, center.Y + 32);
            gfx.DrawImage(atlas.Get(N, NMod), corners);

            corners[0] = center;
            corners[1] = new Point(center.X + 55, center.Y + 32);
            corners[2] = new Point(center.X - 55, center.Y + 32);
            gfx.DrawImage(atlas.Get(B, BMod), corners);

            gfx.Dispose();

            return result;
        }

        public override bool WriteChanges()
        {
            return false;
        }
    }
}
