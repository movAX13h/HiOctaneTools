using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using LevelEditor.Games.HiOctane.Models;
using LevelEditor.Utils;

namespace LevelEditor.Games.HiOctane.Resources
{
    public class MapEntry : TableItem
    {
        public float Height;
        public int TextureId;
        public int TextureModification;

        public int X { get; private set; }
        public int Z { get; private set; }

        public ColumnDefinition Column { get; private set; }

        public MapEntry(int x, int z, int offset, byte[] bytes, OrderedDictionary<int, ColumnDefinition> columnDefinitions)
        {
            X = x;
            Z = z;

            Offset = offset;
            Bytes = bytes;

            Height = (float)bytes[2] / 255f + (float)bytes[3];

            int cid = BitConverter.ToInt16(bytes, 4);
            if (cid < 0) // is column of blocks?
            {
                Column = columnDefinitions.GetValue(-cid);
                cid = Column.FloorTextureID;
            }

            TextureId = cid;
            TextureModification = (bytes[10] >> 4);
        }

        public override bool WriteChanges()
        {
            byte[] heightBytes = Height.ToBytesFractional();
            Bytes[2] = heightBytes[0];
            Bytes[3] = heightBytes[1];

            //Bytes[4] = (byte)(TextureId >> 8);
            //Bytes[5] = (byte)TextureId;

            //Bytes[10] = (byte)(TextureModification << 4);

            return true;
        }
    }

}
