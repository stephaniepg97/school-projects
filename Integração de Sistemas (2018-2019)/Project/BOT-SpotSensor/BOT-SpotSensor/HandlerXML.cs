using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Schema;

namespace BOT_SpotSensor
{
    public class HandlerXML
    {
        public HandlerXML(string xmlFile)
        {
            XmlFilePath = xmlFile;
        }

        public HandlerXML(string xmlFile, string xsdFile)
        {
            XmlFilePath = xmlFile;
            XsdFilePath = xsdFile;
        }

        public string XmlFilePath { get; set; }
        public string XsdFilePath { get; set; }

        private bool isValid = true;
        private string validationMessage;

        public string ValidationMessage
        {
            get { return validationMessage; }
        }

        #region Create Parking Spot XML Node
        public string CreateParkingSpotXML(ParkingSpot newParkingSpot)
        {
            XmlDocument doc = new XmlDocument();
            // Create the parkingSpot element
            XmlElement parkingSpot = doc.CreateElement("parkingSpot");
            XmlElement id = doc.CreateElement("id");
            id.InnerText = newParkingSpot.ParkId;
            XmlElement type = doc.CreateElement("type");
            type.InnerText = newParkingSpot.Type.ToString();
            XmlElement name = doc.CreateElement("name");
            name.InnerText = newParkingSpot.SpotId;
            XmlElement location = doc.CreateElement("location");
            location.InnerText = newParkingSpot.Location; //empty
            XmlElement status = doc.CreateElement("status");
            XmlElement value = doc.CreateElement("value");
            XmlElement timestamp = doc.CreateElement("timestamp");
            if (newParkingSpot.Status.Value)
            {
                value.InnerText = "free";
            }
            else
            {
                value.InnerText = "it has a vehicle";
            }
            timestamp.InnerText = newParkingSpot.Status.Timestamp.ToString("o");
            status.AppendChild(value);
            status.AppendChild(timestamp);
            XmlElement batteryStatus = doc.CreateElement("batteryStatus");
            if (newParkingSpot.BatteryStatus)
            {
                batteryStatus.InnerText = "1";
            }
            else
            {
                batteryStatus.InnerText = "0";
            }
            parkingSpot.AppendChild(id);
            parkingSpot.AppendChild(type);
            parkingSpot.AppendChild(name);
            parkingSpot.AppendChild(location);
            parkingSpot.AppendChild(status);
            parkingSpot.AppendChild(batteryStatus);

            return parkingSpot.OuterXml;
        }
        #endregion

        #region Validate XML with XML Schema (xsd)
        public Boolean ValidateXML()
        {
            isValid = true;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(XmlFilePath);
                ValidationEventHandler eventHandler = new ValidationEventHandler(MyValidateMethod);
                doc.Schemas.Add(null, XsdFilePath);
                doc.Validate(eventHandler);
            }
            catch (XmlException ex)
            {
                isValid = false;
                validationMessage = string.Format("ERROR: {0}", ex.ToString());
            }
            return isValid;
        }

        private void MyValidateMethod(object sender, ValidationEventArgs args)
        {
            isValid = false;
            switch (args.Severity)
            {
                case XmlSeverityType.Error:
                    validationMessage = string.Format("ERROR: {0}", args.Message);
                    break;
                case XmlSeverityType.Warning:
                    validationMessage = string.Format("WARNING: {0}", args.Message);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}