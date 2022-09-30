using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAP.Middleware.Connector;
using System.Xml;
using System;
using System.Xml.Linq;

namespace ScaleRfcApp
{
    class ServerConfiguration : IServerConfiguration
    {
        public event RfcServerManager.ConfigurationChangeHandler ConfigurationChanged;
        //4939: not needed
        //Dictionary<string, string> dict = new Dictionary<string, string>();
        //public string IpAddress;   //scale IP address
        //public string port;  //scale port

        public bool ChangeEventsSupported()
        {
            //throw new NotImplementedException();
            return false;
        }

        public RfcConfigParameters GetParameters(string serverName)
        {
            //throw new NotImplementedException();

            RfcConfigParameters parameters = new RfcConfigParameters();

            var settings = (Settings)XMLConfigHandler.ReadConfig(typeof(Settings));
            if (settings != null)
            {
                parameters.Add(RfcConfigParameters.GatewayHost, settings.sapGWHost);
                parameters.Add(RfcConfigParameters.GatewayService, settings.sapGateway); //4939: not required
                parameters.Add(RfcConfigParameters.ProgramID, settings.programId);
                parameters.Add(RfcConfigParameters.RepositoryDestination, settings.destName);
                parameters.Add(RfcConfigParameters.RegistrationCount, "1"); //4939: not required
                parameters.Add(RfcConfigParameters.Language, settings.language); //4939: not required
                parameters.Add(RfcConfigParameters.SystemNumber, settings.systNo); //4939: not required
            }
            return parameters;
        }
    }
}
