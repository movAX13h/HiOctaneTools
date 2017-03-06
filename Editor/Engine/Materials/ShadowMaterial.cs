using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Materials
{
    class ShadowMaterial : Material
    {
        public ShadowMaterial()
        {
            shaderResource = EngineBase.Manager.GetShader("shadow");
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
