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
    public static class EntityTable
    {
        public static Dictionary<int, EntityItem> Fill(ListView listView, ref byte[] data, ref Color[] lineColors, ref Color[] pointColors)
        {
            listView.Items.Clear();
            Dictionary<int, EntityItem> items = new Dictionary<int, EntityItem>();

            for (int i = 0; i < 4000; i++)
            {
                int baseOffset = i * 24;
                if (data[baseOffset] == 0) continue;

                EntityItem item = new EntityItem(i, baseOffset, data.SubArray(baseOffset, 24));

                ListViewItem listItem = new ListViewItem(item.ToStringArray());
                listItem.Tag = item;
                listItem.UseItemStyleForSubItems = false;
                item.ListItem = listItem;

                if (item.GameType == EntityItem.EntityType.Unknown)
                {
                    for (int j = 0; j < listItem.SubItems.Count; j++)
                        listItem.SubItems[j].BackColor = Color.MistyRose;
                }

                Color itemColor = pointColors[item.Type];
                if (item.Type == 9) itemColor = Color.FromArgb(255, 255, 200, 255); // waypoints
                if (item.GameType == EntityItem.EntityType.WallSegment) itemColor = Color.FromArgb(255, 255, 150, 150); // walls

                listItem.SubItems[0].BackColor = itemColor;
                listItem.SubItems[1].BackColor = itemColor;
                listItem.SubItems[12].BackColor = itemColor;

                listItem.SubItems[2].BackColor = Color.FromArgb(255, 205, 80, 180); // type
                listItem.SubItems[3].BackColor = Color.FromArgb(255, 255, 100, 230); // subtype

                if (item.Group != 1) listItem.SubItems[4].BackColor = lineColors[item.Group]; // group
                if (item.TargetGroup != 0) listItem.SubItems[5].BackColor = lineColors[item.TargetGroup]; // group target


                // link
                listItem.SubItems[6].BackColor = item.NextID != 0 ? Color.FromArgb(255, 100, 255, 100) : Color.FromArgb(255, 200, 255, 200);
                if (item.Bytes[14] != 0) listItem.SubItems[7].BackColor = Color.Aquamarine;

                // x and y
                listItem.SubItems[8].BackColor = Color.LightSkyBlue;
                listItem.SubItems[9].BackColor = Color.LightSkyBlue;

                // ox and oy
                listItem.SubItems[10].BackColor = item.OffsetX != 0 || item.OffsetY != 0 ? Color.FromArgb(255, 180, 180, 180) : Color.FromArgb(255, 220, 220, 220);
                listItem.SubItems[11].BackColor = listItem.SubItems[10].BackColor;

                listView.Items.Add(listItem);
                items.Add(i, item);
            }

            return items;
        }

        public static void Write(Dictionary<int, EntityItem> items, ref byte[] data)
        {
            foreach (KeyValuePair<int, EntityItem> pair in items)
            {
                EntityItem item = pair.Value;
                int baseOffset = item.Offset;
                for (int i = 0; i < 24; i++) data[baseOffset + i] = item.Bytes[i];
            }
        }

        public static void Init(ListView listView)
        {
            listView.DoubleBuffered(true);
            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;

            listView.Columns.Add("ID", 40);
            listView.Columns.Add("ENTITY", 100);

            listView.Columns.Add("TYPE", 40);
            listView.Columns.Add("SUBTYPE", 40);

            listView.Columns.Add("GRP", 42);
            listView.Columns.Add("TARGET GRP", 40);

            listView.Columns.Add("LINK", 40);

            listView.Columns.Add("VALUE", 40);

            listView.Columns.Add("X", 40);
            listView.Columns.Add("Y", 40);
            listView.Columns.Add("DX", 30);
            listView.Columns.Add("DY", 30);

            listView.Columns.Add("OFFSET", 60);
        }

    }
}
