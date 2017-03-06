using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using LevelInspector.Utils;

namespace LevelInspector.Tables
{
    public static class BlockTexTable
    {
        public static Dictionary<int, BlockTexItem> Fill(ListView listView, ref byte[] data, ref Color[] lineColors, ref Color[] pointColors)
        {
            listView.Items.Clear();
            Dictionary<int, BlockTexItem> items = new Dictionary<int, BlockTexItem>();

            for (int i = 0; i < 1024; i++)
            {
                int baseOffset = 124636 + i * 16;
                if (data[baseOffset] == 0) continue;

                BlockTexItem item = new BlockTexItem(i, baseOffset, data.SubArray(baseOffset, 16));

                string[] captions = item.ToStringArray();

                ListViewItem listItem = new ListViewItem(captions);
                listItem.Tag = item;
                listItem.UseItemStyleForSubItems = false;
                item.ListItem = listItem;

                int j = 0;
                foreach (string caption in captions)
                {
                    if (j > 0 && j < 7) listItem.SubItems[j].BackColor = Color.LightSeaGreen;
                    j++;
                }

                listView.Items.Add(listItem);
                items.Add(i, item);
            }

            return items;
        }

        public static void Write(Dictionary<int, BlockTexItem> items, ref byte[] data)
        {
            foreach(KeyValuePair<int,BlockTexItem> pair in items)
            {
                BlockTexItem item = pair.Value;
                int baseOffset = item.Offset;
                for (int i = 0; i < 16; i++) data[baseOffset + i] = item.Bytes[i];
            }
        }

        public static void Init(ListView listView)
        {
            listView.DoubleBuffered(true);
            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;

            listView.Columns.Add("ID", 30);

            listView.Columns.Add("N", 30);
            listView.Columns.Add("E", 30);
            listView.Columns.Add("S", 30);
            listView.Columns.Add("W", 30);
            listView.Columns.Add("T", 30);
            listView.Columns.Add("B", 30);
            listView.Columns.Add("b6", 30);
            listView.Columns.Add("b7", 30);
            listView.Columns.Add("b8", 30);
            listView.Columns.Add("b9", 30);
            listView.Columns.Add("b10", 30);
            listView.Columns.Add("b11", 30);
            listView.Columns.Add("b12", 30);
            listView.Columns.Add("b13", 30);
            listView.Columns.Add("b14", 30);
            listView.Columns.Add("b15", 30);

            listView.Columns.Add("offset", 50);
            //            listView.Columns.Add("ENTITY", 100);
        }
    }
}
