using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Materials
{
    class LightMaterial : Material
    {
        public LightMaterial()
        {
            shaderResource = EngineBase.Manager.GetShader("light");
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
