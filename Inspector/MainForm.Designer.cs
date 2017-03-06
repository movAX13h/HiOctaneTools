namespace LevelInspector
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.targetLevelFileInput = new System.Windows.Forms.TextBox();
            this.modifyButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.loadMapButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.offsetInput = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.valueInput = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.setBytePanel = new System.Windows.Forms.Panel();
            this.operationSelect = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.stayOnTopCheckbox = new System.Windows.Forms.CheckBox();
            this.winRightButton = new System.Windows.Forms.Button();
            this.winLeftButton = new System.Windows.Forms.Button();
            this.showOutputCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.offsetInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueInput)).BeginInit();
            this.setBytePanel.SuspendLayout();
            this.SuspendLayout();
            //
            // targetLevelFileInput
            //
            this.targetLevelFileInput.Location = new System.Drawing.Point(154, 38);
            this.targetLevelFileInput.Name = "targetLevelFileInput";
            this.targetLevelFileInput.Size = new System.Drawing.Size(225, 20);
            this.targetLevelFileInput.TabIndex = 0;
            this.targetLevelFileInput.Text = "E:\\Software\\D-Fend Reloaded\\VirtualHD\\HiOctane12\\MAPS";
            //
            // modifyButton
            //
            this.modifyButton.Location = new System.Drawing.Point(386, 37);
            this.modifyButton.Name = "modifyButton";
            this.modifyButton.Size = new System.Drawing.Size(100, 33);
            this.modifyButton.TabIndex = 2;
            this.modifyButton.Text = "level0-1.dat";
            this.modifyButton.UseVisualStyleBackColor = true;
            this.modifyButton.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(150, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "OUTPUT DIRECTORY";
            //
            // outputBox
            //
            this.outputBox.Location = new System.Drawing.Point(153, 286);
            this.outputBox.Multiline = true;
            this.outputBox.Name = "outputBox";
            this.outputBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.outputBox.Size = new System.Drawing.Size(225, 111);
            this.outputBox.TabIndex = 5;
            //
            // loadMapButton
            //
            this.loadMapButton.Location = new System.Drawing.Point(23, 37);
            this.loadMapButton.Name = "loadMapButton";
            this.loadMapButton.Size = new System.Drawing.Size(100, 33);
            this.loadMapButton.TabIndex = 6;
            this.loadMapButton.Text = "level0-1.dat";
            this.loadMapButton.UseVisualStyleBackColor = true;
            this.loadMapButton.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button1
            //
            this.button1.Location = new System.Drawing.Point(23, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 33);
            this.button1.TabIndex = 7;
            this.button1.Text = "level0-2.dat";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(383, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "APPLY TO";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "INSPECT";
            //
            // button2
            //
            this.button2.Location = new System.Drawing.Point(23, 109);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 33);
            this.button2.TabIndex = 10;
            this.button2.Text = "level0-3.dat";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button3
            //
            this.button3.Location = new System.Drawing.Point(23, 145);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(100, 33);
            this.button3.TabIndex = 11;
            this.button3.Text = "level0-4.dat";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button4
            //
            this.button4.Location = new System.Drawing.Point(23, 181);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 33);
            this.button4.TabIndex = 12;
            this.button4.Text = "level0-5.dat";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button5
            //
            this.button5.Location = new System.Drawing.Point(23, 217);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(100, 33);
            this.button5.TabIndex = 13;
            this.button5.Text = "level0-6.dat";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button6
            //
            this.button6.Location = new System.Drawing.Point(386, 73);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(100, 33);
            this.button6.TabIndex = 2;
            this.button6.Text = "level0-2.dat";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // button7
            //
            this.button7.Location = new System.Drawing.Point(386, 109);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(100, 33);
            this.button7.TabIndex = 2;
            this.button7.Text = "level0-3.dat";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // button8
            //
            this.button8.Location = new System.Drawing.Point(386, 145);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(100, 33);
            this.button8.TabIndex = 2;
            this.button8.Text = "level0-4.dat";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // button9
            //
            this.button9.Location = new System.Drawing.Point(386, 181);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(100, 33);
            this.button9.TabIndex = 2;
            this.button9.Text = "level0-5.dat";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // button10
            //
            this.button10.Location = new System.Drawing.Point(386, 217);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(100, 33);
            this.button10.TabIndex = 2;
            this.button10.Text = "level0-6.dat";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // button11
            //
            this.button11.Location = new System.Drawing.Point(23, 253);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(100, 33);
            this.button11.TabIndex = 13;
            this.button11.Text = "level0-7.dat";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button12
            //
            this.button12.Location = new System.Drawing.Point(23, 289);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(100, 33);
            this.button12.TabIndex = 13;
            this.button12.Text = "level0-8.dat";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button13
            //
            this.button13.Location = new System.Drawing.Point(23, 325);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(100, 33);
            this.button13.TabIndex = 13;
            this.button13.Text = "level0-9.dat";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.loadMapButton_Click);
            //
            // button14
            //
            this.button14.Location = new System.Drawing.Point(386, 253);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(100, 33);
            this.button14.TabIndex = 2;
            this.button14.Text = "level0-7.dat";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // button15
            //
            this.button15.Location = new System.Drawing.Point(386, 289);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(100, 33);
            this.button15.TabIndex = 2;
            this.button15.Text = "level0-8.dat";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // button16
            //
            this.button16.Location = new System.Drawing.Point(386, 325);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(100, 33);
            this.button16.TabIndex = 2;
            this.button16.Text = "level0-9.dat";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.modifyButton_Click);
            //
            // offsetInput
            //
            this.offsetInput.Location = new System.Drawing.Point(14, 27);
            this.offsetInput.Maximum = new decimal(new int[] {
            896139,
            0,
            0,
            0});
            this.offsetInput.Name = "offsetInput";
            this.offsetInput.Size = new System.Drawing.Size(120, 20);
            this.offsetInput.TabIndex = 14;
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "offset";
            //
            // valueInput
            //
            this.valueInput.Location = new System.Drawing.Point(14, 67);
            this.valueInput.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.valueInput.Name = "valueInput";
            this.valueInput.Size = new System.Drawing.Size(120, 20);
            this.valueInput.TabIndex = 15;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(11, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "value";
            //
            // setBytePanel
            //
            this.setBytePanel.Controls.Add(this.valueInput);
            this.setBytePanel.Controls.Add(this.label5);
            this.setBytePanel.Controls.Add(this.offsetInput);
            this.setBytePanel.Controls.Add(this.label4);
            this.setBytePanel.Location = new System.Drawing.Point(155, 115);
            this.setBytePanel.Name = "setBytePanel";
            this.setBytePanel.Size = new System.Drawing.Size(225, 165);
            this.setBytePanel.TabIndex = 16;
            //
            // operationSelect
            //
            this.operationSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.operationSelect.FormattingEnabled = true;
            this.operationSelect.Items.AddRange(new object[] {
            "copy file",
            "set byte"});
            this.operationSelect.Location = new System.Drawing.Point(154, 88);
            this.operationSelect.Name = "operationSelect";
            this.operationSelect.Size = new System.Drawing.Size(226, 21);
            this.operationSelect.TabIndex = 17;
            this.operationSelect.SelectedIndexChanged += new System.EventHandler(this.operationSelect_SelectedIndexChanged);
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(151, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "OPERATION";
            //
            // stayOnTopCheckbox
            //
            this.stayOnTopCheckbox.AutoSize = true;
            this.stayOnTopCheckbox.Checked = true;
            this.stayOnTopCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stayOnTopCheckbox.Location = new System.Drawing.Point(71, 364);
            this.stayOnTopCheckbox.Name = "stayOnTopCheckbox";
            this.stayOnTopCheckbox.Size = new System.Drawing.Size(63, 17);
            this.stayOnTopCheckbox.TabIndex = 18;
            this.stayOnTopCheckbox.Text = "topmost";
            this.stayOnTopCheckbox.UseVisualStyleBackColor = true;
            this.stayOnTopCheckbox.CheckedChanged += new System.EventHandler(this.stayOnTopCheckbox_CheckedChanged);
            //
            // winRightButton
            //
            this.winRightButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.winRightButton.Location = new System.Drawing.Point(47, 364);
            this.winRightButton.Name = "winRightButton";
            this.winRightButton.Size = new System.Drawing.Size(18, 33);
            this.winRightButton.TabIndex = 19;
            this.winRightButton.Text = "❯";
            this.winRightButton.UseVisualStyleBackColor = true;
            this.winRightButton.Click += new System.EventHandler(this.winRightButton_Click);
            //
            // winLeftButton
            //
            this.winLeftButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.winLeftButton.Location = new System.Drawing.Point(23, 364);
            this.winLeftButton.Name = "winLeftButton";
            this.winLeftButton.Size = new System.Drawing.Size(18, 33);
            this.winLeftButton.TabIndex = 20;
            this.winLeftButton.Text = "❮";
            this.winLeftButton.UseVisualStyleBackColor = true;
            this.winLeftButton.Click += new System.EventHandler(this.winLeftButton_Click);
            //
            // showOutputCheckbox
            //
            this.showOutputCheckbox.AutoSize = true;
            this.showOutputCheckbox.Location = new System.Drawing.Point(71, 382);
            this.showOutputCheckbox.Name = "showOutputCheckbox";
            this.showOutputCheckbox.Size = new System.Drawing.Size(56, 17);
            this.showOutputCheckbox.TabIndex = 21;
            this.showOutputCheckbox.Text = "output";
            this.showOutputCheckbox.UseVisualStyleBackColor = true;
            this.showOutputCheckbox.CheckedChanged += new System.EventHandler(this.showOutputCheckbox_CheckedChanged);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 421);
            this.Controls.Add(this.showOutputCheckbox);
            this.Controls.Add(this.winLeftButton);
            this.Controls.Add(this.winRightButton);
            this.Controls.Add(this.stayOnTopCheckbox);
            this.Controls.Add(this.operationSelect);
            this.Controls.Add(this.setBytePanel);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.loadMapButton);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.modifyButton);
            this.Controls.Add(this.targetLevelFileInput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 200);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Hi-Octane level data file inspector";
            ((System.ComponentModel.ISupportInitialize)(this.offsetInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valueInput)).EndInit();
            this.setBytePanel.ResumeLayout(false);
            this.setBytePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox targetLevelFileInput;
        private System.Windows.Forms.Button modifyButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.Button loadMapButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.NumericUpDown offsetInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown valueInput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel setBytePanel;
        private System.Windows.Forms.ComboBox operationSelect;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox stayOnTopCheckbox;
        private System.Windows.Forms.Button winRightButton;
        private System.Windows.Forms.Button winLeftButton;
        private System.Windows.Forms.CheckBox showOutputCheckbox;
    }
}

