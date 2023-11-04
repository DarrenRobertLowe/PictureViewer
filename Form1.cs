using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace PictureViewer
{
    public partial class Form1 : Form
    {
        private string folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private string previousFolder = "";

        List<string> filteredFiles = new List<string>(); // used to store the files from the chosen folder
        private int iteration = 0;
        private int thumbnailWidth  = 100;
        private int thumbnailHeight = 100;
        private bool thumbnailsToggleState = true;
        private int tableLayoutPanel1PreviousWidth;
        private bool fullScreenMode = false;
        public string openWithFile = "";
        public float zoomFactor = 1.0f;

        PictureBox fullscreenPictureBox = new PictureBox();
        PictureBox backdrop = new PictureBox();

        int oldWindowLeft;
        int oldWindowTop;
        float oldX = 0.0f;
        float oldY = 0.0f;
        int offsetX = 0;
        int offsetY = 0;
        bool isDragging = false;

        System.Windows.Forms.FormWindowState oldWindowState;
        Rectangle oldWindowBounds;

        
        public Form1()
        {
            InitializeComponent();

            /*
            DialogResult dialogResult = MessageBox.Show("Do you want to use PictureViewer as your default image viewer?", "Associate Files", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                
                //RegistryKey key = Registry.ClassesRoot.CreateSubKey(".jpg");
                //key.SetValue("", "MyApplication");
                //key.CreateSubKey("DefaultIcon").SetValue("", "MyApplication.exe,1");
                //key.CreateSubKey(@"Shell\Open\Command").SetValue("", "\"C:\\MyApplication\\MyApplication.exe\" \"%1\"");
                
                
                
                [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
                public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

                private void RegisterForImages()
                {
                    // Register the application for images
                    SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
                }
                
            }
            else if (dialogResult == DialogResult.No)
            {
            }
            */

            // Set the UserPaint and AllPaintingInWmPaint styles to true
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            imageList1 = new ImageList();
            imageList1.ImageSize = new Size(thumbnailWidth, thumbnailHeight);

            imageList1.ColorDepth = ColorDepth.Depth24Bit;
            tableLayoutPanel1PreviousWidth = tableLayoutPanel1.Width;

            // controls
            this.KeyPreview = true;
            //this.KeyUp += new KeyEventHandler(Form1_KeyUp);

            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(CheckKeys);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(CheckMouseWheel);
            

            // custom events
            fullscreenPictureBox.DoubleClick += fullscreen_DoubleClick;
            fullscreenPictureBox.MouseDown += fullscreen_Clicked;
            fullscreenPictureBox.MouseMove += fullscreen_Moving;
            fullscreenPictureBox.MouseUp += fullscreen_Released;
            //fullscreenPictureBox.KeyUp += fullscreen_KeyPress;
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(fullscreen_PreviewKeyDown);
            this.KeyDown += new KeyEventHandler(fullscreen_KeyDown);

            // setup
            updateNextPrev();
            updateViewList();
        }

        private void CheckMouseWheel(object sender, MouseEventArgs mouse)
        {
            if (fullScreenMode)
            {
                // mouse wheel up
                if (mouse.Delta > 0)
                {
                    zoomFactor = 1.1f;
                }

                // mouse wheel down
                if (mouse.Delta < 0)
                {
                    zoomFactor = 0.9f;
                }

                float newWidth  = (fullscreenPictureBox.Width * zoomFactor);
                float newHeight = (fullscreenPictureBox.Height * zoomFactor);

                fullscreenPictureBox.Size = new Size((int)newWidth, (int)newHeight);
                fullscreenPictureBox.Left = (int)(mouse.X - zoomFactor * (mouse.X - fullscreenPictureBox.Left));
                fullscreenPictureBox.Top  = (int)(mouse.Y - zoomFactor * (mouse.Y - fullscreenPictureBox.Top));
            }
        }


        // Image dragging while zoomed
        private void fullscreen_Clicked(object sender, MouseEventArgs mouse)
        {
            offsetX = mouse.X;
            offsetY = mouse.Y;
            isDragging = true;
        }

        private void fullscreen_Moving(object sender, MouseEventArgs mouse)
        {
            
            if (isDragging)
            {
                int newX = fullscreenPictureBox.Left + mouse.X - offsetX;
                int newY = fullscreenPictureBox.Top  + mouse.Y - offsetY;
                fullscreenPictureBox.Left = newX;
                fullscreenPictureBox.Top = newY;
            }
        }

        private void fullscreen_Released(object sender, MouseEventArgs mouse)
        {
            isDragging = false;
        }


        // update the list of items
        private async Task updateViewList()
        {
            if (folder != previousFolder)
            {
                listView1.Items.Clear();

                foreach (String s in filteredFiles)
                {
                    await Task.Delay(1);
                    string filename = System.IO.Path.GetFileName(s);
                    listView1.Items.Add(filename);
                }

                getThumbnails();
                previousFolder = folder;
            } 
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

        private Image ResizeImage(string imageUrl, int thumbnailWidth, int thumbnailHeight)
        {
            var image = System.Drawing.Image.FromFile(imageUrl);

            double ratio    = Math.Min((double)thumbnailWidth / image.Width, (double)thumbnailHeight / image.Height);
            int newWidth    = (int)(image.Width * ratio);
            int newHeight   = (int)(image.Height * ratio);
            Bitmap thumbnail = new Bitmap(thumbnailWidth, thumbnailHeight);

            using (Graphics graphic = Graphics.FromImage(thumbnail))
            {
                graphic.Clear(Color.White);
                graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
                graphic.DrawImage(image, (thumbnailWidth - newWidth) / 2, (thumbnailHeight - newHeight) / 2, newWidth, newHeight);
            }

            return thumbnail;
        }

        private void getThumbnails()
        {
            imageList1.Images.Clear();

            for (var i = 0; i < listView1.Items.Count; i++)
            {
                // find the image
                var imageUrl = (folder + "\\" + listView1.Items[i].Text);
                var image = System.Drawing.Image.FromFile(imageUrl);
                
                // generate the thumbnail
                Image.GetThumbnailImageAbort callback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image img = ResizeImage(imageUrl, thumbnailWidth, thumbnailHeight);

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
            fullScreenMode = true;

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
            int screenWidth  = Screen.FromControl(fullscreenPictureBox).Bounds.Width;
            int screenHeight = Screen.FromControl(fullscreenPictureBox).Bounds.Height;


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
            this.Controls.Add(fullscreenPictureBox); // needed for the picture box to work
            fullscreenPictureBox.Load(pictureBox1.ImageLocation);
            fullscreenPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            
            fullscreenPictureBox.Size = new Size(screenWidth, screenHeight);
            fullscreenPictureBox.Left = 0;
            oldX = fullscreenPictureBox.Left;
            oldY = fullscreenPictureBox.Top;

            //int verticalOffset = (int)(Math.Round((screenHeight - pictureBox2.Image.Height) * .5)); // this probably won't work if we use zoom mode
            fullscreenPictureBox.Top = 0;// verticalOffset;
            fullscreenPictureBox.BackColor = Color.Black;
            fullscreenPictureBox.Visible = true;
            fullscreenPictureBox.BringToFront();
        }

        private void fullscreen_DoubleClick(object sender, EventArgs e)
        {
            exitFullScreenMode();
        }

        private void exitFullScreenMode()
        {
            fullScreenMode = false;
            flowLayoutPanel1.Show();    // show the flowLayoutPanel1
            flowLayoutPanel2.Show();    // show the flowLayoutPanel2
            listView1.Show();           // show the list
            loadWindowState();
            this.Owner = null;
            fullscreenPictureBox.Hide();
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


        private void CheckKeys(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                if (fullScreenMode)
                {
                    exitFullScreenMode();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }




        // PreviewKeyDown is where you preview the key.
        // Do not put any logic here, instead use the
        // KeyDown event after setting IsInputKey to true.
        private void fullscreen_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                case Keys.Up:
                case Keys.Left:
                case Keys.Right:
                    e.IsInputKey = true;
                    break;
            }
        }

        // By default, KeyDown does not fire for the ARROW keys
        void fullscreen_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    fullscreenPictureBox.Top -= 10;
                    break;
                case Keys.Up:
                    fullscreenPictureBox.Top += 10;
                    break;
                case Keys.Left:
                    fullscreenPictureBox.Left += 10;
                    break;
                case Keys.Right:
                    fullscreenPictureBox.Left -= 10;
                    break;
            }
        }
    }

    /*
    public class DoubleBufferedPictureBox : PictureBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            // Create a new bitmap and graphics object
            Bitmap buffer = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(buffer);

            // Call the base OnPaint method to draw the image to the bitmap
            base.OnPaint(new PaintEventArgs(g, this.ClientRectangle));

            // Draw the bitmap to the screen
            e.Graphics.DrawImage(buffer, 0, 0);

            // Dispose of the bitmap and graphics object
            buffer.Dispose();
            g.Dispose();
        }
    }
    */
}


