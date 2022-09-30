using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScaleRfcApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool config = false;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                //Determine if -i was provided as a command-line argument
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-i")
                    {
                        config = true;
                        break;
                    }
                }

                //Load the Configuration Screen if -i was provided
                if (config == true)
                {
                    Application.Run(new Config());
                }
                //Load the Floater if -i was not provided
                else
                {
                    Application.Run(new Floater());
                }
            }
            catch(Exception ex)
            {
                //4939: Error logging
                ErrorLog.log(true, 'E', "Error encountered when launching the application: {0}", ex.Message);
            }

        }
    }
}
