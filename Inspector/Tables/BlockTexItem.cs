using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelInspector.Tables
{
    public class BlockTexItem : TableItem
    {
        public BlockTexItem(int id, int offset, byte[] bytes)
        {
            ID = id;
            Bytes = bytes;
            Offset = offset;
        }

        public override string[] ToStringArray()
        {
            string[] a = new string[18];

            a[0] = ID.ToString();
            a[1] = Bytes[0].ToString();
            a[2] = Bytes[1].ToString();
            a[3] = Bytes[2].ToString();
            a[4] = Bytes[3].ToString();
            a[5] = Bytes[4].ToString();
            a[6] = Bytes[5].ToString();
            a[7] = Bytes[6].ToString();
            a[8] = Bytes[7].ToString();
            a[9] = Bytes[8].ToString();
            a[10] = Bytes[9].ToString();
            a[11] = Bytes[10].ToString();
            a[12] = Bytes[11].ToString();
            a[13] = Bytes[12].ToString();
            a[14] = Bytes[13].ToString();
            a[15] = Bytes[14].ToString();
            a[16] = Bytes[15].ToString();

            a[17] = Offset.ToString();
            //a[23] = GameType.ToString();

            return a;
        }
    }
}
