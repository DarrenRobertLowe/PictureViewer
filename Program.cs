using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureViewer
{
    static class Program
    {
        static string openFile;
        static Form1 myForm;

        static bool AppRunningCheck;
        static Mutex mutex = new Mutex(true, "{12637405-A473-4464-965A-631A94E0FC52}", out AppRunningCheck); // used to make the program a singleton. Note Static will prevent Garbage Collector from recycling the mutex


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {

            if (!AppRunningCheck)
            {
                MessageBox.Show("Another instance is already running.");
                // send a win32 message to bring the currently running instance window to the front
                // send the file location as a message to the running app
                // in the running app open the file upon receiving the message
            }
            else //if (mutex.WaitOne(TimeSpan.Zero, true))
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
