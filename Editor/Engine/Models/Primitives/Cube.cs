using LevelEditor.Engine.Core;
using LevelEditor.Engine.Materials;

using OpenTK;
using LevelEditor.Utils;

namespace LevelEditor.Engine.Models.Primitives
{
    class Cube : Primitive
    {
        public Cube():this(-1, -1, -1, 1, 1, 1, new Vector3(0.6f, 0.6f, 0.6f))
        {

        }

        public Cube(float negX, float negY, float negZ, float posX, float posY, float posZ, Vector3 color):base(color)
        {
            Name = "Cube";

            positionVboData = new Vector3[] {
            new Vector3(negX, negY, posZ),
            new Vector3(posX, negY, posZ),
            new Vector3(posX, posY, posZ),
            new Vector3(negX, posY, posZ),
            new Vector3(negX, negY, negZ),
            new Vector3(posX, negY, negZ),
            new Vector3(posX, posY, negZ),
            new Vector3(negX, posY, negZ) };

            indicesVboData = new int[] {
                // front face
                0, 1, 2, 2, 3, 0,

                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                0, 1, 5, 5, 4, 0,
                // right face
                1, 5, 6, 6, 2, 1 };


            material = new DefaultMaterial();

            createVBOs();
            createVAOs();

            ready = true;
        }
    }
}
