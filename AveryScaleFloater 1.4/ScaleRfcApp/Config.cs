using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Security.Cryptography;
using System.IO;

namespace ScaleRfcApp
{
    public partial class Config : Form
    {
		//4939: Password object for encryption/decryption
        Password pw;

        public Config()
        {
            InitializeComponent();
            var settings = (Settings)XMLConfigHandler.ReadConfig(typeof(Settings));
			//4939: Initialize Password object
			pw = new Password();
            if (settings != null)
                UpdateComponents(settings);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var settings = new Settings();

            settings.client = txtClient.Text;
            settings.userName = txtUserName.Text;
			//4939: Encrypt password before saving to the config file
            settings.password = pw.Encrypt(txtPassword.Text,"ABC");
            settings.language = txtLanguage.Text;
            settings.systNo = txtSystNo.Text;
            settings.serverName = txtServerName.Text;
            settings.destName = txtDestName.Text;
			//4939: not needed anymore
            //settings.appServer = txtAppServer.Text; 
            settings.programId = txtProgId.Text;
            settings.sapGWHost = txtSapGWHost.Text;
            settings.sapGateway = txtSapGateway.Text;
            settings.IPAddress = txtIPAddress.Text;
            settings.port = txtPort.Text;
            //4939: Detailed Logs
            settings.detailedLogs = chkDetailedLogs.Checked;
           
        XMLConfigHandler.SaveConfig(settings);
            //4939: Better Message
            MessageBox.Show("Config Settings saved successfully", "Config Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

			//4939: Close window after saving
            this.Close();

        }      

        private void UpdateComponents(Settings s)
        {
            txtClient.Text = s.client ;
            txtUserName.Text = s.userName ;
            //4939: Decrypt password before displaying
            txtPassword.Text = pw.Decrypt(s.password, "ABC");
            txtLanguage.Text = s.language;
            txtSystNo.Text = s.systNo;
			//4939: required but can be hard-coded
            txtServerName.Text = "SAP";
			//4939: required but can be hard-coded
            txtDestName.Text = "SAP";
			//4939: not needed anymore
            //txtAppServer.Text = s.appServer;
            txtProgId.Text = s.programId;
            txtSapGWHost.Text = s.sapGWHost;
            txtSapGateway.Text = s.sapGateway;
            txtIPAddress.Text = s.IPAddress;
            txtPort.Text = s.port;
            //4939: Detailed Logs
            chkDetailedLogs.Checked = s.detailedLogs;
        }        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
