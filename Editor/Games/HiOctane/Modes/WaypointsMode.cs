using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class WaypointsMode : EditMode
    {
        public WaypointsMode() : base("Waypoints")
        {
            showWalls = false;
            showWaypoints = true;
            showBuildings = true;
            showEntities = false;
            showTerrain = true;
            morphsEnabled = false;

            fadeTerrain = true;
            fadeBuildings = true;
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
