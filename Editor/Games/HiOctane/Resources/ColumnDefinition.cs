using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LevelEditor.Games.HiOctane.Resources
{
    public class ColumnDefinition : TableItem
    {
        public byte Shape { get { return Bytes[0]; } }
        public int FloorTextureID { get { return BitConverter.ToInt16(Bytes, 2); } }
        public int Unknown1 { get { return BitConverter.ToInt16(Bytes, 4); } }
        public int A { get { return BitConverter.ToInt16(Bytes, 6); } }
        public int B { get { return BitConverter.ToInt16(Bytes, 8); } }
        public int C { get { return BitConverter.ToInt16(Bytes, 10); } }
        public int D { get { return BitConverter.ToInt16(Bytes, 12); } }
        public int E { get { return BitConverter.ToInt16(Bytes, 14); } }
        public int F { get { return BitConverter.ToInt16(Bytes, 16); } }
        public int G { get { return BitConverter.ToInt16(Bytes, 18); } }
        public int H { get { return BitConverter.ToInt16(Bytes, 20); } }
        public int Unknown2 { get { return BitConverter.ToInt16(Bytes, 22); } }
        public int Unknown3 { get { return BitConverter.ToInt16(Bytes, 24); } }

        public int[] Blocks { get; private set; }

        public ColumnDefinition(int id, int offset, byte[] bytes)
        {
            ID = id;
            Bytes = bytes;
            Offset = offset;
            Blocks = new int[8] {A, B, C, D, E, F, G, H };
        }

        public override bool WriteChanges()
        {
            return false;
        }
    }
}
