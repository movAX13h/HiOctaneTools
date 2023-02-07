using LevelEditor.Games.HiOctane.Models;
using LevelEditor.Games.HiOctane.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Games.HiOctane
{
    public class Morph
    {
        public bool Enabled = true;

        public EntityItem Source;
        public EntityItem Target;

        public float LastProgress { get; private set; }
        private float progress = 0f;

        public bool Permanent = false;
        public bool UVsFromSource = false;

        public List<Column> Columns;

        public float Progress
        {
            get
            {
                return progress;
            }
            set
            {
                LastProgress = progress;
                progress = value;
            }
        }

        public int Width; // width and height from 2D top-view
        public int Height;

        public Morph(EntityItem source, EntityItem target, int width, int height, bool permanent)
        {
            Source = source;
            Target = target;
            Width = width;
            Height = height;
            Permanent = permanent;
            LastProgress = 0f;
            Columns = new List<Column>();
        }

        public void MorphColumns()
        {
            foreach (Column column in Columns) column.Morph(Progress);
        }
    }
}
