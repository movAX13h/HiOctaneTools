using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Materials
{
    class PostProcessMaterial : Material
    {
        public PostProcessMaterial(string name)
        {
            shaderResource = EngineBase.Manager.GetShader(name);
            ready = shaderResource != null && shaderResource.Ready;
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
