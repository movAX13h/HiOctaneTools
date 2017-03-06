using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Materials
{
    class DefaultMaterial : Material
    {
        public DefaultMaterial()
        {
            shaderResource = EngineBase.Manager.GetShader("default");
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
