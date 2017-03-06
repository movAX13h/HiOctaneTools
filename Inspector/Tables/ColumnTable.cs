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
    public static class ColumnTable
    {
        public static Dictionary<int, ColumnItem> Fill(ListView listView, ref byte[] data, ref Color[] lineColors, ref Color[] pointColors)
        {
            listView.Items.Clear();
            Dictionary<int, ColumnItem> items = new Dictionary<int, ColumnItem>();

            for (int i = 0; i < 1024; i++)
            {
                int baseOffset = 98012 + i * 26;
                if (data[baseOffset] == 0) continue;

                ColumnItem item = new ColumnItem(i, baseOffset, data.SubArray(baseOffset, 26));

                string[] captions = item.ToStringArray();

                ListViewItem listItem = new ListViewItem(captions);
                listItem.Tag = item;
                listItem.UseItemStyleForSubItems = false;
                item.ListItem = listItem;

                int j = 0;
                foreach(string caption in captions)
                {
                    if (j > 1 && j < 14 && caption != "0") listItem.SubItems[j].BackColor = j != 3 && j < 12 ? Color.LightSeaGreen : Color.LightGreen;
                    j++;
                }

                /*
                if (item.GameType == EntityItem.EntityType.Unknown)
                {
                    for (int j = 0; j < listItem.SubItems.Count; j++)
                        listItem.SubItems[j].BackColor = Color.MistyRose;
                }

                listItem.SubItems[1].BackColor = Color.LightGray;
                listItem.SubItems[2].BackColor = Color.LightGray;
                listItem.SubItems[13].BackColor = Color.LightGray;
                listItem.SubItems[16].BackColor = Color.LightGray;
                listItem.SubItems[17].BackColor = Color.LightGray;

                if (item.Type == 9) listItem.SubItems[23].BackColor = lineColors[item.SubType];
                else listItem.SubItems[23].BackColor = pointColors[item.Type];
                */

                listView.Items.Add(listItem);
                items.Add(i, item);
            }

            return items;
        }

        public static void Write(Dictionary<int, ColumnItem> items, ref byte[] data)
        {
            foreach (KeyValuePair<int, ColumnItem> pair in items)
            {
                ColumnItem item = pair.Value;
                int baseOffset = item.Offset;
                for (int i = 0; i < 26; i++) data[baseOffset + i] = item.Bytes[i];
            }
        }

        public static void Init(ListView listView)
        {
            listView.DoubleBuffered(true);
            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;

            listView.Columns.Add("ID", 30);

            listView.Columns.Add("Shape", 60);

            //listView.Columns.Add("b1", 30);

            listView.Columns.Add("FloorTex", 40);
            listView.Columns.Add("s4", 40);

            listView.Columns.Add("A", 30);
            listView.Columns.Add("B", 30);
            listView.Columns.Add("C", 30);
            listView.Columns.Add("D", 30);
            listView.Columns.Add("E", 30);
            listView.Columns.Add("F", 30);
            listView.Columns.Add("G", 30);
            listView.Columns.Add("H", 30);

            listView.Columns.Add("s22", 40);
            listView.Columns.Add("s24", 40);

            listView.Columns.Add("offset", 50);
//            listView.Columns.Add("ENTITY", 100);
        }
    }
}
