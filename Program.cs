using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureViewer
{
    static class Program
    {
        static string openFile;
        static Form1 myForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            myForm = new Form1();

            

            if (args.Length > 0)
            {
                openFile = args[0];

                // Attach a handler to the Shown event
                myForm.Shown += new EventHandler(myForm_Shown);
            }

            // keep this at the end of the Main() method as anything after it won't run until closing.
            Application.Run(myForm);
        }

        // The handler for the Shown event
        static void myForm_Shown(object sender, EventArgs e)
        {
            // async open file
            openFileAfterLaunchAsync(openFile); // Call the async method without waiting
        }

        static async Task openFileAfterLaunchAsync(string openFile)
        {
            Console.WriteLine("openFile: " + openFile);
            await Task.Delay(100); // Wait a little while for everything to start
            myForm.openFile(openFile);
        }
    }
}
