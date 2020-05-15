using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace BOT_SpotSensor
{
    public class ServiceParkingSpots : IServiceParkingSpots
    {
        public static String m_strPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "App_Data\\";

        public static String m_strFilenameParkingSpotXML = "ParkingSpotConfig.xml";
        public static String m_strFilenameParkingSpotXSD = "ParkingSpotConfig.xsd";
        public static String m_strFilenameParkingNodesXSD = "ParkingNodesConfig.xsd";
        
        private string m_strExceptionXml;

        public Boolean CreateParkingSpotXML(ParkingSpot newParkingSpot)
        {
            XmlDocument doc = new XmlDocument();
            // Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec); //dec é o header do doc XML

            HandlerXML handlerXML = new HandlerXML(m_strPath + m_strFilenameParkingSpotXML, m_strPath + m_strFilenameParkingSpotXSD);

            string parkingSpotXML = handlerXML.CreateParkingSpotXML(newParkingSpot);
            doc.InnerXml += parkingSpotXML;
            doc.Save(m_strPath + m_strFilenameParkingSpotXML);
            if (!handlerXML.ValidateXML())
            {
                m_strExceptionXml = handlerXML.ValidationMessage;
                return false;
            }
            return true;
        }

        public ParkingSpot GetParkingSpot()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_strPath + m_strFilenameParkingSpotXML);

            XmlNode node = doc.SelectSingleNode("/parkingSpot");
            if (node == null)
            {
                return null;
            }

            bool spotStatus = node["status"].FirstChild.InnerText == "free";
            bool batteryStatus = node["batteryStatus"].InnerText == "1";
            return new ParkingSpot(
                    node["id"].InnerText,
                    (Type)Enum.Parse(typeof(Type), node["type"].InnerText),
                    node["name"].InnerText,
                    node["location"].InnerText, //empty
                    spotStatus,
                    (DateTime)DateTime.Parse(node["status"].LastChild.InnerText),
                    batteryStatus);
        }

        public string GetParkingSpotXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_strPath + m_strFilenameParkingSpotXML);

            return doc.LastChild.OuterXml; //send only the parkingSpot xml element
        }
    }
}
