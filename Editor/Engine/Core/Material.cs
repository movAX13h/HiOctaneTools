using System.Text;

namespace LevelEditor.Engine.Core
{
    public class Material : Shader
    {
        protected bool ready = false;
        public bool Ready { get { return ready; } }

        public Material()
        {
        }
    }
}
