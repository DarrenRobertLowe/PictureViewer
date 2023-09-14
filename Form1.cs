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

        


        public Form1()
        {
            InitializeComponent();
            updateNextPrev();
            updateViewList();
        }

        private void updateViewList()
        {
            foreach (String s in filteredFiles)
            {
                string filename = System.IO.Path.GetFileName(s);
                listView1.Items.Add(filename);
            }
        }



        /// <summary>
        /// enables/disables the next and previous buttons depending on file count in folder
        /// </summary>
        private void updateNextPrev() 
        {
            if (filteredFiles.Count <= 1) 
            {
                nextImageButton.Enabled     = false;
                previousImageButton.Enabled = false;
            }
            else 
            {
                nextImageButton.Enabled     = true;
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

        private void openImage_Click(object sender, EventArgs e)  // select an image
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                folder = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);
                filteredFiles = getFiles();
                updateNextPrev();
                updateViewList();
            }
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

        private void closeProgram_Click(object sender, EventArgs e) // close
        {
            this.Close();
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

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                string selected = listView1.SelectedItems[0].Text;
                pictureBox1.Load(folder + "\\" + selected);
            }
        }
    }
}
