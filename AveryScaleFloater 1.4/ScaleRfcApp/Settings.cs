using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScaleRfcApp
{
    [XmlRootAttribute("LocalSettings")]
    public class Settings
    {
        //SAP Client Settings
        public string client{ set; get; }
        public string userName { set; get; }
        public string password { set; get; }
        public string language { set; get; }
        public string systNo { set; get; }
        public string appServer { set; get; }
        public string serverName { set; get; }        

        //SAP Server settings
        public string programId { set; get; }
        public string sapGWHost { set; get; }
        public string sapGateway{ set; get; }
        public string destName { set; get; }

        //Scale Config
        public string IPAddress { set; get; }
        public string port { set; get; }

		//4939: Detailed Logs
        public bool detailedLogs { set; get; }

        public override string ToString()
        {
            return string.Format("Client={0} UserName={1} Password={2} Language = {3} SystemNumber={4} ApplicationServer={5} ProgramId={6} SAPGWHost={7} SAPGateway={8} IPAddress={9} Port={10} ServerName={11} Repository Dest. Name = {12} ",
                client, userName, password, language, systNo, appServer, programId, sapGWHost, sapGateway,IPAddress,port,serverName,destName);
        }
    }
}
