using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
//System.Web.HttpContext

namespace ScaleRfcApp
{
    class SAPSystemConnect : IDestinationConfiguration
    {
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;
        //4939: not needed
        //Dictionary<string, string> dict = new Dictionary<string, string>();

        public bool ChangeEventsSupported()
        {
            return true;
        }

        public RfcConfigParameters GetParameters(string destinationName)
        {
            //throw new NotImplementedException();
            RfcConfigParameters parameters = new RfcConfigParameters();
			
			//4939: Password object for decryption
            Password pw = new Password();

            var settings = (Settings)XMLConfigHandler.ReadConfig(typeof(Settings));
            if (settings != null)
            {
                parameters.Add(RfcConfigParameters.Client, settings.client);
                parameters.Add(RfcConfigParameters.GatewayHost, settings.sapGWHost);
                parameters.Add(RfcConfigParameters.GatewayService, settings.sapGateway); //4939: not required
                parameters.Add(RfcConfigParameters.ProgramID, settings.programId); //4939: not required
                parameters.Add(RfcConfigParameters.RegistrationCount, "1"); //4939: not required
                parameters.Add(RfcConfigParameters.Language, settings.language); //4939: not required
                parameters.Add(RfcConfigParameters.SystemNumber, settings.systNo); //4939: not required
                parameters.Add(RfcConfigParameters.User, settings.userName);
				//4939: Decrypt password before connecting to SAP
                parameters.Add(RfcConfigParameters.Password, pw.Decrypt(settings.password,"ABC"));
                parameters.Add(RfcConfigParameters.PeakConnectionsLimit, "10"); //4939: not required
                parameters.Add(RfcConfigParameters.ConnectionIdleTimeout,"10"); //4939: not required
            }
            return parameters;
        }
    }
}
