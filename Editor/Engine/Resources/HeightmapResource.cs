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
    class HeightmapResource : Resource
    {
        public float[,] Heights { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        protected int id;

        public int Id
        {
            get { return id; }
        }

        public HeightmapResource(string name)
            : base(name)
        {
            string filepath = Config.DATA_FOLDER + name;
            id = 0;

            if (!File.Exists(filepath))
            {
                Log.WriteLine(Log.LOG_ERROR, "heightmap file '" + name + "' not found");
                return;
            }

            Heights = load(filepath);

            Log.WriteLine("heightmap ready '" + name + "'");
            ready = true;
        }

        protected virtual float[,] load(string filename)
        {
            Bitmap bmp = new Bitmap(filename);
            Width = bmp.Width;
            Height = bmp.Height;

            float[,] heights = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    heights[x, y] = bmp.GetPixel(x, y).R; // red channel only for now
                }
            }

            return heights;
        }
    }
}
