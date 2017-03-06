using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class WallsMode : EditMode
    {
        public WallsMode() : base("Walls")
        {
            showWalls = true;
            showWaypoints = false;
            showBuildings = true;
            showEntities = false;
            showTerrain = true;

            fadeBuildings = true;
            fadeTerrain = true;
        }

        public override void CreateControls(GUI gui)
        {

        }
        public override void Resize(GUI gui)
        {

        }

        protected override void enable()
        {
        }

        protected override void disable()
        {

        }
    }
}
