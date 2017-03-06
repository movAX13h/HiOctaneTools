using LevelEditor.Games.HiOctane.Resources;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Games.HiOctane
{
    public class RayCastResult
    {
        public bool Hit { get; private set; }
        public Vector3 Position { get; private set; }
        public Vector3 PositionSnapped { get; private set; }
        public Vector3 PositionOfEntry { get; private set; }
        public MapEntry PositionEntry { get; private set; }
        public MapEntry NearestEntry { get; private set; }

        public RayCastResult()
        {
            Hit = false;
            Position = Vector3.Zero;
        }

        public RayCastResult(Vector3 pos, Vector3 entryPos, Vector3 posSnapped, MapEntry positionEntry, MapEntry nearestEntry)
        {
            Hit = true;
            Position = pos;
            PositionOfEntry = entryPos;
            PositionSnapped = posSnapped;
            PositionEntry = positionEntry;
            NearestEntry = nearestEntry;
        }
    }

    public class LevelCollision
    {
        private Level level;
        private LevelFile levelData;

        public LevelCollision(Level level, LevelFile levelData)
        {
            this.level = level;
            this.levelData = levelData;
        }

        public RayCastResult RayCast(Vector3 start, Vector3 dir, int steps = 400, float stepSize = 0.5f)
        {
            if (start.Y != ClippedHeight(start))
                return new RayCastResult(); // start is below/inside terrain

            if (start.Y > level.Terrain.Size.Y) // start is above terrain
            {
                float o = (level.Terrain.Size.Y - start.Y) / dir.Y; // steps to reach terrain top
                if (o < 0) return new RayCastResult(); // pointing away from terrain (up; could also use dir.Y)
                start += o * dir; // shift start to reach terrain max height
            }

            // march ray
            Vector3 rayPos = start + 0.2f * dir;

            for (int i = 0; i < steps; i++)
            {
                if (rayPos.Y < ClippedHeight(rayPos))
                {
                    // march backwards with smaller step size to get closer to intersection
                    int remainingSteps = Math.Min(100, Math.Max(10, steps - i));
                    float reverseStepSize = stepSize / remainingSteps;

                    for(int j = 0; j < remainingSteps; j++)
                    {
                        rayPos -= reverseStepSize * dir;
                        if (rayPos.Y >= ClippedHeight(rayPos)) break;
                    }

                    int x = (int)Math.Floor(rayPos.X);
                    int z = (int)Math.Floor(rayPos.Z);
                    MapEntry posEntry = level.Terrain.GetMapEntry(x, z);
                    Vector3 entryPos = new Vector3(x, posEntry.Height, z);

                    x = (int)Math.Round(rayPos.X);
                    z = (int)Math.Round(rayPos.Z);
                    MapEntry nearEntry = level.Terrain.GetMapEntry(x, z);

                    Vector3 snappedPos = new Vector3(x, level.Terrain.GetHeightInterpolated(x, z), z);
                    return new RayCastResult(rayPos, entryPos, snappedPos, posEntry, nearEntry);
                }
                rayPos += stepSize * dir;
            }

            return new RayCastResult();
        }

        public float ClippedHeight(Vector3 pos)
        {
            if (pos.X < 0 || pos.X > levelData.Width || pos.Z < 0 || pos.Z > levelData.Height) return pos.Y;
            float h = level.Terrain.GetHeightInterpolated(pos.X, pos.Z);
            if (pos.Y < h) return h;
            return pos.Y;
        }

        public bool BoxIntersection(Vector3 pos, Vector3 size)
        {
            int numX = (int)Math.Ceiling(size.X);
            int numZ = (int)Math.Ceiling(size.Z);
            Vector3 columnPos = Vector3.Zero;

            float dx = Math.Min(1.0f, size.X);
            float dz = Math.Min(1.0f, size.Z);

            for (float x = 0; x <= size.X; x+=dx)
            {
                for (float z = 0; z <= size.Z; z+=dz)
                {
                    // check terrain height
                    float terrainHeight = level.Terrain.GetHeightInterpolated(pos.X + x, pos.Z + z);
                    if (pos.Y < terrainHeight) return true; // is below terrain

                    columnPos.X = (float)Math.Floor(pos.X + x);
                    columnPos.Z = (float)Math.Floor(pos.Z + z);

                    // check column
                    if (levelData.Columns.ContainsKey(columnPos)) continue;

                    ColumnDefinition colDef = levelData.Columns[columnPos];

                    int bitStart = (int)Math.Ceiling(pos.Y - terrainHeight);
                    if (bitStart < 8)
                    {
                        int bitEnd = (int)Math.Min(8f, Math.Max(bitStart + 1f, Math.Ceiling(pos.Y + size.Y - terrainHeight)));

                        // all blocks of this column
                        for (int bitNum = bitStart; bitNum < bitEnd; bitNum++)
                        {
                            //TODO: make this work properly
                            //if ((colDef.Shape & (1 << bitNum)) != 0) return true; // block in the way
                        }
                    }
                }
            }

            return false;
        }

    }
}
