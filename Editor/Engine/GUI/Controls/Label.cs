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
    public class Label : Control
    {
        private DynamicTexture texture;
        private PrivateFontCollection privateFontCollection;
        private Font font;

        private float fontSize = 10.0f;
        private string fontName = "";

        private bool shadow = false;
        public bool Shadow
        {
            set
            {
                shadow = value;
                updateText();
            }

            get
            {
                return shadow;
            }
        }
        public Color TextColor
        {
            set
            {
                textColor = value;
                updateText();
            }
            get
            {
                return textColor;
            }
        }
        private Color textColor;

        public Color BackgroundColor
        {
            set
            {
                backgroundColor = value;
                updateText();
            }
            get
            {
                return backgroundColor;
            }
        }
        private Color backgroundColor;

        public string Text
        {
            set
            {
                if (text != value)
                {
                    text = value;
                    updateText();
                }
            }
            get
            {
                return text;
            }
        }
        private string text = "";

        public SizeF TextSize { get; private set; }
        private TextRenderingHint renderingHint;

        public Label(Vector2 size, float fontSize, TextRenderingHint renderingHint = TextRenderingHint.ClearTypeGridFit, string fontName = "")
            : base("texture", size)
        {
            SnapToPixel = true;
            textColor = Color.White;
            backgroundColor = Color.Transparent;

            if (fontName == "") fontName = Config.FONT_DEFAULT;

            this.renderingHint = renderingHint;
            this.fontSize = fontSize;
            this.fontName = fontName;

            setup();
        }

        private void setup()
        {
            if (isFontInstalled(fontName))
            {
                font = new Font(fontName, fontSize);
            }
            else
            {
                if (File.Exists(fontName))
                {
                    privateFontCollection = new PrivateFontCollection();
                    privateFontCollection.AddFontFile(fontName);
                    font = new Font(privateFontCollection.Families[0], fontSize);
                }
                else
                {
                    font = SystemFonts.DefaultFont;
                    Log.WriteLine(Log.LOG_WARNING, "TextSprite font '" + fontName + "' not found. Using default font.");
                }
            }

            texture = new DynamicTexture((int)Size.X, (int)Size.Y, TextureUnit.Texture0, renderingHint);
            updateText();
        }

        private void updateText()
        {
            if (texture == null) return;

            texture.Clear(backgroundColor);
            if (shadow)
                texture.DrawString(text, font, new SolidBrush(Color.Black), new PointF(1,1));
            TextSize = texture.DrawString(text, font, new SolidBrush(textColor), PointF.Empty);
        }

        private bool isFontInstalled(string fontName)
        {
            using (var testFont = new Font(fontName, 8))
            {
                return 0 == string.Compare(
                  fontName,
                  testFont.Name,
                  StringComparison.InvariantCultureIgnoreCase);
            }
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
