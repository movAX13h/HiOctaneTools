using System;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Materials
{
    class FontMaterial : Material
    {
        public FontMaterial()
        {
            shaderResource = EngineBase.Manager.GetShader("font");
            ready = shaderResource.Ready;
        }

        public override void Bind()
        {
            base.Bind();
        }

        public override void Unbind()
        {
            base.Unbind();
        }
    }
}
