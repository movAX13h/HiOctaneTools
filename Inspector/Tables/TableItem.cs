using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelInspector.Tables
{
    public class TableItem
    {
        public byte[] Bytes { get; protected set; }
        public int ID { get; protected set; }
        public int Offset { get; protected set; }

        public ListViewItem ListItem;

        public virtual string[] ToStringArray()
        {
            return new string[0];
        }

        public void UpdateCaptions()
        {
            string[] a = ToStringArray();
            for(int i = 0; i < a.Length; i++)
            {
                ListItem.SubItems[i].Text = a[i];
            }
        }
    }
}
