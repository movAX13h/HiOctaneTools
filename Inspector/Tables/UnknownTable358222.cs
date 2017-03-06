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
    public static class UnknownTable358222
    {
        public static Dictionary<int, UnknownItem358222> Fill(ListView listView, ref byte[] data, ref Color[] lineColors, ref Color[] pointColors)
        {
            listView.Items.Clear();
            Dictionary<int, UnknownItem358222> items = new Dictionary<int, UnknownItem358222>();

            for (int i = 0; i < 170; i++)
            {
                int baseOffset = 358222 + i * 16;
                if (data[baseOffset] == 0) continue;

                UnknownItem358222 item = new UnknownItem358222(i, baseOffset, data.SubArray(baseOffset, 16));

                string[] captions = item.ToStringArray();

                ListViewItem listItem = new ListViewItem(captions);
                listItem.Tag = item;
                listItem.UseItemStyleForSubItems = false;
                item.ListItem = listItem;

                /*
                int j = 0;
                foreach(string caption in captions)
                {
                    if (j > 1 && j < 14 && caption != "0") listItem.SubItems[j].BackColor = j != 3 && j < 12 ? Color.LightSeaGreen : Color.LightGreen;
                    j++;
                }
                */
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

        public static void Write(Dictionary<int, UnknownItem358222> items, ref byte[] data)
        {
            foreach (KeyValuePair<int, UnknownItem358222> pair in items)
            {
                UnknownItem358222 item = pair.Value;
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

            for (int i = 0; i < 16; i++)
            {
                listView.Columns.Add("b" + i, 30);
            }

            listView.Columns.Add("offset", 50);
//            listView.Columns.Add("ENTITY", 100);
        }
    }
}
