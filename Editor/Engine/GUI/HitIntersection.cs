using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace LevelEditor.Engine.GUI
{
    public class HitIntersection
    {
        public Vector2 Position { private set; get; }
        public List<Sprite> Sprites { private set; get; }

        public HitIntersection(List<Sprite> hits, Vector2 position)
        {
            Sprites = hits;
            Position = position;
        }
    }
}
