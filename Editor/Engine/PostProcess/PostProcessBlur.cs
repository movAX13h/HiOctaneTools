using LevelEditor.Engine.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.PostProcess
{
    class PostProcessBlur : PostProcessEffect
    {
        public Vector2 Intensity;

        public PostProcessBlur(float x, float y) : base("blur")
        {
            Intensity = new Vector2(x, y);
        }

        protected override void applyUniforms()
        {
            material.SetUniform("intensity", Intensity);
        }
    }
}
