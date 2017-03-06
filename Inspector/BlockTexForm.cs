using LevelInspector.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelInspector
{
    public partial class BlockTexForm : Form
    {
        public BlockTexForm(Atlas atlas, int n, int e, int s, int w, int t, int b)
        {
            InitializeComponent();

            northBox.Image = atlas.Get(n);
            eastBox.Image = atlas.Get(e);
            southBox.Image = atlas.Get(s);
            westBox.Image = atlas.Get(w);
            topBox.Image = atlas.Get(t);
            bottomBox.Image = atlas.Get(b);

            idsLabel.Text = "   " + n + Environment.NewLine + w + " " + t + " " + e + " " + b + Environment.NewLine + "   " + s;
        }

        public static void TopView(PictureBox canvas, Bitmap northBitmap, Bitmap eastBitmap, Bitmap southBitmap, Bitmap westBitmap, Bitmap topBitmap, Bitmap bottomBitmap)
        {
            // top view
            Bitmap canvasBitmap = new Bitmap(canvas.Width, canvas.Height);
            canvas.Image = canvasBitmap;

            Graphics gfx = Graphics.FromImage(canvasBitmap);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gfx.Clear(Color.Black);

            Point center = new Point(canvas.Width / 2, canvas.Height / 2);
            Point[] corners = new Point[3];

            corners[0] = center;
            corners[1] = new Point(center.X + 55, center.Y - 32);
            corners[2] = new Point(center.X, center.Y + 64);
            gfx.DrawImage(southBitmap, corners);

            corners[0] = new Point(center.X - 55, center.Y - 32);
            corners[1] = center;
            corners[2] = new Point(center.X - 55, center.Y + 32);
            gfx.DrawImage(westBitmap, corners);

            corners[0] = center;
            corners[1] = new Point(center.X - 55, center.Y - 32);
            corners[2] = new Point(center.X + 55, center.Y - 32);
            gfx.DrawImage(topBitmap, corners);

            gfx.Dispose();
            canvas.Refresh();
        }

        public static void BottomView(PictureBox canvas, Bitmap northBitmap, Bitmap eastBitmap, Bitmap southBitmap, Bitmap westBitmap, Bitmap topBitmap, Bitmap bottomBitmap)
        {
            // bottom view
            Bitmap canvasBitmap = new Bitmap(canvas.Width, canvas.Height);
            canvas.Image = canvasBitmap;

            Graphics gfx = Graphics.FromImage(canvasBitmap);

            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gfx.Clear(Color.Black);

            Point center = new Point(canvas.Width / 2, canvas.Height / 2);
            Point[] corners = new Point[3];

            corners[0] = new Point(center.X, center.Y - 64);
            corners[1] = new Point(center.X + 55, center.Y - 32);
            corners[2] = center;
            gfx.DrawImage(eastBitmap, corners);

            corners[0] = new Point(center.X - 55, center.Y - 32);
            corners[1] = new Point(center.X, center.Y - 64);
            corners[2] = new Point(center.X - 55, center.Y + 32);
            gfx.DrawImage(northBitmap, corners);

            corners[0] = center;
            corners[1] = new Point(center.X + 55, center.Y + 32);
            corners[2] = new Point(center.X - 55, center.Y + 32);
            gfx.DrawImage(bottomBitmap, corners);

            gfx.Dispose();
            canvas.Refresh();
        }


    }
}
