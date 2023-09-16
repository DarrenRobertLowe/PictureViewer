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


        public Form1()
        {
            InitializeComponent();

            //pictureBox1.WaitOnLoad = false;
            imageList1 = new ImageList();
            imageList1.ImageSize = new Size(thumbnailWidth, thumbnailHeight);

            imageList1.ColorDepth = ColorDepth.Depth24Bit;
            tableLayoutPanel1PreviousWidth = tableLayoutPanel1.Width;

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

                //tableLayoutPanel1PreviousWidth = tableLayoutPanel1.Width;
                //tableLayoutPanel1.ColumnStyles[1].Width = 0;//Hide();
                thumbnailsToggleState = false;
                ToggleThumbnailsButton.Text = ">";
                Console.WriteLine("Hidden");
            }
            else
            {
                //tableLayoutPanel1.ColumnStyles[1].Width = tableLayoutPanel1PreviousWidth;//Show();
                thumbnailsToggleState = true;
                ToggleThumbnailsButton.Text = "<";
                Console.WriteLine("Visible");
            }
            
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Picture has been clicked. Now would be a good time to full screen it.");
        }


        /*
        private void pictureBox1_LoadCompleted(Object sender, AsyncCompletedEventArgs e)
        {
            updateFileInfo(); 
        }
        */
    }
}
