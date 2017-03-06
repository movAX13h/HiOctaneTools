using LevelInspector.Tables;
using LevelInspector.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelInspector
{
    public partial class MapForm : Form
    {
        private Bitmap canvas;

        private byte[] data;
        private Color[] pointColors;
        private Color[] lineColors;

        private float maxHeight = 0.1f;
        private float minHeight = 1e8f;

        private bool entityTableLocked = false;
        private Dictionary<int, EntityItem> entityItems;

        private Dictionary<int, ColumnItem> columnItems;
        private Dictionary<int, BlockTexItem> blockTexItems;
        private Dictionary<int, UnknownItem247264> unknownItems247264;
        private Dictionary<int, UnknownItem358222> unknownItems358222;

        private List<MapPointOfInterest> pois;

        private Atlas atlas;
        private string outFilename;

        public MapForm(ref byte[] bytes, Atlas atlas, string outFilename)
        {
            this.atlas = atlas;
            this.outFilename = outFilename;

            InitializeComponent();

            canvasBox.DoubleBuffered(true);

            initColors();
            EntityTable.Init(entityTable);
            ColumnTable.Init(columnTable);
            BlockTexTable.Init(blockTexTable);
            UnknownTable247264.Init(unknownTable247264);
            UnknownTable358222.Init(unknownTable358222);

            data = bytes;
            findHeightExtremes();
            fillEntityTable();
            fillColumnTable();
            fillBlockTexTable();
            fillUnknownTable247264();
            fillUnknownTable358222();
            draw();
        }

        private void fillEntityTable()
        {
            entityItems = EntityTable.Fill(entityTable, ref data, ref lineColors, ref pointColors);
            entityTableInfo.Text = entityItems.Count + " items found";
        }

        private void fillColumnTable()
        {
            columnItems = ColumnTable.Fill(columnTable, ref data, ref lineColors, ref pointColors);
            columnTableInfo.Text = columnItems.Count + " items found";
        }

        private void fillBlockTexTable()
        {
            blockTexItems = BlockTexTable.Fill(blockTexTable, ref data, ref lineColors, ref pointColors);
            blockTexTableInfo.Text = blockTexItems.Count + " items found";
        }

        private void fillUnknownTable247264()
        {
            unknownItems247264 = UnknownTable247264.Fill(unknownTable247264, ref data, ref lineColors, ref pointColors);
            unknownTableInfo.Text = unknownItems247264.Count + " items found";
        }

        private void fillUnknownTable358222()
        {
            unknownItems358222 = UnknownTable358222.Fill(unknownTable358222, ref data, ref lineColors, ref pointColors);
            unknownTable358222Info.Text = unknownItems358222.Count + " items found";
        }

        public void Write()
        {
            writeButton.Enabled = false;
            File.WriteAllBytes(outFilename, data);
        }

        private void draw()
        {
            if (entityTableLocked) return;

            logTextbox.Text = "";

            if (canvas != null) canvas.Dispose();

            int blockSize = (int)(zoomInput.Value);

            canvas = new Bitmap(blockSize * 256, blockSize * 160);
            Graphics gfx = Graphics.FromImage(canvas);
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gfx.Clear(Color.Black);

            drawMap(gfx, blockSize, (int)mapByteInput.Value);

            // entity table
            if (entityTableCheckbox.Checked)
            {
                if (entityTable.Items.Count == 0) fillEntityTable();
                if (connect9EntityCheckbox.Checked) connectEntityType(9, gfx);

                drawEntities(gfx);
            }
            else entityTable.Items.Clear();

            // column table
            if (columnTableCheckbox.Checked)
            {
                if (columnTable.Items.Count == 0) fillColumnTable();
                //drawTable1(gfx);
            }
            else columnTable.Items.Clear();

            // block tex table
            if (blockTexTableCheckbox.Checked)
            {
                if (blockTexTable.Items.Count == 0) fillBlockTexTable();
                //drawTable1(gfx);
            }
            else blockTexTable.Items.Clear();

            canvasBox.Image = canvas;
            gfx.Dispose();
        }

        private void drawMap(Graphics gfx, int blockSize, int byteNum)
        {
            if (byteNum == -3) return;

            // draw map
            SolidBrush brush = new SolidBrush(Color.Black);
            SolidBrush poiBrush = new SolidBrush(Color.Black);

            pois = new List<MapPointOfInterest>();

            int i,c, r;
            byte b;
            float h;
            float scale = 255 / (maxHeight - minHeight);

            // selection lists
            List<int> selectedCellsColumn = new List<int>();
            if (highlightColumnsCheckbox.Checked)
            {
                foreach (ListViewItem item in columnTable.SelectedItems)
                {
                    selectedCellsColumn.Add((item.Tag as ColumnItem).ID);
                }
            }

            List<int> selectedCellsBlock = new List<int>();
            if (highlightBlockTexCheckbox.Checked)
            {
                foreach(ListViewItem item in blockTexTable.SelectedItems)
                {
                    int id = (item.Tag as BlockTexItem).ID;

                    // find all columns using this id
                    foreach(KeyValuePair<int, ColumnItem> cPair in columnItems)
                    {
                        ColumnItem cItem = cPair.Value;
                        if (cItem.FloorTextureID == id ||
                            cItem.A == id ||
                            cItem.B == id ||
                            cItem.C == id ||
                            cItem.D == id ||
                            cItem.E == id ||
                            cItem.F == id ||
                            cItem.G == id ||
                            cItem.H == id)
                        {
                            selectedCellsBlock.Add(cItem.ID);
                        }
                    }

                }
            }

            for (int y = 0; y < 160; y++)
            for (int x = 0; x < 256; x++)
            {
                int baseOffset = 404620 + (x + y * 256) * 12;

                if (byteNum == -2)
                {
                    int cid = BitConverter.ToInt16(data, baseOffset + 4);
                    r = (data[baseOffset + 10] >> 4);

                    if (cid < 0) cid = columnItems[-cid].FloorTextureID;

                    gfx.DrawImage(atlas.Get(cid, r), x * blockSize, y * blockSize, blockSize, blockSize);

                    // collect points of interest
                    int poiValue = (data[baseOffset + 7] << 8) | data[baseOffset + 6];
                    if (poiValue > 0)
                    {
                        MapPointOfInterest poi = new MapPointOfInterest();
                        poi.Position = new PointF(x * blockSize + blockSize * 0.5f, y * blockSize + blockSize * 0.5f);
                        poi.Value = poiValue;
                        pois.Add(poi);
                    }
                }
                else
                {
                    if (byteNum >= 0)
                    {
                        // simple byte map (NOTE: some planes contain values invisible in grayscale)
                        i = baseOffset + byteNum;
                        b = data[i];
                        brush.Color = Color.FromArgb(255, b, b, b);
                    }
                    else
                    {
                        // normalized heightmap
                        i = baseOffset + 2;
                        h = data[i] / 256 + data[i + 1] - minHeight;
                        c = (int)Math.Floor(h * scale);
                        c = Math.Max(0, Math.Min(255, c));
                        brush.Color = Color.FromArgb(255, c, c, c);

                        // special areas
                        /*if (data[baseOffset + 10] > 0)
                        {
                            brush.Color = Color.Red;
                        }*/
                    }

                    if (selectedCellsColumn.Count > 0 || selectedCellsBlock.Count > 0)
                    {
                        // get map column index at current position
                        int cid = BitConverter.ToInt16(data, baseOffset + 4);
                        if (cid < 0)
                        {
                            if (selectedCellsColumn.Contains(-cid)) brush.Color = Color.Purple;
                            if (selectedCellsBlock.Contains(-cid)) brush.Color = Color.Violet;
                        }
                    }

                    gfx.FillRectangle(brush, x * blockSize, y * blockSize, blockSize, blockSize);
                }
            }


            foreach (MapPointOfInterest poi in pois)
            {
                poiBrush.Color = Color.FromArgb(20, 255, 255, 0);
                //gfx.FillRectangle(Brushes.Yellow, x * blockSize, y * blockSize, blockSize, blockSize);
                gfx.FillCircle(poiBrush, poi.Position.X, poi.Position.Y, 3.0f * blockSize);
                SizeF ts = gfx.MeasureString(poi.Value.ToString(), SystemFonts.DefaultFont);
                gfx.DrawString(poi.Value.ToString(), SystemFonts.DefaultFont, Brushes.Black, poi.Position.X - ts.Width * 0.5f + 1, poi.Position.Y - ts.Height * 0.5f + 1);
                gfx.DrawString(poi.Value.ToString(), SystemFonts.DefaultFont, Brushes.White, poi.Position.X - ts.Width * 0.5f, poi.Position.Y - ts.Height * 0.5f);
            }
        }

        private void findHeightExtremes()
        {
            int i = 0;
            float h;

            for (int y = 0; y < 160; y++)
                for (int x = 0; x < 256; x++)
                {
                    i = 404620 + (x + y * 256) * 12 + 2;
                    h = data[i] / 256 + data[i + 1];
                    maxHeight = Math.Max(maxHeight, h);
                    minHeight = Math.Min(minHeight, h);
                }

        }

        private void connectEntityType(int type, Graphics gfx)
        {
            int blockSize = (int)(zoomInput.Value);
            Pen pen = new Pen(Color.Red);
            pen.Width = 2;

            int numOtherMissing = 0;
            int numDifferentType = 0;
            int numNextNull = 0;

            foreach(KeyValuePair<int,EntityItem> pair in entityItems)
            {
                EntityItem item = pair.Value;
                if (item.Type != type) continue;

                if (item.NextID != 0)
                {
                    if (entityItems.ContainsKey(item.NextID))
                    {
                        EntityItem nextItem = entityItems[item.NextID];
                        if (nextItem.Type != type)
                        {
                            logTextbox.AppendText("Other type: " + item.ID + " links to " + nextItem.ID + " of type " + nextItem.Type + Environment.NewLine);
                            numDifferentType++;
                        }
                        //else
                        //{
                            pen.Color = lineColors[item.SubType];
                            gfx.DrawLine(pen, (item.X + 0.5f) * blockSize, (item.Y + 0.5f) * blockSize, (nextItem.X + 0.5f) * blockSize, (nextItem.Y + 0.5f) * blockSize);
                        //}
                    }
                    else
                    {
                        logTextbox.AppendText("Missing: " + item.NextID.ToString() + Environment.NewLine);
                        numOtherMissing++;
                    }
                }
                else numNextNull++;
            }

            logTextbox.AppendText("Num different type: " + numDifferentType.ToString() + Environment.NewLine);
            logTextbox.AppendText("Num missing: " + numOtherMissing.ToString() + Environment.NewLine);
            logTextbox.AppendText("Num next is null: " + numNextNull.ToString() + Environment.NewLine);
        }

        private void drawEntities(Graphics gfx)
        {
            if (entityTable.SelectedItems.Count > 0)
            {
                foreach (ListViewItem listItem in entityTable.SelectedItems)
                {
                    EntityItem item = (EntityItem)listItem.Tag;
                    drawEntity(gfx, item);
                }
            }
            else
            {
                foreach (ListViewItem listItem in entityTable.Items)
                {
                    EntityItem item = (EntityItem)listItem.Tag;
                    drawEntity(gfx, item);
                }
            }
        }

        private void drawEntity(Graphics gfx, EntityItem item)
        {
            int blockSize = (int)(zoomInput.Value);
            float bh = blockSize * 0.5f;
            float rad = (float)dotsizeInput.Value;
            SolidBrush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(Color.FromArgb(100, 100, 255, 100), 1);

            float w, h;
            float itemX = item.X * blockSize;
            float itemY = item.Y * blockSize;
            SizeF bounds;

            w = (item.OffsetX + 1) * blockSize;
            h = (item.OffsetY + 1) * blockSize;

            if (item.OffsetX == 0 || item.OffsetY == 0)
            {
                w = item.OffsetX * blockSize;
                h = item.OffsetY * blockSize;
            }

            switch (item.Type)
            {
                case 1: // checkpoints
                    rad = 8f;
                    pen = new Pen(Color.FromArgb(150, 170, 216, 255), 3);
                    gfx.DrawLine(pen, itemX + bh, itemY + bh, itemX + item.OffsetX * blockSize + bh, itemY + item.OffsetY * blockSize + bh);
                    gfx.FillCircle(item.Bytes[14] == 0 ? Brushes.Red : Brushes.White, itemX + bh, itemY + bh, rad + 2f);
                    gfx.FillCircle(Brushes.Black, itemX + bh, itemY + bh, rad);
                    bounds = gfx.MeasureString(item.Bytes[14].ToString(), SystemFonts.DefaultFont);
                    gfx.DrawString(item.Bytes[14].ToString(), SystemFonts.DefaultFont, Brushes.White, itemX + bh - bounds.Width * 0.5f, itemY + bh - 6);
                    break;
                case 2:
                    drawEntityDot(gfx, item);

                    if (morphCheckbox.Checked)
                    {
                        if (item.NextID > 0) // line to linked morph area
                        {
                            pen = new Pen(Color.FromArgb(100, 100, 255, 100), 1);
                            EntityItem parent = entityItems[item.NextID];
                            gfx.DrawLine(pen, itemX, itemY, parent.X * blockSize, parent.Y * blockSize);
                            gfx.DrawLine(pen, itemX + w, itemY, parent.X * blockSize + w, parent.Y * blockSize);
                            gfx.DrawLine(pen, itemX + w, itemY + h, parent.X * blockSize + w, parent.Y * blockSize + h);
                            gfx.DrawLine(pen, itemX, itemY + h, parent.X * blockSize, parent.Y * blockSize + h);
                        }

                        switch (item.SubType)
                        {
                            case 9: // morph source
                            case 7:
                            case 16: // morph target
                            case 23:
                                brush.Color = Color.FromArgb(60, 100, 255, 100);

                                if (w == 0 || h == 0)
                                {
                                    pen = new Pen(item.SubType == 9 || item.SubType == 7 ? Color.AliceBlue : Color.LightGreen, 3);
                                    gfx.DrawLine(pen, itemX, itemY, itemX + w, itemY + h);
                                }
                                else
                                {
                                    gfx.FillRectangle(brush, itemX, itemY, w, h);
                                    gfx.DrawRectangle(item.SubType == 9 || item.SubType == 7 ? Pens.AliceBlue : Pens.LightGreen, itemX, itemY, w, h);
                                }
                                bounds = gfx.MeasureString(item.Group.ToString(), SystemFonts.DefaultFont);
                                gfx.DrawString(item.Group.ToString(), SystemFonts.DefaultFont, Brushes.Black, itemX + (w - bounds.Width) * 0.5f + 1f, itemY + (h - bounds.Height) * 0.5f + 1f);
                                gfx.DrawString(item.Group.ToString(), SystemFonts.DefaultFont, Brushes.LightGreen, itemX + (w - bounds.Width) * 0.5f, itemY + (h - bounds.Height) * 0.5f);
                                break;
                        }
                    }
                    break;

                case 8:
                    drawEntityDot(gfx, item);
                    switch(item.SubType)
                    {
                        case 0: // triggers
                        case 3:
                            w = (item.OffsetX + 1f) * blockSize;
                            h = (item.OffsetY + 1f) * blockSize;
                            brush.Color = Color.FromArgb(200, 255, 100, 100);
                            gfx.FillRectangle(brush, itemX, itemY, w, h);
                            gfx.DrawRectangle(Pens.Purple, itemX, itemY, w, h);
                            bounds = gfx.MeasureString(item.TargetGroup.ToString(), SystemFonts.DefaultFont);
                            gfx.DrawString(item.TargetGroup.ToString(), SystemFonts.DefaultFont, Brushes.Black, itemX + (w - bounds.Width) * 0.5f + 1f, itemY + (h - bounds.Height) * 0.5f + 1f);
                            gfx.DrawString(item.TargetGroup.ToString(), SystemFonts.DefaultFont, Brushes.BlueViolet, itemX + (w - bounds.Width) * 0.5f, itemY + (h - bounds.Height) * 0.5f);
                            break;

                    }
                    break;

                default:
                    drawEntityDot(gfx, item);
                    break;
            }
        }

        private void drawEntityDot(Graphics gfx, EntityItem item)
        {
            int blockSize = (int)(zoomInput.Value);
            float bh = blockSize * 0.5f;
            float rad = (float)dotsizeInput.Value;

            SolidBrush brush = new SolidBrush(Color.Red);
            brush.Color = pointColors[item.Type];

            SolidBrush dropBrush = new SolidBrush(Color.White);

            gfx.FillCircle(dropBrush, item.X * blockSize + bh, item.Y * blockSize + bh, rad + 1f);

            dropBrush.Color = Color.FromArgb(127, Color.Black);
            gfx.FillCircle(dropBrush, item.X * blockSize + bh, item.Y * blockSize + bh, rad + 2f);

            gfx.FillCircle(brush, item.X * blockSize + bh, item.Y * blockSize + bh, rad);
        }

        private void selectNodesAt(int x, int y)
        {
            float blockSize = (float)(zoomInput.Value);
            x = (int)Math.Floor(x / blockSize);
            y = (int)Math.Floor(y / blockSize);

            entityTableLocked = true;

            List<int> sel = new List<int>();
            foreach (ListViewItem listItem in entityTable.Items)
            {
                EntityItem item = (EntityItem)(listItem.Tag);
                listItem.Selected = (item.X == x && item.Y == y);
                if (listItem.Selected) listItem.EnsureVisible();
                listItem.Focused = listItem.Selected;
            }

            entityTableLocked = false;
            entityTable.Focus();
            draw();
        }

        private void initColors()
        {
            var r = new Random(1134);

            pointColors = new Color[256];
            for (int i = 0; i < 256; i++)
            {
                pointColors[i] = Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            }

            pointColors[0] = Color.IndianRed;
            pointColors[1] = Color.LightBlue;
            pointColors[2] = Color.LightGreen;
            pointColors[3] = Color.Yellow;
            pointColors[4] = Color.Cyan;
            pointColors[5] = Color.Turquoise;
            pointColors[6] = Color.SkyBlue;
            pointColors[7] = Color.Orange;
            pointColors[8] = Color.HotPink;
            pointColors[9] = Color.White;


            lineColors = new Color[256];
            for (int i = 0; i < 256; i++)
            {
                lineColors[i] = Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            }

            lineColors[0] = Color.Red;
            lineColors[1] = Color.LightBlue;
            lineColors[2] = Color.Green;
            lineColors[3] = Color.Yellow;
            lineColors[4] = Color.Cyan;
            lineColors[5] = Color.Turquoise;
            lineColors[6] = Color.SkyBlue;
            lineColors[7] = Color.Orange;
            lineColors[8] = Color.HotPink;
            lineColors[9] = Color.BlueViolet;
        }

        #region Event handlers
        private void zoomInput_ValueChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void entityTableCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void entityTable_MouseUp(object sender, MouseEventArgs e)
        {
            draw();
        }

        private void deselectButton_Click(object sender, EventArgs e)
        {
            entityTable.SelectedIndices.Clear();
            draw();
        }

        private void entityTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            entityTable.ListViewItemSorter = new IntegerComparer(e.Column);
            entityTable.Sort();
        }

        private void connect9Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void mapByteInput_ValueChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void entityTable_KeyUp(object sender, KeyEventArgs e)
        {
            draw();
        }

        private void canvasBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.None)
            {
                infoAt(e.X, e.Y);
            }
        }

        private void infoAt(int x, int y)
        {
            float blockSize = (float)zoomInput.Value;

            int cx = (int)Math.Floor(x / blockSize);
            int cy = (int)Math.Floor(y / blockSize);

            mouseInfoLabel.Text = "Pos: " + cx.ToString() + "/" + cy.ToString() + ", index: " + (cx + cy * 256).ToString();
        }

        private void canvasBox_MouseDown(object sender, MouseEventArgs e)
        {
            infoAt(e.X, e.Y);
        }

        private void canvasBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                selectNodesAt(e.X, e.Y);
            }
        }

        private void dotsizeInput_ValueChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void columnTableCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void blockTexTableCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void columnTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            columnTable.ListViewItemSorter = new IntegerComparer(e.Column);
            columnTable.Sort();
        }

        private void blockTexTable_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            blockTexTable.ListViewItemSorter = new IntegerComparer(e.Column);
            blockTexTable.Sort();
        }

        private void columnTable_MouseUp(object sender, MouseEventArgs e)
        {
            if (highlightColumnsCheckbox.Checked) draw();
        }

        private void columnTable_KeyUp(object sender, KeyEventArgs e)
        {
            if (highlightColumnsCheckbox.Checked) draw();
        }

        private void highlightBlockTexCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void blockTexTable_KeyUp(object sender, KeyEventArgs e)
        {
            draw();
        }

        private void blockTexTable_MouseUp(object sender, MouseEventArgs e)
        {
            draw();
        }

        private void highlightColumnsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            draw();
        }

        private void deselectColumnsButton_Click(object sender, EventArgs e)
        {
            columnTable.SelectedIndices.Clear();
            draw();
        }

        private void deselectBlockTexButton_Click(object sender, EventArgs e)
        {
            blockTexTable.SelectedIndices.Clear();
            draw();
        }

        private void columnTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = columnTable.GetItemAt(e.X, e.Y);
            ListViewItem.ListViewSubItem subItem = item.GetSubItemAt(e.X, e.Y);

            int id = 0;
            if (int.TryParse(subItem.Text, out id) && id > 0 && id < blockTexItems.Count)
            {
                // select entry in block tex table
                foreach (ListViewItem btItem in blockTexTable.Items)
                {
                    if ((btItem.Tag as BlockTexItem).ID == id)
                    {
                        tableTabs.SelectedTab = blockTexTab;
                        blockTexTable.SelectedIndices.Clear();
                        btItem.Selected = true;
                        btItem.Focused = true;
                        btItem.EnsureVisible();
                        blockTexTable.Focus();
                    }
                }

            }
        }

        private void blockTexTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (blockTexTable.SelectedItems.Count == 0) return;

            if (atlas == null)
            {
                MessageBox.Show("Sorry, the atlas is not available.", "You know...");
                return;
            }

            BlockTexItem item = (BlockTexItem)(blockTexTable.SelectedItems[0].Tag);

            // show block texture viewer
            BlockTexForm form = new BlockTexForm(atlas,
                item.Bytes[0],
                item.Bytes[1],
                item.Bytes[2],
                item.Bytes[3],
                item.Bytes[4],
                item.Bytes[5]
            );

            form.Show(this);
        }

        private void writeButton_Click(object sender, EventArgs e)
        {
            Write();
        }

        private void entityTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            editEntityButton.Enabled = entityTable.SelectedItems.Count == 1;
        }

        private void columnTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            editColumnButton.Enabled = columnTable.SelectedItems.Count == 1;
        }


        private void blockTexTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            editBlockTexButton.Enabled = blockTexTable.SelectedItems.Count == 1;

            if (!editBlockTexButton.Enabled)
            {
                cubePreviewBottomPanel.Visible = false;
                cubePreviewTopPanel.Visible = false;
                return;
            }

            if (atlas == null) return;

            cubePreviewBottomPanel.Visible = true;
            cubePreviewTopPanel.Visible = true;

            BlockTexItem item = (BlockTexItem)(blockTexTable.SelectedItems[0].Tag);

            // show block texture viewer
            BlockTexForm.TopView(cubePreviewTopPicture,
                atlas.Get(item.Bytes[0], item.Bytes[6]),
                atlas.Get(item.Bytes[1], item.Bytes[7]),
                atlas.Get(item.Bytes[2], item.Bytes[8]),
                atlas.Get(item.Bytes[3], item.Bytes[9]),
                atlas.Get(item.Bytes[4], item.Bytes[10]),
                atlas.Get(item.Bytes[5], item.Bytes[11])
            );

            BlockTexForm.BottomView(cubePreviewBottomPicture,
                atlas.Get(item.Bytes[0], item.Bytes[6]),
                atlas.Get(item.Bytes[1], item.Bytes[7]),
                atlas.Get(item.Bytes[2], item.Bytes[8]),
                atlas.Get(item.Bytes[3], item.Bytes[9]),
                atlas.Get(item.Bytes[4], item.Bytes[10]),
                atlas.Get(item.Bytes[5], item.Bytes[11])
            );
        }

        private void editEntityButton_Click(object sender, EventArgs e)
        {
            EntityItem item = entityTable.SelectedItems[0].Tag as EntityItem;
            editEntityItem(item);
        }

        private void editEntityItem(EntityItem item)
        {
            EditItemForm f = new EditItemForm(item);
            if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.None) return;

            item.UpdateGameType();
            item.UpdateCaptions();
            EntityTable.Write(entityItems, ref data);
            writeButton.Enabled = true;
        }

        private void editColumnButton_Click(object sender, EventArgs e)
        {
            ColumnItem item = columnTable.SelectedItems[0].Tag as ColumnItem;
            EditItemForm f = new EditItemForm(item);
            if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.None) return;

            item.UpdateCaptions();
            ColumnTable.Write(columnItems, ref data);
            writeButton.Enabled = true;
        }

        private void editBlockTexButton_Click(object sender, EventArgs e)
        {
            BlockTexItem item = blockTexTable.SelectedItems[0].Tag as BlockTexItem;
            EditItemForm f = new EditItemForm(item);
            if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.None) return;

            item.UpdateCaptions();
            BlockTexTable.Write(blockTexItems, ref data);
            writeButton.Enabled = true;
        }

        private void unknownTable247264_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            unknownTable247264.ListViewItemSorter = new IntegerComparer(e.Column);
            unknownTable247264.Sort();
        }

        private void unknownTable358222_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            unknownTable358222.ListViewItemSorter = new IntegerComparer(e.Column);
            unknownTable358222.Sort();
        }

        private void entityTable_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EntityItem item = entityTable.SelectedItems[0].Tag as EntityItem;
            editEntityItem(item);
        }
        #endregion

        private void morphCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            draw();
        }

    }

    public class MapPointOfInterest
    {
        public PointF Position;
        public int Value;
    }

    public class MapCheckpoint
    {
        public PointF Position;
        public int Number;
    }


}
