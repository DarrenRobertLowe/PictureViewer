
namespace PictureViewer
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.nextImageButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.previousImageButton = new System.Windows.Forms.Button();
            this.fileInfoText1 = new System.Windows.Forms.RichTextBox();
            this.openButton = new System.Windows.Forms.Button();
            this.listViewControlPanel = new System.Windows.Forms.Panel();
            this.ToggleThumbnailsButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.listViewControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AllowDrop = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.listViewControlPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1045, 626);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(159, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(883, 557);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(150, 557);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AllowDrop = true;
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.762887F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 91.23711F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 223F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 87F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 91F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel2.Controls.Add(this.nextImageButton, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.checkBox1, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.previousImageButton, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.fileInfoText1, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.openButton, 5, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(159, 566);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(883, 57);
            this.tableLayoutPanel2.TabIndex = 9;
            this.tableLayoutPanel2.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel2_Paint);
            // 
            // nextImageButton
            // 
            this.nextImageButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.nextImageButton.Location = new System.Drawing.Point(705, 3);
            this.nextImageButton.Name = "nextImageButton";
            this.nextImageButton.Size = new System.Drawing.Size(75, 23);
            this.nextImageButton.TabIndex = 7;
            this.nextImageButton.Text = "Next";
            this.nextImageButton.UseVisualStyleBackColor = true;
            this.nextImageButton.Click += new System.EventHandler(this.nextImageButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(257, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(127, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Fit Image To Window";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // previousImageButton
            // 
            this.previousImageButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.previousImageButton.Location = new System.Drawing.Point(616, 3);
            this.previousImageButton.Name = "previousImageButton";
            this.previousImageButton.Size = new System.Drawing.Size(75, 23);
            this.previousImageButton.TabIndex = 6;
            this.previousImageButton.Text = "Previous";
            this.previousImageButton.UseVisualStyleBackColor = true;
            this.previousImageButton.Click += new System.EventHandler(this.previousImageButton_Click);
            // 
            // fileInfoText1
            // 
            this.fileInfoText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fileInfoText1.Location = new System.Drawing.Point(390, 3);
            this.fileInfoText1.Name = "fileInfoText1";
            this.fileInfoText1.Size = new System.Drawing.Size(217, 51);
            this.fileInfoText1.TabIndex = 8;
            this.fileInfoText1.Text = "";
            // 
            // openButton
            // 
            this.openButton.AllowDrop = true;
            this.openButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.openButton.AutoSize = true;
            this.openButton.Location = new System.Drawing.Point(798, 3);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(75, 23);
            this.openButton.TabIndex = 0;
            this.openButton.Text = "Open";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openImage_Click);
            // 
            // listViewControlPanel
            // 
            this.listViewControlPanel.Controls.Add(this.ToggleThumbnailsButton);
            this.listViewControlPanel.Location = new System.Drawing.Point(3, 566);
            this.listViewControlPanel.Name = "listViewControlPanel";
            this.listViewControlPanel.Size = new System.Drawing.Size(150, 57);
            this.listViewControlPanel.TabIndex = 10;
            // 
            // ToggleThumbnailsButton
            // 
            this.ToggleThumbnailsButton.AutoSize = true;
            this.ToggleThumbnailsButton.Location = new System.Drawing.Point(0, 3);
            this.ToggleThumbnailsButton.Name = "ToggleThumbnailsButton";
            this.ToggleThumbnailsButton.Size = new System.Drawing.Size(131, 23);
            this.ToggleThumbnailsButton.TabIndex = 5;
            this.ToggleThumbnailsButton.Text = "<< Hide Folder Contents";
            this.ToggleThumbnailsButton.UseVisualStyleBackColor = true;
            this.ToggleThumbnailsButton.Click += new System.EventHandler(this.ToggleThumbnailsButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "All files (*.*)|*.*|JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files " +
    "(*.png)|*.png|BMP Files (*.bmp)|*.bmp";
            this.openFileDialog1.Title = "Select a picture file";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 626);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Picture Viewer";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.listViewControlPanel.ResumeLayout(false);
            this.listViewControlPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button previousImageButton;
        private System.Windows.Forms.Button nextImageButton;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.RichTextBox fileInfoText1;
        private System.Windows.Forms.Button ToggleThumbnailsButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel listViewControlPanel;
    }
}

