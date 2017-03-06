using OpenTK;
using OpenTK.Graphics.OpenGL;

using LevelEditor.Engine.Core;

namespace LevelEditor.Engine.GUI
{
    public class ControlMaterial : Material
    {
        public ControlMaterial(string name)
        {
            shaderResource = EngineBase.Manager.GetShader("gui/default", "gui/" + name, null);
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
