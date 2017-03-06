using LevelInspector.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelInspector
{
    public partial class MainForm : Form
    {
        private MapForm mapForm;
        private string mapsDir = "maps/";
        private string imagesDir = "images/";

        private int widthA;
        private int widthB = 150;

        public MainForm()
        {
            InitializeComponent();

            widthA = Width;
            Width = widthB;
            operationSelect.SelectedIndex = 0;
            TopMost = stayOnTopCheckbox.Checked;
            Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width - 30;
        }


        private void modifyLevel(string inFile, string outFile)
        {
            if (!File.Exists(outFile))
            {
                MessageBox.Show("Target file does not exist.", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            byte[] bytes = File.ReadAllBytes(inFile);

            switch(operationSelect.Text)
            {
                case "set byte":
                    bytes[(int)offsetInput.Value] = (byte)valueInput.Value;
                    break;

                case "copy file":
                default:
                    break;
            }

            File.WriteAllBytes(outFile, bytes);
            outputBox.Text = bytes.Length + " bytes written to " + outFile;
        }

        /*
        private void setMapByte(ref byte[] bytes, int offset, byte value)
        {
            // 404620 = total offset in all hi-octane level data files
            int mapWidth = 256;
            int mapHeight = 160;

            // entry is 12 bytes long, map is at end of file
            int numBytes = 12 * mapWidth * mapHeight;

            int i = bytes.Length - numBytes;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    bytes[i + offset] = value;
                    i += 12;
                }
            }
        }*/

        /*
        private void setMapShort(ref byte[] bytes, int offset, int value)
        {
            if (offset > 10)
            {
                MessageBox.Show("Offset is too big.");
                return;
            }

            // 404620 = total offset in all hi-octane level data files
            int mapWidth = 256;
            int mapHeight = 160;

            // entry is 12 bytes long, map is at end of file
            int numBytes = 12 * mapWidth * mapHeight;

            int i = bytes.Length - numBytes;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    byte b0 = (byte)value,
                         b1 = (byte)(value >> 8);

                    bytes[i + offset] = b0;
                    bytes[i + offset + 1] = b1;
                    i += 12;
                }
            }
        }*/

        private void modifyButton_Click(object sender, EventArgs e)
        {
            string level = ((Button)sender).Text;
            string file = mapsDir + level;
            modifyLevel(file, Path.Combine(targetLevelFileInput.Text, level));
        }

        private void loadMapButton_Click(object sender, EventArgs e)
        {
            string file = ((Button)sender).Text;
            showMap(file);
        }

        private void showMap(string filename)
        {
            byte[] bytes = File.ReadAllBytes(mapsDir + filename);

            Atlas atlas = null;

            string atlasFile = Path.GetFileNameWithoutExtension(filename);
            if (atlasFile == "level0-7") atlasFile = "level0-1"; // hardcoded in original game

            string textureFile = imagesDir + atlasFile + ".png";
            if (File.Exists(textureFile)) atlas = new Atlas(textureFile);

            Dictionary<uint, string> MapNames = new Dictionary<uint,string>();
            MapNames.Add(3308913189, "Amazon Delta Turnpike");
            MapNames.Add(2028380229, "Trans-Asia Interstate");
            MapNames.Add(3087166776, "Shanghai Dragon");
            MapNames.Add(1401140937, "New Chernobyl Central");
            MapNames.Add(4215346212, "Slam Canyon");
            MapNames.Add(3809451489, "Thrak City");
            MapNames.Add(3062313907, "Ancient Mine Town");
            MapNames.Add(3081898808, "Arctic Land");
            MapNames.Add(540505443, "Death Match Arena");

            Crc32 crc = new Crc32();
            uint cs = crc.ComputeChecksum(bytes);

            mapForm = new MapForm(ref bytes, atlas, Path.Combine(targetLevelFileInput.Text, filename));
            mapForm.Text = "Hi-Octane level inspector - " + Path.GetFileNameWithoutExtension(filename);

            if (MapNames.ContainsKey(cs)) mapForm.Text += " - " + MapNames[cs];
            else mapForm.Text += " - unidentified";

            //mapForm.FormClosed += mapForm_FormClosed;
            mapForm.Show();
        }

        private void operationSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            setBytePanel.Visible = false;

            switch(operationSelect.Text)
            {
                case "set byte": setBytePanel.Visible = true; break;
                case "copy file":
                default:
                    break;
            }
        }

        private void stayOnTopCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = stayOnTopCheckbox.Checked;
        }

        private void showOutputCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Width = showOutputCheckbox.Checked ? widthA : widthB;
        }

        private void winRightButton_Click(object sender, EventArgs e)
        {
            this.Left = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Right - this.Width - 30;
        }

        private void winLeftButton_Click(object sender, EventArgs e)
        {
            this.Left = 0;
        }

    }
}
