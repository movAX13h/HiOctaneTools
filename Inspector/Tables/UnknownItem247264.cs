using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelInspector.Tables
{
    public class UnknownItem247264 : TableItem
    {

        public UnknownItem247264(int id, int offset, byte[] bytes)
        {
            ID = id;
            Bytes = bytes;
            Offset = offset;

        }

        public override string[] ToStringArray()
        {
            string[] a = new string[87];
            a[0] = ID.ToString();
            int i = 1;
            foreach(byte b in Bytes)
            {
                a[i] = Bytes[i - 1].ToString();
                i++;
            }
            a[86] = Offset.ToString();

            return a;
        }
    }
}
