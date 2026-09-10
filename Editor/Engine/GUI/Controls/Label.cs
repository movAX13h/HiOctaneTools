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
        private float textureScale;

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
                    font = new Font(SystemFonts.DefaultFont.FontFamily, fontSize);
                    Log.WriteLine(Log.LOG_WARNING, "TextSprite font '" + fontName + "' not found. Using default font.");
                }
            }

            updateText();
        }

        private void ensureTexture()
        {
            if (texture != null && textureScale == Window.UiScale) return;
            if (texture != null) texture.Dispose();
            textureScale = Window.UiScale;
            // Grayscale coverage works on transparent textures; ClearType assumes
            // a known opaque background and otherwise leaves colored fringes.
            var hint = renderingHint == TextRenderingHint.ClearTypeGridFit
                ? TextRenderingHint.AntiAliasGridFit : renderingHint;
            texture = new DynamicTexture(Math.Max(1, Size.X),
                Math.Max(1, Size.Y), TextureUnit.Texture0, hint, textureScale);
        }

        private void updateText()
        {
            if (font == null) return;
            ensureTexture();

            texture.Clear(backgroundColor);
            if (shadow)
                using (var brush = new SolidBrush(Color.Black))
                    texture.DrawString(text, font, brush, new PointF(1,1));
            using (var brush = new SolidBrush(textColor))
                TextSize = texture.DrawString(text, font, brush, PointF.Empty);
        }

        public override void Resize(float w, float h)
        {
            if (Size.X == w && Size.Y == h) return;
            base.Resize(w, h);
            if (texture != null) texture.Dispose();
            texture = null;
            updateText();
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
            if (font != null) font.Dispose();
            if (privateFontCollection != null) privateFontCollection.Dispose();
            base.Unload();
        }

        protected override void ApplyUniforms()
        {
            if (textureScale != Window.UiScale) updateText();
            texture.Bind();
            material.SetUniform("texture", 0);
            // Map each raster pixel to exactly one screen pixel. Fractional DPI
            // otherwise squeezes the rounded-up bitmap and blurs it a second time.
            material.SetUniform("geometryScale", new Vector2(
                texture.PixelSize.Width / (Window.UiScale * Size.X),
                texture.PixelSize.Height / (Window.UiScale * Size.Y)));
        }

        protected override Vector2 RenderPosition
        {
            get
            {
                if (!SnapToPixel) return Pos;
                Vector2 offset = WorldPositionOffset();
                Vector2 position = (Pos + offset) * Window.UiScale;
                return new Vector2((float)Math.Round(position.X), (float)Math.Round(position.Y)) / Window.UiScale - offset;
            }
        }
    }
}
