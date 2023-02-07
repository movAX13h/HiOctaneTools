using System;
using System.Threading;
using System.Windows.Forms;

namespace ObjectsConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            InitializeComponent();            
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter = "Bullfrog DAT file|*.dat";

            if (dialog.ShowDialog() != DialogResult.OK) return;
            
            ObjectDatFile file = ObjectDatFile.ReadFile(dialog.FileName);

            if (file.Error != "")
            {
                outBox.Text = file.Error;
                return;
            }

            outBox.Text = file.ToWavefrontOBJ();
        }

    }
}
