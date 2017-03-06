using LevelInspector.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LevelInspector
{
    public partial class EditItemForm : Form
    {
        private TableItem item;
        private NumericUpDown[] inputs;

        public EditItemForm(TableItem targetItem)
        {
            item = targetItem;
            InitializeComponent();

            inputs = new NumericUpDown[item.Bytes.Length];

            for(int i = 0; i < item.Bytes.Length; i++)
            {
                addByteInput(i, item.Bytes[i]);
            }

            Width = 28 + 60 * 12;
            Height = 80 + 60 * (int)Math.Floor(item.Bytes.Length / 12f);
        }

        private void addByteInput(int i, byte value)
        {
            int x = 10 + 60 * (i % 12);
            int y = 10 + 50 * (int)Math.Floor(i / 12f);

            Label l = new Label();
            l.Text = "b" + i.ToString();
            l.AutoSize = true;
            l.Left = x - 1;
            l.Top = y;
            l.Parent = this;

            NumericUpDown c = new NumericUpDown();
            c.Maximum = 255;
            c.Minimum = 0;
            c.Width = 50;
            c.Left = x;
            c.Top = y + 16;
            c.Value = value;
            c.Parent = this;

            inputs[i] = c;
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach(NumericUpDown c in inputs)
            {
                item.Bytes[i] = (byte)c.Value;
                i++;
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }


    }
}
