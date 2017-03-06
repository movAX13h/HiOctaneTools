using LevelEditor.Engine.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Engine.PostProcess
{
    class PostProcessEdgeDetect : PostProcessEffect
    {
        public float Threshold;
        public Vector4 Color;

        public PostProcessEdgeDetect(float threshold, Vector4 color) : base("edge")
        {
            Threshold = threshold;
            Color = color;
        }

        protected override void applyUniforms()
        {
            material.SetUniform("threshold", Threshold);
            material.SetUniform("color", Color);
        }
    }
}
