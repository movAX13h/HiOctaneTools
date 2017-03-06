using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Materials
{
    class SkySphereMaterial : Material
    {
        public float Time = 0;
        public SkySphereMaterial()
        {
            shaderResource = EngineBase.Manager.GetShader("skysphere");
            ready = shaderResource.Ready;
        }

        public override void Bind()
        {
            base.Bind();
            SetUniform("time", Time);
        }

        public override void Unbind()
        {
            base.Unbind();
        }

    }
}
