using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScaleRfcApp
{
    class ErrorLog
    {
        public static void log(bool exit_app, char msgType, string format, params object[] objs )
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(format, objs);

            try
            {
                //4939: Read Config File
                var settings = (Settings)XMLConfigHandler.ReadConfig(typeof(Settings));

                //4939: Write log if this is an error message or if detailed logs are enabled
                if (msgType == 'E' || settings.detailedLogs == true)
                {
                    using (StreamWriter w = File.AppendText("log.txt"))
                    {
                        w.Write($"[{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}]: ");
                        w.WriteLine($"{sb}");
                    }
                }

                //4939: Display message box if this is a serious error message then close the application
                if (exit_app == true)
                {
                    MessageBox.Show(sb.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Environment.Exit(0);
                }
            }
            catch(Exception ex)
            {
                
            }

        }
    }
}
