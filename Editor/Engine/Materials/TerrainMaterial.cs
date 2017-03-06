using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.Materials
{
    class TerrainMaterial : Material
    {
        public float Far = 300f;

        public TerrainMaterial()
        {
            shaderResource = EngineBase.Manager.GetShader("terrain");
            ready = shaderResource.Ready;
        }

        public override void Bind()
        {
            base.Bind();
            SetUniform("far", Far);

        }

        public override void Unbind()
        {
            base.Unbind();
        }

    }
}
