using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAP.Middleware.Connector;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using IWshRuntimeLibrary;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace ScaleRfcApp
{
    /// <summary>
    /// This class provides functions to act as an RFC server for remote function /SPDGS/DGS_DEVICE_CALL.
    /// This function takes in an COMMAND and returns the 'Weight'  from the floater which reads weight from the weighing scale.  
    /// SAP Connector libraries such as sapnco,sapnco_utils  are used to connect to SAP System.
    /// </summary>
    class RfcServerFuncs
    {
        private RfcServer SapHost = null;
        //4939: not needed
        //private static System.Diagnostics.EventLog eventLog1;
        private string mySapSystem;
        public static Floater objfloater;
        static double tare;
        public static string netWeight;
        public static string grossWeight;
        public static bool stableWeight;

        //4939: not needed
        //RfcConfigParameters maparam;
        //public static int repeatCommandFlag;
        //RfcConfigParameters parameters = null;

        SAPSystemConnect objsapSystemConnect;

        public RfcServerFuncs(string sapSystem)
        {
            //4939: not needed
            //Create an EventLog object. 
            //eventLog1 = new System.Diagnostics.EventLog();       
            
            mySapSystem = sapSystem;

            //4939: Initialise Connection to SAP
            objsapSystemConnect = new SAPSystemConnect();
            RfcDestinationManager.RegisterDestinationConfiguration(objsapSystemConnect);

            //4939: Initialise RFC Connection
            ServerConfiguration objserverconfig = new ServerConfiguration();
            RfcServerManager.RegisterServerConfiguration(objserverconfig);
        }

        private void ReadXml()
        {
            RfcDestination newdest = RfcDestinationManager.GetDestination("dummyString");
            RfcSystemAttributes sapAttributes = newdest.SystemAttributes;  //statement added to check an excpetion when supplying credentials to SAP system. 
        }

        public bool StartRfcServer()
        {
            //4939: not needed
            //IDestinationConfiguration config = null;

            Type[] RfcHandlers = new Type[1] { typeof(RfcServerFuncs) };

            // This is standard boiler plate code to start an RFC server. Note that the SAP instances must be
            // defined in the App.config file.
            try
            {
                //4939: Message Log
                ErrorLog.log(false, 'I', "Starting RFC...", "");
                ReadXml();
                SapHost = RfcServerManager.GetServer(mySapSystem, RfcHandlers);
                SapHost.Start();
                //4939: Message Log
                ErrorLog.log(false, 'I', "RFC Started.", "");
            }
            catch (Exception ex)
            {
                //4939: Error Log
                ErrorLog.log(false, 'E', "Error encountered while starting the server: {0}", ex.Message);
                return false;
            }

            return true;
        }

        public void StopRfcServer()
        {
            try
            {
                //4939: Validation
                if (SapHost != null)
                {
                    //4939: Message Log
                    ErrorLog.log(false, 'I', "Shutting Down RFC...", "");
                    SapHost.Shutdown(true);
                    //4939: Message Log
                    ErrorLog.log(false, 'I', "RFC Shut Down.", "");
                }
            }
            catch (Exception ex)
            {
                //4939: Error Log
                ErrorLog.log(false, 'E', "Error encountered while shutting the server: {0}", ex.Message);
            }
        }

        /* 4939: Not being used
        public static void log(bool error, string format, params object[] objs)
        {
            //Just some simple code to write an event to the application log.
#if (_CONSOLE)
                            Console.WriteLine(format, objs);
#endif
            EventLogEntryType lvl = new EventLogEntryType();
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(format, objs);

            if (error)
                lvl = EventLogEntryType.Error;
            else
                lvl = EventLogEntryType.Information;

            eventLog1.WriteEntry(sb.ToString(), lvl);
        }
        */
        /// <summary>
        /// RAF Application calls SAP function "/SPDGS/DGS_DEVICE_CALL" and this below function gets executed.
        /// RAF Application looks for RFC destination maintained in ZRAF_INFO.The device name is configured in RAF App. and looks for corresponding RFC Destination.
        /// Tansaction 'sm59' in SAP system has the program ID for corresponding RFC destination.This program ID is configured in App.config of this solution.
        /// </summary>
        /// <param name="myServerContext"></param>
        /// <param name="myFunction"></param>
        [RfcServerFunction(Name = "/SPDGS/DGS_DEVICE_CALL")]
        public static void GetWeightStatusRfc(RfcServerContext myServerContext, IRfcFunction myFunction)
        {
            try
            {
                objfloater = new Floater(out netWeight, out grossWeight, out stableWeight);
                IRfcTable addressData = myFunction.GetTable("PTABELLE");
                string command = addressData.GetString("COMMAN");
                //4939: Message Log
                ErrorLog.log(false, 'I', "Received command: {0}", command);
                if (command != "" && command[0].ToString() == "T")
                {
                    tare = Math.Round(Convert.ToDouble(command.Substring(1, command.Length - 1)), 3);            
                    objfloater.setTare(tare);
                }

                if (command == "")
                {
                    //4939: Return Stable Weight Status
                    string finalweight;
                    if (stableWeight == true)
                    {
                        finalweight = " ; 0; ;" + grossWeight + ";" + netWeight + ";" + tare;
                    }
                    else
                    {
                        finalweight = " ; 1; ;" + grossWeight + ";" + netWeight + ";" + tare;
                    }
                    addressData.SetValue("COMMAN", finalweight);
                    addressData.Append();
                    //4939: Message Log
                    ErrorLog.log(false, 'I', "Returning Gross Weight: {0}Kg", grossWeight);
                    ErrorLog.log(false, 'I', "Returning Net Weight: {0}Kg", netWeight);
                    ErrorLog.log(false, 'I', "Returning Tare Weight: {0}Kg", tare);
                    ErrorLog.log(false, 'I', "Returning Stable Weight Status: {0}", stableWeight);
                }

                if (command == "t")
                {
                    //Set net weight to zero
                }

                if (command == "Z")
                {
                    objfloater.setTare(0.000);
                    //Set tare to zero
                }

            }

            catch (Exception ex)
            {
                ErrorLog.log(true, 'E', "Error encountered while reading values from SAP: {0}", ex.Message);
            }
        }

        /*4939: Not being used
        private void MapZDrive()
        {
            IWshNetwork_Class net = new IWshNetwork_Class();
            IWshCollection drives = net.EnumNetworkDrives();
            bool nextOne = false;

            //This is both inelegant and insecure...
            string mapping = @"\\BECYSS1\NLISDatabase";
            string user = @"BECAMPBELL\yg_nlis";
            string password = "Security786";

            log(false, "Checking that Z drive is mapped to {0}", mapping);

            for (int i = 0; i < drives.length; i++)
            {
                if (nextOne)
                {
                    string currMapping = drives.Item(i).ToString();

                    log(false, "Current Z Drive mapping {0}", currMapping);
                    if (currMapping.Equals(mapping))
                    {
                        log(false, "Already mapped correctly to {0} - leave it as is.", mapping);
                        return;  //Already mapping correctly
                    }
                    else
                    {
                        log(false, "Removing current Z: mapping to {0}", currMapping);
                        try
                        {
                            net.RemoveNetworkDrive("z:");
                        }
                        catch (Exception e)
                        {
                            log(true, "Exception raised when removing Z drive mapping:\n {0}", e.ToString());
                        }
                        break;
                    }
                }

                if (drives.Item(i).ToString().Equals("Z:"))
                {
                    //This 
                    nextOne = true;
                }
            }
            log(false, "Mapping Z: drive to {0}", mapping);
            try
            {
                net.MapNetworkDrive("z:", mapping, Type.Missing, user, password);
            }
            catch (Exception e)
            {
                log(true, "Exception raised when creating Z drive mapping:\n {0}", e.ToString());
            }
        }
        */

        //4939: Check RFC Status and restart if needed
        public bool restartRFC()
        {
            try
            {
                ErrorLog.log(false, 'I', "Network Connection to Scale is okay. Checking RFC Status...", "");
                if (SapHost != null)
                {                    
                    if (SapHost.Monitor.ConnectionCount == 0)
                    {
                        ErrorLog.log(false, 'I', "RFC is down. Restarting RFC...", "");
                        SapHost.Shutdown(true);
                        return this.StartRfcServer();
                    }
                    else
                    {
                        ErrorLog.log(false, 'I', "RFC is up.", "");
                        return true;
                    }
                }
                else
                {
                    return this.StartRfcServer();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(false, 'E', "Error encountered while restarting RFC: {0}", ex.Message);
                return false;
            }
        }

    }
}
