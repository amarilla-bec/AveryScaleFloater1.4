using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary;
using System.Xml.Linq;

namespace ScaleRfcApp
{
    public partial class Floater : Form
    {
        Thread thread;
        //4939: not used
        //Thread threadTare   ;
        //string[] args;
        //RfcServerFuncs[] rfcFuncs;
        RfcServerFuncs rfcFunc;
        delegate void SetTextCallback(string text);
        delegate void SetTareCallback(double tare);
        public static string netWgt;
        public static string grossWgt;
        //4939: Stable weight flag
        public static bool gv_stable;
        public string weight;       
        public static double tare;
        public string initialtare;
        public TcpClient client;
        public static string IPaddress;
        public static int port;
        string filepath = @"Settings.xml";
        static readonly object _locker = new object();
        ServerConfiguration objServConfig;
        //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
        private IAsyncResult result;
        private bool success;
        private NetworkStream stream;
        //4939: RFC Connection Status
        private bool rfcConnection;
        private bool readWeight;
        //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
        public Floater()
        {
            InitializeComponent();
             objServConfig = new ServerConfiguration();
            XElement elem = XElement.Load(filepath);
            IPaddress = elem.Element("IPAddress").Value;
            port = Convert.ToInt32(elem.Element("port").Value);

            rfcFunc = new RfcServerFuncs(elem.Element("serverName").Value);
            rfcFunc.StartRfcServer();

            this.FormClosed += Floater_FormClosed;

            //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement  
            newConnection();
            readWeight = true;
            //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement

            thread = new Thread(CallWithAsync);
            thread.Name = "FloaterThread";
            thread.Start();

        }

        public Floater(out string netWeight, out string grossWeight, out bool stableWeight)
        {
            InitializeComponent();
            netWeight = netWgt;
            grossWeight = grossWgt;
            stableWeight = gv_stable;
        }

        private void Floater_FormClosed(object sender, FormClosedEventArgs e)
        {
            rfcFunc.StopRfcServer();
            
            thread.Abort();
            //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
            readWeight = false;
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
            //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
            this.Dispose();
            this.Close();

        }

        private void SetText(string text)  //this parameter is our Gross weight
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblnetWeight1.IsDisposed == false && this.lblnetWeight1.InvokeRequired)
            {
                try
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    if (this.lblnetWeight1.IsDisposed == false)
                        this.Invoke(d, new object[] { text });  //throws an exception here when form(floater) is closed . Catch it please.
                }
                catch(Exception ex)
                {
                    ErrorLog.log(false, 'E', "Error encountered setting text: {0}", ex.Message);
                }
            }
            else
            {
                //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
                weight = text.Substring(1, 8).Trim();  //always gross weight
                //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement

                //4939: Initialize text
                this.weightStabilityHeader.Text = "";
                this.lblnetWeight1.Text = "--";
                this.tare1.Text = "-- Kg";
                this.grossWeight.Text = "-- Kg";				
				
                //4939:Initialize text colour
				SetTextColour(Color.White);
				
                //4939: Initialize background colour
				this.BackColor = Color.Red;

				//4939: Connection failed
                if (text == "No Connection to Scale")
                {
                    this.weightStabilityHeader.Text = text;				
                }
                else
                {
                    //4939: Unstable weight
                    //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
                    if (text[13].ToString() != "0")
                    //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
                    {
                        this.weightStabilityHeader.Text = "Unstable Weight";
                        gv_stable = false;
                    }
					//4939: Stable weight
                    else
                    {
                        this.weightStabilityHeader.Text = "Weight is stable";
                        SetTextColour(Color.Black);
						this.BackColor = Color.Lime;
                        gv_stable = true;
					}
					
					//4939: Display weight
                    try
                    {
                        grossWgt = String.Format("{0:0.00}", Convert.ToDouble(weight));
                        netWgt = String.Format("{0:0.00}", Convert.ToDouble(weight) - tare);                        
                        this.lblnetWeight1.Text = (String.Format("{0:0.00}", netWgt));
                        this.tare1.Text = (String.Format("{0:0.00} Kg", tare));
                        this.grossWeight.Text = grossWgt + " Kg";  
                    }
					//4939: Overloaded
                    catch (Exception ex)
                    {
                        this.weightStabilityHeader.Text = "Weight Overload";                            
						SetTextColour(Color.Black);
						this.BackColor = Color.Yellow; 
                        ErrorLog.log(false, 'E', "Error encountered: {0}, possibly 'Weight Overload'", ex.Message);
                    }
                    
                }               
            }
        }       
        
        public async void CallWithAsync()
        {
            //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
            while (readWeight)
            //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
            {
                string result = await GreetingAsync();
                SetText(result);              
            }            
        }

        public Task<string> GreetingAsync()
        {
            return Task.Run<string>(() =>
            {
                //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
                return this.ReadWeight("W\r"); //this weight is gross weight, we arent setting the tare anywhere using 'T' command.always using gross weight and then calculating the net weight.
                //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
            });
        }

        public void setTare(double tare)
        {
            Floater.tare = Convert.ToDouble(tare.ToString("0.##"));  //limit to 2 decimal places          
            this.tare1.Text = (String.Format("{0:0.00} kg", tare));
        }

        public string ReadWeight(string command)
        {           
            lock (_locker)
            {
                try
                {
                    // Create a TcpClient.
                    // Note, for this client to work you need to have a TcpServer 
                    // connected to the same address as specified by the server, port
                    // combination.
                    string message = command;
                   
                    //4939: Check if floater is connected to the scale and if RFC Connection is okay
                    if (!success || !rfcConnection)
                    {
                        return "No Connection to Scale";
                    }
                    else
                    {

                        // Translate the passed message into ASCII and store it as a Byte array.
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                       
                        // Send the message to the connected TcpServer. 
                        stream.Write(data, 0, data.Length);

                        // Buffer to store the response bytes.
                        data = new Byte[256];

                        // String to store the response ASCII representation.
                        String responseData = String.Empty;

                        //4939: Set Timeout to 2 seconds
                        stream.ReadTimeout = 2000;

                        // Read the first batch of the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                        //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
                        ErrorLog.log(false, 'I', responseData, "");
                        //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
                        return (responseData);
                    }
                }
                catch (Exception ex)
                {
                    ErrorLog.log(false, 'E', "Error encountered while connecting to the scale: {0}", ex.Message);
                    //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement  
                    newConnection();
                    //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
                    return "No Connection to Scale";
                }
            }

        }

        //4939: Set Text Colour
        private void SetTextColour(Color textColor)
        {
            this.lblnetWeight1.ForeColor = textColor;
            this.tare1.ForeColor = textColor;
            this.grossWeight.ForeColor = textColor;
            this.lblGrossWeight.ForeColor = textColor;
            this.lblNetWeight.ForeColor = textColor;
            this.lblTare.ForeColor = textColor;
            this.weightStabilityHeader.ForeColor = textColor;
            this.lblNetWeightUOM.ForeColor = textColor;
        }
        //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
        private void newConnection()
        {
            try
            {
                //4939: Check connection and return status in 1 second
                client = new TcpClient();
                result = client.BeginConnect(IPaddress, port, null, null);
                success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1000));
                if (success == true && client.Connected == true)
                {
                    //4939: Check RFC Connection status if floater can connect to the scale
                    rfcConnection = rfcFunc.restartRFC();
                    // Get a client stream for reading and writing.                   
                    stream = client.GetStream();
                }
                else
                {
                    //4939: No connection to scale. Stop RFC Server.
                    ErrorLog.log(false, 'E', "Can't connect to the scale.", "");
                    rfcFunc.StopRfcServer();
                    rfcConnection = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.log(false, 'E', "Error encountered while establishing connection to scale: {0}", ex.Message);
                rfcFunc.StopRfcServer();
                rfcConnection = false;
            }
        }

        private void tmrCheckConnection_Tick(object sender, EventArgs e)
        {
            if (client.Connected == false || rfcConnection == false)
            {
                newConnection();
            }
        }
        //AMARILLA - 09.06.2022 - NC1 Protocol and Performance Improvement
    }
}
