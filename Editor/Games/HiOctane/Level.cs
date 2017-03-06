using LevelEditor.Engine;
using LevelEditor.Engine.Core;
using LevelEditor.Engine.Lights;
using LevelEditor.Engine.Materials;
using LevelEditor.Engine.Models;
using LevelEditor.Engine.Models.Primitives;
using LevelEditor.Games.HiOctane.Materials;
using LevelEditor.Games.HiOctane.Models;
using LevelEditor.Games.HiOctane.Resources;
using LevelEditor.Utils;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LevelEditor.Games.HiOctane
{
    public class Level : RootNode
    {
        public CameraController Camera { get; private set; }
        public LevelTerrain Terrain { get; private set; }
        public LevelCollision Collisions { get; private set; } // this is a helper for world collisions/raycasts
        public LevelFile Data { get; private set; }
        public Dictionary<int, List<EntityItem>> GroupedEntities { get; private set; }
        public List<Morph> Morphs { get; private set; }
        public SceneNode WaypointLines { get; private set; }
        public SceneNode WallLines { get; private set; }
        public SceneNode Buildings { get; private set; }

        private Dictionary<int, Column> columnsByPosition;

        public SceneNode Entities { get; private set; }

        #region public enablers/disablers
        public bool ShowWallLines
        {
            get
            {
                return WallLines.Parent != null;
            }

            set
            {
                if (value) AddNode(WallLines);
                else RemoveNode(WallLines);
            }
        }

        public bool ShowWaypoints
        {
            get
            {
                return WaypointLines.Parent != null;
            }

            set
            {
                if (value) AddNode(WaypointLines);
                else RemoveNode(WaypointLines);
            }
        }

        public bool ShowBuildings
        {
            get
            {
                return Buildings.Parent != null;
            }

            set
            {
                if (value) AddNode(Buildings);
                else RemoveNode(Buildings);
            }
        }

        public bool ShowEntities
        {
            get
            {
                return Entities.Parent != null;
            }

            set
            {
                if (value) AddNode(Entities);
                else RemoveNode(Entities);
            }
        }

        public bool ShowTerrain
        {
            get
            {
                return Terrain.Parent != null;
            }

            set
            {
                if (value) AddNode(Terrain);
                else RemoveNode(Terrain);
            }
        }

        private bool morphsEnabled = true;
        public bool TerrainMorphsEnabled
        {
            get
            {
                return morphsEnabled;
            }

            set
            {
                morphsEnabled = value;
                if (!morphsEnabled) disableMorphs();
            }
        }

        #endregion

        private DirectionalLight light;
        private SkySphere skySphere;
        public AtlasMaterial Atlas;

        private int levelNumber;

        public Level(int levelNumber) : base()
        {
            this.levelNumber = Math.Max(1, Math.Min(9, levelNumber));
        }

        protected override bool setup()
        {
            #region setup world
            // scene light
            Vector3 lightDir = new Vector3(1f, -1f, 1f);
            light = new DirectionalLight(lightDir.Normalized(), new Vector3(1, 1, 1));
            AddLight(light);

            // shadow material (unused)
            //ShadowMaterial = new ShadowMaterial();
            //if (!ShadowMaterial.Ready) return false;

            // create skysphere
            skySphere = new SkySphere(2000f, 2);
            if (!skySphere.Ready) return false;
            AddNode(skySphere);

            // load level file
            Data = new LevelFile(Config.DATA_FOLDER + "maps/level0-" + levelNumber + ".dat");
            if (!Data.Ready) return false;

            Name = Data.Name;

            // load level atlas texture file
            int atlasNumber = levelNumber;
            if (levelNumber == 7) atlasNumber = 1; // original game has this hardcoded too

            Atlas = new AtlasMaterial("images/level0-" + atlasNumber + ".png", false);
            if (!Atlas.Ready) return false;

            // create terrain
            Terrain = new LevelTerrain("Terrain Level " + levelNumber, Data, Atlas);
            if (!Terrain.Ready) return false;
            AddNode(Terrain);

            // player (camera controller really)
            Camera = new CameraController(new Vector3(6f, Terrain.Size.Y + 6f, 6f), new Vector3(10f, Terrain.Size.Y + 5f, 10f));

            // create building node and add columns
            Buildings = new SceneNode();
            AddNode(Buildings);

            columnsByPosition = new Dictionary<int, Column>();

            foreach(var columnPosPair in Data.Columns)
                addColumn(columnPosPair.Value, columnPosPair.Key);

            // create entities and link columns with their morph source
            createEntities();
            #endregion

            #region temporary
            /*
            mouseLine = new Line(Vector3.Zero, Vector3.UnitZ, new Vector4(1, 1, 1, 1));
            AddNode(mouseLine);

            */

            /*
            Craft craft = new Craft("JET0-0", box.Position);
            AddNode(craft);
            */

            // level bounds rectangle
            /*
            Line line;
            line = new Line(new Vector3(0f, Terrain.Size.Y, 0f), new Vector3(256f, Terrain.Size.Y, 0f), new Vector4(1.0f, 1.0f, 0.0f, 1.0f));
            if (!line.Ready) return false;
            line.Extend(new Vector3(256f, Terrain.Size.Y, 160f)).Extend(new Vector3(0f, Terrain.Size.Y, 160f)).Extend(line.A);
            AddNode(line);
            */
            #endregion

            #region helpers
            // X / Y / Z
            AddNode(new Line(new Vector3(0f, 0f, 0f), new Vector3(20f, 0f, 0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)));
            AddNode(new Line(new Vector3(0f, 0f, 0f), new Vector3(0f, 20f, 0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f)));
            AddNode(new Line(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 20f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)));

            // setup collision helper for superfast Hi-Octane world box collisions
            Collisions = new LevelCollision(this, Data);
            #endregion
            return true;
        }

        internal void FocusMapEntry(MapEntry mapEntry)
        {
            Camera.AnimateTo(new Vector3(mapEntry.X, mapEntry.Height + 3f, mapEntry.Z), 7.0f);
        }

        private void addColumn(ColumnDefinition definition, Vector3 pos)
        {
            Column column = new Column(definition, pos, Data, Atlas);
            columnsByPosition.Add((int)pos.X + (int)pos.Z * Data.Width, column);
            Buildings.AddNode(column);
        }

        private void createEntities()
        {
            Entities = new SceneNode();
            AddNode(Entities);

            GroupedEntities = new Dictionary<int, List<EntityItem>>();
            Morphs = new List<Morph>();

            WaypointLines = new SceneNode();
            WaypointLines.Name = "Waypoints";
            AddNode(WaypointLines);

            WallLines = new SceneNode();
            WallLines.Name = "Wall Segments";
            AddNode(WallLines);

            foreach (var idEntityPair in Data.Entities)
            {
                EntityItem entity = idEntityPair.Value;
                createEntity(entity);
            }
        }

        private void createEntity(EntityItem entity)
        {
            Line line;
            float w, h;
            Collectable collectable;
            Box box;

            EntityItem next = null;

            if (!GroupedEntities.ContainsKey(entity.Group)) GroupedEntities.Add(entity.Group, new List<EntityItem>());
            GroupedEntities[entity.Group].Add(entity);

            Vector4 color = new Vector4(1.0f, 1.0f, 0.0f, 0.2f);
            float boxSize = 0;
            collectable = null;

            if (entity.NextID != 0 && Data.Entities.ContainsKey(entity.NextID))
            {
                next = Data.Entities.GetValue(entity.NextID);
            }

            switch (entity.GameType)
            {
                case EntityItem.EntityType.WaypointAmmo:
                case EntityItem.EntityType.WaypointFuel:
                case EntityItem.EntityType.WaypointShield:
                case EntityItem.EntityType.WaypointShortcut:
                case EntityItem.EntityType.WaypointSpecial1:
                case EntityItem.EntityType.WaypointSpecial2:
                case EntityItem.EntityType.WaypointSpecial3:
                case EntityItem.EntityType.WaypointFast:
                case EntityItem.EntityType.WaypointSlow:
                    color = new Vector4(0.1f, 0.1f, 1.0f, 1.0f);
                    //boxSize = 0.04f;
                    if (next != null)
                    {
                        line = new Line(entity.Center, next.Center, color);
                        line.Name = "Waypoint line " + entity.ID + " to " + next.ID;
                        WaypointLines.AddNode(line);
                    }
                    break;
                case EntityItem.EntityType.WallSegment:
                    color = new Vector4(1.0f, 0.0f, 0.0f, 1.0f);
                    if (next != null)
                    {
                        line = new Line(entity.Center + Vector3.UnitY, next.Center + Vector3.UnitY, color);
                        line.Name = "Wall segment line " + entity.ID + " to " + next.ID;
                        WallLines.AddNode(line);
                    }
                    break;
                case EntityItem.EntityType.TriggerCraft:
                case EntityItem.EntityType.TriggerRocket:
                    w = entity.OffsetX + 1f;
                    h = entity.OffsetY + 1f;
                    box = new Box(0, 0, 0, w, 2, h, new Vector4(0.9f, 0.3f, 0.6f, 0.5f));
                    box.Position = entity.Pos + Vector3.UnitY * 0.01f;
                    Entities.AddNode(box);
                    break;
                case EntityItem.EntityType.TriggerTimed:
                    Billboard timer = new Billboard("images/stopwatch.png", 0.4f, 0.4f);
                    timer.Position = entity.Center;
                    Entities.AddNode(timer);
                    break;


                case EntityItem.EntityType.MorphOnce:
                case EntityItem.EntityType.MorphPermanent:
                    w = entity.OffsetX + 1f;
                    h = entity.OffsetY + 1f;
                    //box = new Box(0, 0, 0, w, 1, h, new Vector4(0.1f, 0.3f, 0.9f, 0.5f));
                    //box.Position = entity.Pos + Vector3.UnitY * 0.01f;
                    //AddNode(box);

                    EntityItem source = Data.Entities.GetValue(entity.NextID);

                    // morph for this entity and its linked source
                    List<Column> targetColumns = ColumnsInRange((int)entity.Pos.X, (int)entity.Pos.Z, w, h);
                    List<Column> sourceColumns = ColumnsInRange((int)source.Pos.X, (int)source.Pos.Z, w, h);

                    // regular morph
                    if (targetColumns.Count == sourceColumns.Count)
                    {
                        for (int i = 0; i < targetColumns.Count; i++)
                        {
                            targetColumns[i].MorphSource = sourceColumns[i];
                            sourceColumns[i].MorphSource = targetColumns[i];
                        }
                    }
                    else
                    {
                        // permanent morphs dont destroy buildings, instead they morph the column based on terrain height
                        if (entity.GameType == EntityItem.EntityType.MorphPermanent)
                        {

                            // we need to update surrounding columns too because they could be affected (one side of them)
                            // (problem comes from not using terrain height for all columns in realtime)
                            targetColumns = ColumnsInRange((int)entity.Pos.X - 1, (int)entity.Pos.Z - 1, w + 1, h + 1);

                            // create dummy morph source columns at source position
                            foreach(Column column in targetColumns)
                            {
                                Vector3 colPos = new Vector3(source.Pos.X + (column.Position.X - entity.Pos.X), 0, source.Pos.Z + (column.Position.Z - entity.Pos.Z));
                                column.MorphSource = new Column(column.Definition, colPos, Data, Atlas);
                            }

                            sourceColumns.Clear();
                        }
                        else
                        {
                            // in this case (MorphOnce) there are no target columns and
                            // (target and source areas are swapped from game perspective)
                            // and buildings have to be destroyed as soon as the morph starts
                            foreach (Column column in sourceColumns) column.DestroyOnMorph = true;
                            foreach (Column column in targetColumns) column.DestroyOnMorph = true;
                        }
                    }

                    // create and collect morph instances
                    Morph morph = new Morph(source, entity, (int)w, (int)h, entity.GameType == EntityItem.EntityType.MorphPermanent);
                    morph.Columns.AddRange(targetColumns);
                    Morphs.Add(morph);

                    // source
                    morph = new Morph(entity, source, (int)w, (int)h, entity.GameType == EntityItem.EntityType.MorphPermanent);
                    morph.Columns.AddRange(sourceColumns);
                    Morphs.Add(morph);
                    break;

                case EntityItem.EntityType.MorphSource1:
                case EntityItem.EntityType.MorphSource2:
                    // no need to display morph sources since they are handled above by their targets
                    break;

                case EntityItem.EntityType.RecoveryTruck:
                    Craft recov = new Craft("RECOV0-0", entity.Center + Vector3.UnitY * 6f);
                    Entities.AddNode(recov);
                    break;

                case EntityItem.EntityType.Cone:
                    Cone cone = new Cone(entity.X + 0.5f, entity.Y + 0.104f, entity.Z + 0.5f);
                    Entities.AddNode(cone);
                    break;

                case EntityItem.EntityType.Checkpoint:
                    color = new Vector4(1.0f, 0.0f, 1.0f, 1.0f);
                    line = new Line(entity.Center, entity.Center + new Vector3(entity.OffsetX, 0, entity.OffsetY), color);
                    line.Name = "Checkpoint line " + entity.ID;
                    Entities.AddNode(line);
                    break;

                case EntityItem.EntityType.Explosion:
                    BillboardAnimation explosion = new BillboardAnimation("images/tmaps/explosion.png", 1f, 1f, 88, 74, 10);

                    explosion.Position = entity.Center;
                    Entities.AddNode(explosion);
                    break;

                case EntityItem.EntityType.ExtraFuel:
                    collectable = new Collectable(29);
                    break;
                case EntityItem.EntityType.FuelFull:
                    collectable = new Collectable(30);
                    break;
                case EntityItem.EntityType.DoubleFuel:
                    collectable = new Collectable(31);
                    break;

                case EntityItem.EntityType.ExtraAmmo:
                    collectable = new Collectable(32);
                    break;
                case EntityItem.EntityType.AmmoFull:
                    collectable = new Collectable(33);
                    break;
                case EntityItem.EntityType.DoubleAmmo:
                    collectable = new Collectable(34);
                    break;

                case EntityItem.EntityType.ExtraShield:
                    collectable = new Collectable(35);
                    break;
                case EntityItem.EntityType.ShieldFull:
                    collectable = new Collectable(36);
                    break;
                case EntityItem.EntityType.DoubleShield:
                    collectable = new Collectable(37);
                    break;

                case EntityItem.EntityType.BoosterUpgrade:
                    collectable = new Collectable(40);
                    break;
                case EntityItem.EntityType.MissileUpgrade:
                    collectable = new Collectable(39);
                    break;
                case EntityItem.EntityType.MinigunUpgrade:
                    collectable = new Collectable(38);
                    break;

                case EntityItem.EntityType.UnknownShieldItem:
                    collectable = new Collectable(41);
                    break;

                case EntityItem.EntityType.UnknownItem:
                case EntityItem.EntityType.Unknown:
                    collectable = new Collectable(50);
                    break;

                default:
                    boxSize = 0.98f;
                    break;
            }

            if (collectable != null)
            {
                collectable.Position = entity.Center;
                Entities.AddNode(collectable);
            }

            if (boxSize > 0f)
            {
                box = new Box(boxSize, entity.Center - 0.5f * boxSize * Vector3.One, color);
                Entities.AddNode(box);
            }
        }

        public List<Column> ColumnsInRange(int sx, int sz, float w, float h)
        {
            List<Column> columns = new List<Column>();
            for(int z = 0; z < h; z++)
            {
                for (int x = 0; x < w; x++)
                {
                    int key = sx + x + (sz + z) * Data.Width;
                    if (columnsByPosition.ContainsKey(key)) columns.Add(columnsByPosition[key]);
                }
            }

            return columns;
        }


        public override void Update(float dTime)
        {
            Time = Editor.Time;

            Camera.Update(dTime);


            // sky sphere
            skySphere.Position = new Vector3(Camera.Camera.Position.X, Camera.Camera.Position.Y - 200f, Camera.Camera.Position.Z);

            if (morphsEnabled)
            {
                float progress = (float)Math.Min(1f, Math.Max(0f, 0.5f + Math.Sin(Time)));

                if (progress != 0 && progress != 1) Editor.Profiler.Begin("MORPHS (CPU)");
                foreach (Morph morph in Morphs)
                {
                    morph.Progress = progress;
                    morph.MorphColumns();
                    Terrain.Morph(morph);
                }
                if (progress != 0 && progress != 1) Editor.Profiler.End("MORPHS (CPU)");
            }

            // sun direction
            //light.Direction.X = (float)Math.Cos(0.2f * Time);
            //light.Direction.Z = (float)Math.Sin(0.2f * Time);

            base.Update(dTime); // this is important! calling Update() on subnodes
        }

        private void disableMorphs()
        {
            foreach (Morph morph in Morphs)
            {
                morph.Progress = 0;
                morph.MorphColumns();
                Terrain.Morph(morph);
            }
        }


        public void Unload()
        {
            // TODO!
        }

    }
}
