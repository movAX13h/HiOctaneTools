using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelInspector.Tables
{
    public class ColumnItem : TableItem
    {
        public string Shape { get { return Convert.ToString(Bytes[0], 2).PadLeft(8, '0'); } }
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


        /*
struct column_s
{
// every bit controls a block - maximum value is 0xFF, that means an 8-block-high wall
short shape;

// texture-index for the terrain, under the building
// this CAN be negative! perhaps lets you access the object's atlas-texture, then
short floor_texture_index;

// does not appear to have any effect
short unknown2;

// refers to a texture-set for each block
short blocktex_index[8];

// does not appear to have any effect
short unknown[2];
};

         */


        public ColumnItem(int id, int offset, byte[] bytes)
        {
            ID = id;
            Bytes = bytes;
            Offset = offset;

        }

        public override string[] ToStringArray()
        {
            string[] a = new string[15];
            a[0] = ID.ToString();
            a[1] = Shape;
            //a[2] = Bytes[1].ToString();
            a[2] = FloorTextureID.ToString();
            a[3] = Unknown1.ToString();
            a[4] = A.ToString();
            a[5] = B.ToString();
            a[6] = C.ToString();
            a[7] = D.ToString();
            a[8] = E.ToString();
            a[9] = F.ToString();
            a[10] = G.ToString();
            a[11] = H.ToString();
            a[12] = Unknown2.ToString();
            a[13] = Unknown3.ToString();

            a[14] = Offset.ToString();
            //a[23] = GameType.ToString();

            return a;
        }
    }
}
