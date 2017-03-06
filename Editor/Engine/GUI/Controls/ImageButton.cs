using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using LevelEditor.Engine.Resources;
using OpenTK.Graphics.OpenGL4;

namespace LevelEditor.Engine.GUI.Controls
{
    public class ImageButton : Control
    {
        public Color Tint = Color.White;
        public TextureResource Icon { get; private set; }
        private Action<ImageButton> callbackDown;
        private Action<ImageButton> callbackUp;

        public bool Down = false;
        //public string ImageFilename { get; private set; }
        public object Tag;

        public ImageButton(Vector2 size, TextureResource tex, Action<ImageButton> cbDown = null, Action<ImageButton> cbUp = null) : base("imagebutton", size)
        {
            Icon = tex;
            MouseEnabled = true;
            callbackDown = cbDown;
            callbackUp = cbUp;
        }

        public ImageButton(Vector2 size, string image, Action<ImageButton> cbDown = null, Action<ImageButton> cbUp = null) : base("imagebutton", size)
        {
            //ImageFilename = image;
            Icon = EngineBase.Manager.GetTexture(image);
            MouseEnabled = true;
            callbackDown = cbDown;
            callbackUp = cbUp;
        }

        public override void Update(float time, float dtime)
        {
            base.Update(time, dtime);
        }

        protected override void OnMouseDown()
        {
            MouseConsumed = true;
            if (callbackDown != null) callbackDown(this);
        }

        protected override void OnMouseUp()
        {
            MouseConsumed = true;
            if (callbackUp != null) callbackUp(this);
        }


        protected override void ApplyUniforms()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, Icon.Id);
            material.SetUniform("icon", 0);
            material.SetUniform("down", MouseDown || Down);
            material.SetUniform("tint", Tint);
        }

    }
}
