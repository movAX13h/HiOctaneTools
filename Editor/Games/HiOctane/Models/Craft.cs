using LevelEditor.Engine.Models;
using OpenTK;

namespace LevelEditor.Games.HiOctane.Models
{
    class Craft : WavefrontModel
    {
        public Craft(string filename, float x, float y, float z)
            : this(filename)
        {
            Position = new Vector3(x, y, z);
        }

        public Craft(string filename, Vector3 pos)
            : this(filename)
        {
            Position = pos;
        }

        public Craft(string filename)
            : base("Craft " + filename, "models/" + filename + ".obj", "models/atlas.png")
        {

        }
    }
}
