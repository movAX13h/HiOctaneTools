using System;
using LevelEditor.Engine.Models;

namespace LevelEditor.Games.HiOctane.Models
{
    class Collectable : Billboard
    {
        public Collectable(int number) : base("images/tmaps/" + number + ".png", 0.45f, 0.45f)
        {

        }
    }
}
