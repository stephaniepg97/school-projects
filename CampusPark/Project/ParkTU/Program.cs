using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Threading;

namespace ParkTU
{
    class Program
    {
        public static String m_strFilenameParksXML = @"Parks.xml";
        public static String m_strFilenameParksXSD = @"Parks.xsd";

        const String STR_CHANNEL_NAME_1 = "PARK_DACE";
        const String STR_CHANNEL_NAME_2 = "PARK_SS";

        //MqttClient m_cClient = new MqttClient(IPAddress.Parse("192.168.237.155"));
        static MqttClient m_cClient = new MqttClient("127.0.0.1");
        static string[] m_strTopicsInfo = { STR_CHANNEL_NAME_1 };

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("*******************************  PARK TU  *******************************");

                #region Start connection...
                Console.WriteLine("Connecting with 127.0.0.1...");

                m_cClient.Connect(Guid.NewGuid().ToString());

                if (m_cClient.IsConnected)
                {
                    Console.WriteLine("Connected.");
                }
                else
                {
                    Console.WriteLine("Error connecting to message broker...");
                    return;
                }
                #endregion

                #region Connection with PARK DACE
                Console.WriteLine("Waiting to receive config file XML from PARK DACE...");
            
                //Subscribe chat channel
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };//QoS
                m_cClient.Subscribe(m_strTopicsInfo, qosLevels);

                do
                {
                    Console.WriteLine("Press 'ESC' to stop connection.");
                    //listener: receive message
                    m_cClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
                #endregion

                //Unsubscribe all topics
                m_cClient.Unsubscribe(m_strTopicsInfo);

                #region End connection...
                Console.WriteLine("Closing connection with 127.0.0.1...");
                do
                {
                    Console.WriteLine("Still on connection, closing...");
                    m_cClient.Disconnect();
                    Thread.Sleep(1000);
                } while (m_cClient.IsConnected);
                #endregion

                Console.WriteLine("Disconnected. Other applications to disconnect: PARK SS & PARK DACE");
                Console.WriteLine("Press a key to exit.");
                Console.ReadKey();
            }
            catch (MqttClientException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string strMsg = Encoding.UTF8.GetString(e.Message);

            if (strMsg.Trim().Length <= 0)
            {
                Console.WriteLine("Invalid message.");
                return;
            }

            Console.WriteLine("Received = " + strMsg + " on topic " + e.Topic);

            if (!CreateParksInfoXmlFile(strMsg))
            {
                return;
            }

            #region Connection with other Clients
            Console.WriteLine("Sending parks information XML to PARK SS...");

            string strMsgToSend = GetParksInfoXml();
            Console.WriteLine("Sending: " + strMsgToSend);

            //send paks.xml content to Park SS
            m_cClient.Publish(STR_CHANNEL_NAME_2, Encoding.UTF8.GetBytes(strMsgToSend));
            #endregion
        }

        static Boolean CreateParksInfoXmlFile(string strMsg)
        {
            XmlDocument docConfigFile = new XmlDocument();
            docConfigFile.LoadXml(strMsg);

            XmlNodeList parkInfoNodes = docConfigFile.SelectNodes("//parkInfo");

            XmlDocument doc = new XmlDocument();
            // Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec); //dec it's the header of doc XML
            XmlElement parks = doc.CreateElement("parks");

            foreach (XmlNode node in parkInfoNodes)
            {
                parks.InnerXml += node.OuterXml;
            }

            doc.AppendChild(parks);
            doc.Save(m_strFilenameParksXML);

            HandlerXML handlerXML = new HandlerXML(m_strFilenameParksXML, m_strFilenameParksXSD);

            if (!handlerXML.ValidateXML())
            {
                Console.WriteLine("Error: Parks Info Xml File not valid. " + handlerXML.ValidationMessage);
                return false;
            }
            else
            {
                Console.WriteLine("Success: Parks Info Xml File valid.");
                return true;
            }
        }

        static string GetParksInfoXml()
        {
            XmlDocument docParksInfo = new XmlDocument();
            docParksInfo.Load(m_strFilenameParksXML);

            return docParksInfo.OuterXml;
        }
    }
}
