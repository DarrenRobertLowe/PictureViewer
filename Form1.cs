using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureViewer
{
    public partial class Form1 : Form
    {
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        List<string> filteredFiles = new List<string>(); // used to store the files from the chosen folder
        private int iteration = 0;
        private int thumbnailWidth  = 128;
        private int thumbnailHeight = 96;
        private bool thumbnailsToggleState = true;
        private int tableLayoutPanel1PreviousWidth;
        private bool FullScreenMode = false;
        PictureBox pictureBox2 = new PictureBox();

        int oldWindowLeft;
        int oldWindowTop;
        System.Windows.Forms.FormWindowState oldWindowState;
        Rectangle oldWindowBounds;

        public Form1()
        {
            InitializeComponent();
             
            imageList1 = new ImageList();
            imageList1.ImageSize = new Size(thumbnailWidth, thumbnailHeight);

            imageList1.ColorDepth = ColorDepth.Depth24Bit;
            tableLayoutPanel1PreviousWidth = tableLayoutPanel1.Width;

            // allow keyboard controls
            this.KeyPreview = true;
            //this.KeyUp += new KeyEventHandler(Form1_KeyUp);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckEscape);

            // custom events
            pictureBox2.DoubleClick += pictureBox2_DoubleClick;

            // setup
            updateNextPrev();
            updateViewList();
        }

        private void updateViewList()
        {
            listView1.Items.Clear();
            foreach (String s in filteredFiles)
            {
                string filename = System.IO.Path.GetFileName(s);
                listView1.Items.Add(filename);
            }

            getThumbnails();
        }



        /// <summary>
        /// enables/disables the next and previous buttons depending on file count in folder
        /// </summary>
        private void updateNextPrev()
        {
            if (filteredFiles.Count <= 1)
            {
                nextImageButton.Enabled = false;
                previousImageButton.Enabled = false;
            }
            else
            {
                nextImageButton.Enabled = true;
                previousImageButton.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // stretch toggle
        {
            if (pictureBox1.SizeMode == PictureBoxSizeMode.Normal)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
            }

        }

        private void openImage_Click(object sender, EventArgs e)  // open an image
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                folder = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                filteredFiles = getFiles();

                updateFileInfo();
                updateNextPrev();
                updateViewList();
            }
        }

        private void updateFileInfo()
        {
            fileInfoText1.Text = "File: " + pictureBox1.ImageLocation;
            fileInfoText1.Text += "\n";
            fileInfoText1.Text += "Size: " + pictureBox1.Image.Width + " x " + pictureBox1.Image.Height;
            fileInfoText1.Text += "\n";
            fileInfoText1.Text += "PPI: " + pictureBox1.Image.HorizontalResolution;// +" x " + pictureBox1.Image.VerticalResolution;
            fileInfoText1.Text += "\n";
        }


        private void clearImage_Click(object sender, EventArgs e) // clear the picture
        {
            pictureBox1.Image = null;
        }

        private void setBackground_Click(object sender, EventArgs e) // set the background color
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;
            }
        }

        /*
        private void closeProgram_Click(object sender, EventArgs e) // close
        {
            this.Close();
        }
        */


        /// <summary>
        /// Iterates backwards through the filteredFiles list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void previousImageButton_Click(object sender, EventArgs e) // previous image
        {
            // iterate backwards
            iteration--;
            if (iteration < 0)
            {
                iteration = filteredFiles.Count - 1; // loop around
            }

            String file = filteredFiles[iteration];
            pictureBox1.Load(file);
            updateFileInfo();
        }

        /// <summary>
        /// Iterates forwards through the filteredFiles list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextImageButton_Click(object sender, EventArgs e)
        {
            // iterate forwards
            iteration++;
            if (iteration >= filteredFiles.Count)
            {
                iteration = 0; // loop around
            }

            String file = filteredFiles[iteration];
            pictureBox1.Load(file);
            updateFileInfo();
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        /// <summary>
        /// Creates a list of files from the selected folder.
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private List<string> getFiles()
        {
            filteredFiles.Clear();
            System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(folder);

            var allFilesList = System.IO.Directory.GetFiles(folder, "*.*")
            .Where(file => file.ToLower().EndsWith(".png") || file.ToLower().EndsWith(".gif") || file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".jpeg") || file.ToLower().EndsWith(".bmp"))
            .ToList();

            allFilesList.ToString();

            return allFilesList;
        }

        private void getThumbnails()
        {
            imageList1.Images.Clear();

            for (var i = 0; i < listView1.Items.Count; i++)
            {
                var imageUrl = (folder + "\\" + listView1.Items[i].Text);
                var image = System.Drawing.Image.FromFile(imageUrl);

                Bitmap img = new Bitmap(image);
                Point p = new Point(0);
                Rectangle area = new Rectangle(new Point(0), new Size(Math.Min(image.Width, thumbnailWidth), Math.Min(image.Height, thumbnailHeight)));
                img.Clone(area, img.PixelFormat);

                imageList1.Images.Add("" + i, img);
            }

            listView1.LargeImageList = imageList1;

            for (var i = 0; i < listView1.Items.Count; i++)
            {
                var file = listView1.Items[i];
                file.ImageIndex = i;
            }


        }



        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selected = listView1.SelectedItems[0].Text;
                pictureBox1.Load(folder + "\\" + selected);
                updateFileInfo();
            }
        }

        private void ToggleThumbnailsButton_Click(object sender, EventArgs e)
        {
            if (thumbnailsToggleState == true)
            {
                thumbnailsToggleState = false;
                ToggleThumbnailsButton.Text = ">";
                Console.WriteLine("Hidden");
            }
            else
            {
                thumbnailsToggleState = true;
                ToggleThumbnailsButton.Text = "<";
                Console.WriteLine("Visible");
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                enterFullScreenMode();
            }
        }

        private void enterFullScreenMode()
        {
            FullScreenMode = true;

            // get the window state
            getWindowState();

            //resize the window
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.GetBounds(this);

            // draw the new picturebox
            Form form2 = new Form();
            this.Owner = form2;
            this.Controls.Add(pictureBox2); // needed for the picture box to work
            pictureBox2.Load(pictureBox1.ImageLocation);
            pictureBox2.Left = 0;
            pictureBox2.Top = 0;
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.Visible = true;
            pictureBox2.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            pictureBox2.BackColor = Color.Black;
            pictureBox2.BringToFront();
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            exitFullScreenMode();
        }

        private void exitFullScreenMode()
        {
            FullScreenMode = false;
            loadWindowState();
            this.Owner = null;
            pictureBox2.Hide();
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }

        private void getWindowState()
        {
            oldWindowLeft   = this.Left;
            oldWindowTop    = this.Top;
            oldWindowState  = this.WindowState;
            oldWindowBounds = this.Bounds;
        }

        private void loadWindowState()
        {
            this.Left = oldWindowLeft;
            this.Top = oldWindowTop;
            this.WindowState = oldWindowState;
            this.Bounds = oldWindowBounds;
        }


        private void pictureBox1_LoadProgressChanged(Object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("load completed!");
            updateFileInfo(); 
        }


        private void CheckEscape(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)27)
            {
                if (FullScreenMode)
                {
                    exitFullScreenMode();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }


    }
}
