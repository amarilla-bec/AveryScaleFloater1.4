using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
//4939: Message Box
using System.Windows.Forms;

namespace ScaleRfcApp
{
     class XMLConfigHandler
    {
        static public object ReadConfig(Type type)
        {
            // Just some boiler plate deserialisation code...
            var settings = Activator.CreateInstance(type);
            System.IO.FileStream fs = null;

            try
            {
                // Create an instance of the XmlSerializer class; specify the type of object to be deserialized.
                XmlSerializer serializer = new XmlSerializer(type);

                /* If the XML document has been altered with unknown nodes or attributes, handle them with the 
                UnknownNode and UnknownAttribute events.*/
                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                // A FileStream is needed to read the XML document.
                fs = new System.IO.FileStream(type.Name + ".xml", System.IO.FileMode.Open);

                /* Use the Deserialize method to restore the object's state withdata from the XML document. */
                settings = serializer.Deserialize(fs);

                fs.Close();
            }
            catch (Exception exp)
            {
                if (fs != null) fs.Close();              
            }            

            return settings;
        }

        static private void serializer_UnknownNode
        (object sender, System.Xml.Serialization.XmlNodeEventArgs e)
        {
            //4939: Message Box
            MessageBox.Show("Unknown Node: " + e.Name, "Error Reading Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
            System.Environment.Exit(0);
        }

        static private void serializer_UnknownAttribute
        (object sender, System.Xml.Serialization.XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            //4939: Message Box
            MessageBox.Show("Unknown Attribute: " + attr.Name, "Error Reading Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
            System.Environment.Exit(0);
        }

        static public void SaveConfig(object settings)
        {
            // Just some boiler plate serialisation code...
            try
            {
                Type type = settings.GetType();

                // Create an instance of the XmlSerializer class; specify the type of object to be deserialized.
                XmlSerializer serializer = new XmlSerializer(type);

                /* If the XML document has been altered with unknown nodes or attributes, handle them with the 
                UnknownNode and UnknownAttribute events.*/
                serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                // A FileStream is needed to read the XML document.
                System.IO.FileStream fs = new System.IO.FileStream(type.Name + ".xml", System.IO.FileMode.Create);

                /* Use the Deserialize method to restore the object's state withdata from the XML document. */
                serializer.Serialize(fs, settings);
            }
            catch (Exception exp)
            {
                //4939: Message Box
                MessageBox.Show(exp.Message, "Error Saving Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
            }
        }
    }
}
