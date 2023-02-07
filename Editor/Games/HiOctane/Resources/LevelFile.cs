using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;

using LevelEditor.Utils;
using System.Text;

namespace LevelEditor.Games.HiOctane.Resources
{

    public struct MapPointOfInterest
    {
        public Vector3 Position;
        public int Value;
    }

    public class LevelFile
    {
        public readonly int Width = 256;
        public readonly int Height = 160;
        public MapEntry[,] Map { get; private set; }

        public bool Ready { get; private set; }

        public string Filename { get; private set; }
        public string Name { get; private set; }

        private byte[] bytes;

        public List<MapPointOfInterest> PointsOfInterest { get; private set; }

        public OrderedDictionary<int, EntityItem> Entities { get; private set; }
        public OrderedDictionary<int, BlockDefinition> BlockDefinitions { get; private set; }
        public OrderedDictionary<int, ColumnDefinition> ColumnDefinitions { get; private set; }

        public OrderedDictionary<Vector3, ColumnDefinition> Columns { get; private set; }

        public LevelFile(string filename)
        {
            Filename = filename;
            Ready = false;

            if (!File.Exists(filename)) return;

            bytes = File.ReadAllBytes(Filename);

            Dictionary<uint, string> MapNames = new Dictionary<uint, string>();
            MapNames.Add(3308913189, "Amazon Delta Turnpike");
            MapNames.Add(2028380229, "Trans-Asia Interstate");
            MapNames.Add(3087166776, "Shanghai Dragon");
            MapNames.Add(1401140937, "New Chernobyl Central");
            MapNames.Add(4215346212, "Slam Canyon");
            MapNames.Add(3809451489, "Thrak City");
            MapNames.Add(3062313907, "Ancient Mine Town");
            MapNames.Add(3081898808, "Arctic Land");
            MapNames.Add(540505443, "Death Match Arena");

            Crc32 crc = new Crc32();
            uint cs = crc.ComputeChecksum(bytes);

            if (MapNames.ContainsKey(cs)) Name = MapNames[cs];
            else Name = "Custom " + Path.GetFileNameWithoutExtension(Filename);

            Ready = loadBlockTexTable() && loadColumnsTable() && loadMap() && loadEntitiesTable();
        }

        public bool Save(string filename)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapEntry entry = Map[x, y];
                    entry.WriteChanges();
                    entry.Bytes.CopyTo(bytes, entry.Offset);
                }
            }

            File.WriteAllBytes(filename, bytes);

            return true;
        }
        
        private bool loadEntitiesTable()
        {
            Entities = new OrderedDictionary<int, EntityItem>();

            for (int i = 0; i < 4000; i++)
            {
                int baseOffset = i * 24;
                if (bytes[baseOffset] == 0) continue;

                EntityItem item = new EntityItem(i, baseOffset, bytes.SubArray(baseOffset, 24));
                item.Y = Map[(int)item.X, (int)item.Z].Height;
                Entities.Add(i, item);
            }

            return true;
        }

        private bool loadBlockTexTable()
        {
            BlockDefinitions = new OrderedDictionary<int, BlockDefinition>();

            for (int i = 0; i < 1024; i++)
            {
                int baseOffset = 124636 + i * 16;
                if (bytes[baseOffset] == 0) continue;

                BlockDefinition item = new BlockDefinition(i, baseOffset, bytes.SubArray(baseOffset, 16));
                BlockDefinitions.Add(i, item);
            }

            return true;
        }

        private bool loadColumnsTable()
        {
            ColumnDefinitions = new OrderedDictionary<int, ColumnDefinition>();

            for (int i = 0; i < 1024; i++)
            {
                int baseOffset = 98012 + i * 26;
                if (bytes[baseOffset] == 0) continue;

                ColumnDefinition item = new ColumnDefinition(i, baseOffset, bytes.SubArray(baseOffset, 26));
                ColumnDefinitions.Add(i, item);
            }

            return true;
        }

        private bool loadMap()
        {
            PointsOfInterest = new List<MapPointOfInterest>();
            Columns = new OrderedDictionary<Vector3, ColumnDefinition>();

            Map = new MapEntry[Width, Height];

            // entry is 12 bytes long, map is at end of file
            int numBytes = 12 * Width * Height;

            int i = bytes.Length - numBytes;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapEntry entry = new MapEntry(x, y, i, bytes.SubArray(i, 12), ColumnDefinitions);
                    if (entry.Column != null) Columns.Add(new Vector3(x, 0, y), entry.Column);

                    Map[x, y] = entry;

                    // check for points of interest
                    int poiValue = (bytes[i + 7] << 8) | bytes[i + 6];
                    if (poiValue > 0)
                    {
                        MapPointOfInterest poi = new MapPointOfInterest();
                        poi.Value = poiValue;
                        poi.Position = new Vector3(x, 0.0f, y);
                        PointsOfInterest.Add(poi);
                    }

                    i += 12;
                }
            }

            return true;
        }

        #region JSON export
        public bool SaveJSON(string filename)
        {
            // minimal JSON data required to reconstruct the level
            StringBuilder s = new StringBuilder();
            s.Append("{\"name\":\"" + Name.Replace("\"", "") + "\",");
            s.Append("\"blocks\":").Append(BlockDefinitionsToJSON()).Append(",");
            s.Append("\"columns\":").Append(ColumnDefinitionsToJSON()).Append(",");
            s.Append("\"entities\":").Append(EntitiesToJSON()).Append(",");
            s.Append("\"terrain\":").Append(MapToJSON());
            s.Append("}");
            File.WriteAllText(filename, s.ToString());
            return true;
        }

        public string ColumnDefinitionsToJSON()
        {
            List<string> l = new List<string>();
            foreach (var def in ColumnDefinitions.Values) l.Add(def.ToJSON());
            return "[" + string.Join(",", l) + "]";
        }

        public string BlockDefinitionsToJSON()
        {
            List<string> l = new List<string>();
            foreach (var def in BlockDefinitions.Values) l.Add(def.ToJSON());                
            return "[" + string.Join(",", l) + "]";
        }

        public string EntitiesToJSON()
        {
            List<string> l = new List<string>();
            foreach(var item in Entities.Values) l.Add(item.ToJSON());
            return "[" + string.Join(",", l) + "]";
        }

        public string MapToJSON()
        {
            StringBuilder s = new StringBuilder();
            
            List<string> cells = new List<string>();
            for (int z = 0; z < Height; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    MapEntry e = Map[x, z];// GetMapEntry(x, z);
                    cells.Add(e.ToJSON());
                }
            }

            s.Append("{\"width\":" + Width + ",\"height\":" + Height + ",");
            s.Append("\"cells\":[");
            s.Append(string.Join(",", cells));
            s.Append("]}");

            return s.ToString();
        }
        #endregion

    }
}
