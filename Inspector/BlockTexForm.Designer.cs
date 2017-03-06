namespace LevelInspector
{
    partial class BlockTexForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BlockTexForm));
            this.northBox = new System.Windows.Forms.PictureBox();
            this.topBox = new System.Windows.Forms.PictureBox();
            this.westBox = new System.Windows.Forms.PictureBox();
            this.bottomBox = new System.Windows.Forms.PictureBox();
            this.eastBox = new System.Windows.Forms.PictureBox();
            this.southBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.idsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.northBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.westBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eastBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.southBox)).BeginInit();
            this.SuspendLayout();
            //
            // northBox
            //
            this.northBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.northBox.Location = new System.Drawing.Point(82, 14);
            this.northBox.Name = "northBox";
            this.northBox.Size = new System.Drawing.Size(64, 64);
            this.northBox.TabIndex = 0;
            this.northBox.TabStop = false;
            //
            // topBox
            //
            this.topBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.topBox.Location = new System.Drawing.Point(82, 84);
            this.topBox.Name = "topBox";
            this.topBox.Size = new System.Drawing.Size(64, 64);
            this.topBox.TabIndex = 0;
            this.topBox.TabStop = false;
            //
            // westBox
            //
            this.westBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.westBox.Location = new System.Drawing.Point(12, 84);
            this.westBox.Name = "westBox";
            this.westBox.Size = new System.Drawing.Size(64, 64);
            this.westBox.TabIndex = 0;
            this.westBox.TabStop = false;
            //
            // bottomBox
            //
            this.bottomBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bottomBox.Location = new System.Drawing.Point(222, 84);
            this.bottomBox.Name = "bottomBox";
            this.bottomBox.Size = new System.Drawing.Size(64, 64);
            this.bottomBox.TabIndex = 0;
            this.bottomBox.TabStop = false;
            //
            // eastBox
            //
            this.eastBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.eastBox.Location = new System.Drawing.Point(152, 84);
            this.eastBox.Name = "eastBox";
            this.eastBox.Size = new System.Drawing.Size(64, 64);
            this.eastBox.TabIndex = 0;
            this.eastBox.TabStop = false;
            //
            // southBox
            //
            this.southBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.southBox.Location = new System.Drawing.Point(82, 154);
            this.southBox.Name = "southBox";
            this.southBox.Size = new System.Drawing.Size(64, 64);
            this.southBox.TabIndex = 0;
            this.southBox.TabStop = false;
            //
            // label1
            //
            this.label1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(184, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 51);
            this.label1.TabIndex = 1;
            this.label1.Text = "  N\r\nW T E B\r\n  S";
            //
            // idsLabel
            //
            this.idsLabel.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idsLabel.Location = new System.Drawing.Point(172, 167);
            this.idsLabel.Name = "idsLabel";
            this.idsLabel.Size = new System.Drawing.Size(114, 51);
            this.idsLabel.TabIndex = 2;
            this.idsLabel.Text = "  N\r\nW T E B\r\n  S";
            //
            // BlockTexViewer
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 231);
            this.Controls.Add(this.idsLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bottomBox);
            this.Controls.Add(this.westBox);
            this.Controls.Add(this.topBox);
            this.Controls.Add(this.southBox);
            this.Controls.Add(this.eastBox);
            this.Controls.Add(this.northBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "BlockTexViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Block Texture Viewer";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.northBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.westBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bottomBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eastBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.southBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox northBox;
        private System.Windows.Forms.PictureBox topBox;
        private System.Windows.Forms.PictureBox westBox;
        private System.Windows.Forms.PictureBox bottomBox;
        private System.Windows.Forms.PictureBox eastBox;
        private System.Windows.Forms.PictureBox southBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label idsLabel;
    }
}
