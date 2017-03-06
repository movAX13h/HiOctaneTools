using System;
using System.Collections.Generic;
using System.Text;

namespace LevelEditor.Games.HiOctane.Modes
{
    public class ViewMode : EditMode
    {
        public ViewMode() : base("Inspect")
        {
            showWalls = true;
            showWaypoints = true;
            showBuildings = true;
            showEntities = true;
            showTerrain = true;
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
