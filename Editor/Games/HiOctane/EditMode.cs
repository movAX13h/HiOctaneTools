using LevelEditor.Engine.Core;
using LevelEditor.Games.HiOctane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Games.HiOctane
{
    public abstract class EditMode
    {
        public string Name { get; private set; }
        public bool Enabled { get; private set; }

        public bool MouseUsed { get; protected set; }

        protected bool showWalls = true;
        protected bool showWaypoints = true;
        protected bool showBuildings = true;
        protected bool showEntities = true;
        protected bool showTerrain = true;
        protected bool morphsEnabled = true;

        protected bool fadeTerrain = false;
        protected bool fadeBuildings = false;

        protected Level level;

        public EditMode(string name)
        {
            Name = name;
            MouseUsed = false;
        }

        public void SetLevel(Level level)
        {
            this.level = level;
        }

        public void Enable()
        {
            if (Enabled) return;
            Enabled = true;

            level.ShowWallLines = showWalls;
            level.ShowWaypoints = showWaypoints;
            level.ShowBuildings = showBuildings;
            level.ShowEntities = showEntities;
            level.ShowTerrain = showTerrain;
            level.TerrainMorphsEnabled = morphsEnabled;


            enable();
        }

        public void Disable()
        {
            if (!Enabled) return;
            Enabled = false;
            disable();
        }

        protected virtual void enable() { }
        protected virtual void disable() { }

        public abstract void CreateControls(GUI gui);

        public virtual void Update(float time, float dTime) // called only when enabled
        {
            if (level.ShowTerrain)
            {
                level.Terrain.Grayscale += ((fadeTerrain ? 1f : 0f) - level.Terrain.Grayscale) * 2f * dTime;
                level.Terrain.TintColor.W = level.Terrain.Grayscale * 0.5f;
            }

            if (level.ShowBuildings)
            {
                foreach(SceneNode node in level.Buildings.Nodes)
                {
                    Column column = node as Column;
                    column.Grayscale += ((fadeBuildings ? 1f : 0f) - column.Grayscale) * 2f * dTime;
                }
            }
        }

        public virtual void Draw() { } // for custom rendering

        public abstract void Resize(GUI gui);

    }
}
