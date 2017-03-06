using LevelEditor.Engine.Models;
using OpenTK;

namespace LevelEditor.Games.HiOctane.Models
{
    class Cone : WavefrontModel
    {
        public Cone(float x, float y, float z) : this()
        {
            Position = new Vector3(x, y, z);
        }

        public Cone(Vector3 pos) : this()
        {
            Position = pos;
        }

        public Cone() : base("Cone", "models/CONE0-0.obj", "models/atlas.png")
        {

        }
    }
}
