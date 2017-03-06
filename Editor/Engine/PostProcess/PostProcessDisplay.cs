using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.PostProcess
{
    class PostProcessDisplay : PostProcessEffect
    {
        public Vector3 Color;
        public float saturation = 3.7f;
        public float gamma = 2.2f;

        public PostProcessDisplay(float saturation, float gamma, Vector3 color) : base("display")
        {
            Color = color;
            this.saturation = saturation;
            this.gamma = gamma;
        }

        protected override void applyUniforms()
        {
            material.SetUniform("color", Color);
            material.SetUniform("saturation", saturation);
            material.SetUniform("gamma", gamma);
        }
    }
}
