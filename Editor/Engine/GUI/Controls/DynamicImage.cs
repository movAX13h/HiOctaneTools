using LevelEditor.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Text;

namespace LevelEditor.Engine.GUI.Controls
{
    public class DynamicImage : Control
    {
        private DynamicTexture texture;

        public DynamicImage(Vector2 size) : base("texture", size)
        {
            SnapToPixel = true;
            texture = new DynamicTexture((int)Size.X, (int)Size.Y, TextureUnit.Texture0);
        }

        public void Clear(Color color)
        {
            texture.Clear(color);
        }

        /*
        public void DrawRectangle(int x, int y, int w, int h, Brush brush)
        {
            texture.DrawRectangle(x, (int)Size.Y - h, w, h, brush);
        }
        */

        public void DrawLine(int x1, int y1, int x2, int y2, Pen pen)
        {
            texture.DrawLine(x1, (int)Size.Y - y1, x2, (int)Size.Y - y2, pen);
        }

        public override void Update(float time, float dtime)
        {
            base.Update(time, dtime);

        }

        public override void Unload()
        {
            if (texture != null) texture.Dispose();
            base.Unload();
        }

        protected override void ApplyUniforms()
        {
            texture.Bind();
            material.SetUniform("texture", 0);
        }
    }
}
