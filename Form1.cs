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
        public string openWithFile = "";
        public float zoomFactor = 1.0f;
        PictureBox pictureBox2 = new PictureBox();
        PictureBox backdrop = new PictureBox();

        int oldWindowLeft;
        int oldWindowTop;
        float oldX = 0.0f;
        float oldY = 0.0f;
        int offsetX = 0;
        int offsetY = 0;
        System.Windows.Forms.FormWindowState oldWindowState;
        Rectangle oldWindowBounds;
        //Panel panel = new Panel();


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

            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(CheckMouseWheel);


            // custom events
            pictureBox2.DoubleClick += pictureBox2_DoubleClick;

            // setup
            updateNextPrev();
            updateViewList();
        }


        private void CheckMouseWheel(object sender, MouseEventArgs e)
        {
            if (FullScreenMode)
            {
                // mouse wheel up
                if (e.Delta > 0)
                {
                    zoomFactor = 1.1f;
                }

                // mouse wheel down
                if (e.Delta < 0)
                {
                    zoomFactor = 0.9f;
                }

                float oldWidth  = (pictureBox2.Width);
                float oldHeight = (pictureBox2.Height);
                float newWidth  = (oldWidth  * zoomFactor);
                float newHeight = (oldHeight * zoomFactor);

                float offsetX = (MousePosition.X - oldX) * (zoomFactor - 1);
                float offsetY = (MousePosition.Y - oldY) * (zoomFactor - 1);


                pictureBox2.Size = new Size((int)newWidth, (int)newHeight);
                pictureBox2.Location = new Point((int)(oldX - offsetX), (int)(oldY - offsetY));

                oldX = offsetX;
                oldY = offsetY;
            }
        }


        private async Task updateViewList()
        {
            
            listView1.Items.Clear();

            foreach (String s in filteredFiles)
            {
                await Task.Delay(1);
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
                string filepath = openFileDialog1.FileName;
                openFile(filepath);
            }
        }

        public async Task openFile(string filepath)
        {
            Console.WriteLine("opening file : " + filepath);
            pictureBox1.Load(filepath);
            Console.WriteLine("pictureBox1 ImageLocation: " + pictureBox1.ImageLocation);
            folder = System.IO.Path.GetDirectoryName(filepath);
            filteredFiles = getFiles();

            updateFileInfo();
            updateNextPrev();
            updateViewList();

            Console.WriteLine("Info : " + fileInfoText1.Text);
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

        public bool ThumbnailCallback()
        {
            Console.WriteLine("Something went wrong generating a thumbnail!");
            return true;
        }

        private void getThumbnails()
        {
            imageList1.Images.Clear();

            for (var i = 0; i < listView1.Items.Count; i++)
            {
                Console.WriteLine("Generating thumbnail");
                // find the image
                var imageUrl = (folder + "\\" + listView1.Items[i].Text);
                var image = System.Drawing.Image.FromFile(imageUrl);
                
                // generate the thumbnail
                Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image img = image.GetThumbnailImage(thumbnailWidth, thumbnailWidth, callback, new IntPtr());
                
                /*
                Bitmap img = new Bitmap(image);
                Point p = new Point(0);
                Rectangle area = new Rectangle(new Point(0), new Size(Math.Min(image.Width, thumbnailWidth), Math.Min(image.Height, thumbnailHeight)));
                img.Clone(area, img.PixelFormat);
                */

                // add to the image list
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
            saveWindowPosition();

            // hide the flowLayoutPanel1
            flowLayoutPanel1.Hide();
            flowLayoutPanel2.Hide();
            listView1.Hide();
            

            //resize the window
            if (this.WindowState == System.Windows.Forms.FormWindowState.Maximized)
            {
                Console.WriteLine("Window state was maximized.");
                this.WindowState = System.Windows.Forms.FormWindowState.Normal;
            }
            
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            

            this.TopMost = true;


            // get the new window size
            int screenWidth  = Screen.FromControl(pictureBox2).Bounds.Width;
            int screenHeight = Screen.FromControl(pictureBox2).Bounds.Height;


            // draw a backing panel
            this.Controls.Add(backdrop); // needed for the picture box to work
            backdrop.Width = screenWidth;
            backdrop.Height = screenHeight;
            backdrop.BackColor = Color.Black;
            backdrop.Left = 0;
            backdrop.Top = 0;
            backdrop.Visible = true;
            backdrop.Show();
            backdrop.BringToFront();

            // draw the new picturebox
            this.Controls.Add(pictureBox2); // needed for the picture box to work
            pictureBox2.Load(pictureBox1.ImageLocation);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            
            pictureBox2.Size = new Size(screenWidth, screenHeight);
            pictureBox2.Left = 0;
            oldX = pictureBox2.Left;
            oldY = pictureBox2.Top;

            //int verticalOffset = (int)(Math.Round((screenHeight - pictureBox2.Image.Height) * .5)); // this probably won't work if we use zoom mode
            pictureBox2.Top = 0;// verticalOffset;
            pictureBox2.BackColor = Color.Black;
            pictureBox2.Visible = true;
            pictureBox2.BringToFront();
        }

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            exitFullScreenMode();
        }

        private void exitFullScreenMode()
        {
            FullScreenMode = false;
            flowLayoutPanel1.Show();    // show the flowLayoutPanel1
            flowLayoutPanel2.Show();    // show the flowLayoutPanel2
            listView1.Show();           // show the list
            loadWindowState();
            this.Owner = null;
            pictureBox2.Hide();
            backdrop.Hide();
            this.FormBorderStyle = FormBorderStyle.Sizable;

        }

        private void saveWindowPosition()
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
            if (e.KeyChar == (char)Keys.Escape)
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
