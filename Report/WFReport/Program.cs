using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFReport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            System.Threading.Mutex mutex = new System.Threading.Mutex(false, "{8D719F92_EA4B_41F1_9905_0C914E45414C_Ololap");
            try
            {
                if (mutex.WaitOne(0, false))
                {
                    // Run the application
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
                else
                {
                    MessageBox.Show("Another instance of the application is already running.\nPlease close all the other instances if you want to open the new one",
                        "Application cannot be run", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            finally
            {
                if (mutex != null)
                {
                    mutex.Close();
                    mutex = null;
                }
            }

        }
    }
}
