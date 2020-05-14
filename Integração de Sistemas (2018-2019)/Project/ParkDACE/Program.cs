using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Microsoft.Office.Interop.Excel;
using ParkDACE.ServiceReferenceParkingSpots;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Exceptions;

namespace ParkDACE
{
    class Program
    {
        public static string m_strFilenameParkingNodesXML = @"ParkingNodesConfig.xml";
        public static string m_strFilenameParkingNodesXSD = @"ParkingNodesConfig.xsd";
        public static string m_strFilenameParkingSpotXML = @"ParkingSpotConfig.xml";
        public static string m_strFilenameParkingSpotXSD = @"ParkingSpotConfig.xsd";
        public static String m_strPathApp = AppDomain.CurrentDomain.BaseDirectory.ToString();

        const String STR_CHANNEL_NAME = "PARK_DACE";

        //MqttClient m_cClient = new MqttClient(IPAddress.Parse("192.168.237.155"));
        static MqttClient m_cClient = new MqttClient("127.0.0.1");
        static string[] m_strTopicsInfo = { STR_CHANNEL_NAME };

        //DLL service generator instance
        public static ParkingSensorNodeDll.ParkingSensorNodeDll dll = new ParkingSensorNodeDll.ParkingSensorNodeDll();

        //Create Parking Spot with SOAP service 
        public static void CreateParkingSpotSOAP()
        {
            Console.WriteLine("----------------- GET PARKING SPOT XML, DATA CONTRACT AND ENDPOINT ---------------------");

            string parkingSpotXML = null;
            ParkingSpot parkingSpot = null;
            string endpoint = null;
            using (ServiceParkingSpotsClient serviceClient = new ServiceParkingSpotsClient())
            {
                parkingSpotXML = serviceClient.GetParkingSpotXML();
                parkingSpot = serviceClient.GetParkingSpot();
                endpoint = serviceClient.Endpoint.ListenUri.AbsoluteUri;
            }

            if (parkingSpotXML == null)
            {
                Console.WriteLine("ERROR: Parking spot XML data not received.");
                return;
            }
            Console.WriteLine("XML: " + parkingSpotXML);
            
            if (parkingSpot == null)
            {
                Console.WriteLine("ERROR: Data Contract <ParkingSpot> type not received.");
                return;
            }
            else
            {
                Console.WriteLine(parkingSpot.SpotId);
            }

            if (endpoint == null)
            {
                Console.WriteLine("ERROR: Endpoint data not received.");
                return;
            }
            else
            {
                Console.WriteLine("Endpoint: " + endpoint);
            }



            JoinData("SOAP", parkingSpot, parkingSpotXML, endpoint);
        }

        //Create Parking Spot with DLL service 
        public static void CreateParkingSpotDLL(string parkingSpotInfo)
        {
            dll.Stop();

            //ParkId;SpotId;Timestamp;ParkingSpotStatus;BatteryStatus
            string[] info = parkingSpotInfo.Split(';');

            Console.WriteLine("--------------- CREATE PARKING SPOT DATA CONTRACT ------------------");

            #region CreateParkingSpotDataContract
            bool batteryStatus = false, spotStatus = false;

            int batteryStatusInt = Convert.ToInt32(info[4], NumberFormatInfo.InvariantInfo);
            int spotStatusInt = Convert.ToInt32(info[3], NumberFormatInfo.InvariantInfo);
            
            if(batteryStatusInt == 1)
            {
                batteryStatus = true;
            }

            if (spotStatusInt == 1)
            {
                spotStatus = true;
            }

            ParkingSpot newParkingSpot = new ParkingSpot
            {
                BatteryStatus = batteryStatus,
                Location = null,
                ParkId = info[0],
                SpotId = info[1],
                Status = new Status
                {
                    Timestamp = DateTime.Parse(info[2]),
                    Value = spotStatus
                },
                Type = ServiceReferenceParkingSpots.Type.ParkingSpot
            };
            #endregion

            Console.WriteLine("--------------- CREATE PARKING SPOT XML ------------------");

            #region CreateParkingSpotXML
            XmlDocument doc = new XmlDocument();
            // Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec); //dec é o header do doc XML

            HandlerXML handlerXML = new HandlerXML(m_strFilenameParkingSpotXML, m_strFilenameParkingSpotXSD);

            string parkingSpotXML = handlerXML.CreateParkingSpotXML(newParkingSpot);
            doc.InnerXml += parkingSpotXML;
            doc.Save(m_strFilenameParkingSpotXML);

            if (!handlerXML.ValidateXML())
            {
                Console.WriteLine(handlerXML.ValidationMessage);
                return;
            }
            Console.WriteLine("SUCCESS: Parking spot XML file created.");
            #endregion

            Console.WriteLine("--------------- SHOW PARKING SPOT XML ------------------");
            Console.WriteLine("Parking Spot Xml (DLL): " + parkingSpotXML);

            string endpoint = dll.ToString();

            JoinData("DLL", newParkingSpot, parkingSpotXML, endpoint);

            dll.DoWork();
        }

        public static void JoinData(string connectionType, ParkingSpot parkingSpot, string parkingSpotXML, string endpoint)
        {
            Console.WriteLine("-------------- GET PARKING SPOT LOCATION -----------------");

            #region UpdateLocation

            string filename = GetGeoLocationFilename(parkingSpot.ParkId);

            if (filename == null)
            {
                Console.WriteLine("ERROR: No filename information attached on the requested parking spot");
            }
            else
            {
                Console.WriteLine("GeoLocationFilename: " + filename);
            }

            string str_pathExcelFile = m_strPathApp + "\\" + filename;

            int[] cell = GetSpotCellFromExcelFile(str_pathExcelFile, parkingSpot.SpotId);

            if (cell == null)
            {
                Console.WriteLine("ERROR: Parking Spot not found on the file.");
                return;
            }
            else
            {
                Console.WriteLine("Cell: " + cell[0].ToString() + "," + cell[1].ToString());
            }

            string location = GetLocationFromExcelFile(str_pathExcelFile, cell[0], cell[1]);

            if (location == null)
            {
                Console.WriteLine("ERROR: No corresponding GeoLocation of the parking spot on the file.");
                return;
            }
            else
            {
                Console.WriteLine("Location: " + location);
            }
            #endregion

            Console.WriteLine("------- UPDATE CONFIG FILE WITH NEW PARKING SPOT, LOCATION AND ENDPOINT ----------");

            #region UpdateConfigFile
            if (!UpdateConfigFile(parkingSpotXML, parkingSpot.ParkId, parkingSpot.SpotId, location, connectionType, endpoint))
            {
                Console.WriteLine("ERROR: Configuration file not updated with new parkingSpot.");
                return;
            }
            else
            {
                Console.WriteLine("SUCCESS: Configuration file updated with new parking spot data.");
            }
            #endregion

            Console.WriteLine("----------------------- VALIDATE CONFIG FILE ---------------------------");

            #region ValidateConfigFile
            HandlerXML handlerXML = new HandlerXML(m_strFilenameParkingNodesXML, m_strFilenameParkingNodesXSD);

            if (!handlerXML.ValidateXML())
            {
                Console.WriteLine("ERROR: Configuration file validation failed");
            }
            else
            {
                Console.WriteLine("SUCCESS: Configuration file validation succeded");
            }
            #endregion
        }




        public static Boolean UpdateConfigFile(string parkingSpotXML, string parkId, string spotId, string location, string connectionType, string endpoint)
        {
            /*Se park não existe => false
             * 
             * Senão:
             *     Se o park estiver no mesmo provider:
             *          Se o park não tem nenhum parking spot => adicionar novo parking spot
             *          Senão
             *              Se tiver o parking spot => substituir (não verificar se existe parking spot noutro provider)
             *          Se o park estiver noutro provider:
             *              Se o park tiver mais parking spots além do parking spot => remover parking spot
             *              Se o park tiver apenas parking spot => remover park
             *          
             *     Se o park estiver apenas no outro provider:
             *     Se o park não tiver o parking spot, mas tem outros => clone do park, remover <parkingSpots> do clone, sem os parking spots para o provider com novo parking spot
             *     Se o park tiver apenas o parking spot => remover <parkingSpots>, clone do park para o provider com novo parking spot
             *     Se o park tiver mais além do parking spot => remover old <parkingSpot>, clone do park para o provider com novo parking spot
            
            */
            XmlDocument docConfigFile = new XmlDocument();
            docConfigFile.Load(m_strFilenameParkingNodesXML);

            // 1st Scenario: if there has any data related to the park
            string xPath = "//parkInfo[id = " + "'" + parkId + "']";
            XmlNodeList parkNodes = docConfigFile.SelectNodes(xPath);

            if (parkNodes == null)
            {
                return false; //Retrives false: park doesn't exist
            }

            xPath = "//provider[endpoint = " + "'" + endpoint + "']/parkInfo[id = " + "'" + parkId + "']";
            XmlNode parkNode = docConfigFile.SelectSingleNode(xPath);

            if (parkNode != null) // 2nd Scenario: if the provider has data related to the park
            {
                if (parkNode["parkingSpots"] == null) // 3rd Scenario: if the park has no parking spots attached
                {
                    #region Create <parkingSpots> node with <parkingSpot> child node
                    XmlNode parkingSpots = docConfigFile.CreateElement("parkingSpots");
                    parkingSpots.InnerXml = parkingSpotXML;
                    parkNode.AppendChild(parkingSpots);
                    #endregion
                }
                else
                {
                    xPath = "descendant::parkingSpot[name = " + "'" + spotId + "']";
                    XmlNode oldParkingSpot = parkNode.SelectSingleNode(xPath);

                    if (oldParkingSpot != null) // 4th Scenario: if the park has older data related to the parking spot
                    {
                        #region Remove old <parkingSpot> node
                        parkNode["parkingSpots"].RemoveChild(oldParkingSpot);
                        #endregion
                    }

                    parkNode["parkingSpots"].InnerXml += parkingSpotXML; //append new parking spot

                    if (parkNodes.Count > 1) // 5th Scenario: if the other provider has also the <parkInfo> node
                    {
                        #region Remove older <parkingSpot> node from another providers
                        foreach (XmlNode node in parkNodes)
                        {
                            if (node.ParentNode["endpoint"].InnerText != endpoint)
                            {
                                oldParkingSpot = node.SelectSingleNode(xPath);
                                if (oldParkingSpot != null) //if the other <parkInfo> has the <parkingSpot> with older data
                                {
                                    if (node["parkingSpots"].ChildNodes.Count > 1) // 5th Scenario: if the other <parkInfo> has more<parkingSpot> nodes attached
                                    {
                                        node["parkingSpots"].RemoveChild(oldParkingSpot); //removes only the <parkingSpot> node with older data 
                                    }
                                    else // 6th Scenario: if the other <parkInfo> has just the <parkingSpot> node with older data 
                                    {
                                        node.RemoveChild(node["parkingSpots"]); //removes the <parkingSpots> node
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            else  // 7th Scenario: if the provider has no data related to the park
            {
                #region Get <parkInfo> node from another provider
                foreach (XmlNode node in parkNodes)
                {
                    if (node.ParentNode["endpoint"].InnerText != endpoint)
                    {
                        parkNode = node.Clone(); //the <parkInfo> node from another provider
                        XmlNode oldParkingSpot = node.SelectSingleNode(xPath);
                        if (oldParkingSpot != null) //8th Scenario: if the <parkInfo> from another provider has the <parkingSpot> with older data
                        {
                            XmlNode parkingSpots = oldParkingSpot.ParentNode;
                            if (parkingSpots.ChildNodes.Count > 1) // 9th Scenario: if the <parkInfo> node has more <parkingSpot> nodes than just the older <parkingSpot>
                            {
                                parkingSpots.RemoveChild(oldParkingSpot); //remove old <parkingSpot>
                                parkNode["parkingSpots"].RemoveAll(); //remove all other <parkingSpot> nodes and older <parkingSpot> => <parkInfo> clone node with just the park data
                            }
                            else // 10th Scenario: if the <parkInfo> from another provider has just the <parkingSpot> node with older data 
                            {
                                node.RemoveChild(parkingSpots); //removes the <parkingSpots> node
                            }
                        }
                        if (parkNode["parkingSpots"] == null)
                        {
                            XmlElement parkingSpots = docConfigFile.CreateElement("parkingSpots");
                            parkingSpots.InnerXml = parkingSpotXML;
                            parkNode.AppendChild(parkingSpots);
                        }
                        else
                        {
                            parkNode["parkingSpots"].InnerText = parkingSpotXML; //replace with new <parkingSpot>
                        }
                    }
                }
                #endregion

                #region Create <provider> node with <connectionType>, <endpoint>, <parkInfo> and <parkingSpot> with actual data 
                XmlElement providerNode = docConfigFile.CreateElement("provider");
                XmlElement connectionTypeNode = docConfigFile.CreateElement("connectionType");
                connectionTypeNode.InnerText = connectionType;
                XmlElement endpointNode = docConfigFile.CreateElement("endpoint");
                endpointNode.InnerXml = endpoint;
                providerNode.AppendChild(connectionTypeNode);
                providerNode.AppendChild(endpointNode);
                providerNode.AppendChild(parkNode);

                XmlNode root = docConfigFile.SelectSingleNode("/parkingLocation");
                root.AppendChild(providerNode);
                #endregion
            }
            docConfigFile.Save(m_strFilenameParkingNodesXML);

            #region UpdateParkingSpotLocation
            if (!UpdateLocationParkingSpot(location, parkId, spotId, endpoint))
            {
                Console.WriteLine("ERROR: Location parameter of requested parking spot not updated.");
                return false;
            }
            else
            {
                Console.WriteLine("SUCCESS: Location parameter of requested parking spot updated.");
            }
            #endregion

            #region RemoveUnnecessaryInfoParkingSpot
            if (!RemoveUnnecessaryInfoParkingSpot(parkId, spotId, endpoint))
            {
                Console.WriteLine("ERROR: Id parameter of requested parking spot not removed.");
                return false;
            }
            else
            {
                Console.WriteLine("SUCCESS: Id parameter of requested parking spot removed.");
            }
            #endregion

            return true;
        } 

        public static string GetGeoLocationFilename(string parkId)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_strFilenameParkingNodesXML);

            string xPath = "/parkingLocation/provider/parkInfo[id = " + "'" + parkId + "']";
            XmlNode node = doc.SelectSingleNode(xPath);

            if (node == null)
            {
                return null;
            }
            return node["geoLocationFile"].InnerText;
        }

        public static int[] GetSpotCellFromExcelFile(string filename, string spotId)
        {
            //Creates an Excel Application instance
            Application excelApplication = new Application();
            excelApplication.Visible = true;

            //Opens the excel file
            Workbook excelWorkbook = excelApplication.Workbooks.Open(filename);
            Worksheet excelWorksheet = excelWorkbook.ActiveSheet;

            Range SpotIdRange = excelWorksheet.Range["A5"];
            Range searchedRange = SpotIdRange.Find(spotId);

            int[] cell = null;
            if (searchedRange != null)
            {
                cell = new int[] { searchedRange.Row, searchedRange.Column };
            }
            
            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);
            ReleaseCOMObjects(excelApplication);
            ReleaseCOMObjects(excelWorksheet);

            return cell;
        }

        public static string GetLocationFromExcelFile(string filename, int row, int col)
        {
            //Creates an Excel Application instance
            Application excelApplication = new Application();
            excelApplication.Visible = true;

            //Opens the excel file
            Workbook excelWorkbook = excelApplication.Workbooks.Open(filename);
            Worksheet excelWorksheet = excelWorkbook.ActiveSheet;
            
            Range locationRange = excelWorksheet.Cells[row, col + 1];
            string location = locationRange.Value;

            //"eliminates" the instances
            excelWorkbook.Close();
            excelApplication.Quit();

            ReleaseCOMObjects(excelWorkbook);
            ReleaseCOMObjects(excelApplication);
            ReleaseCOMObjects(excelWorksheet);

            return location;
        }

        private static void ReleaseCOMObjects(object excelWorkbook)
        {
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelWorkbook);
            excelWorkbook = null;

            GC.Collect();
        }

        public static Boolean UpdateLocationParkingSpot(string location, string parkId, string spotId, string endpoint)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_strFilenameParkingNodesXML);

            string xPath = "/parkingLocation/provider[endpoint = " + "'" + endpoint + "']/parkInfo[id = " + "'" + parkId + "']/parkingSpots/parkingSpot[name = " + "'" + spotId + "']";
            XmlNode node = doc.SelectSingleNode(xPath);

            if (node == null)
            {
                return false;
            }

            node["location"].InnerText = location;
            doc.Save(m_strFilenameParkingNodesXML);
            
            return true;
        }

        public static Boolean RemoveUnnecessaryInfoParkingSpot(string parkId, string spotId, string endpoint)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(m_strFilenameParkingNodesXML);

            string xPath = "/parkingLocation/provider[endpoint = " + "'" + endpoint + "']/parkInfo[id = " + "'" + parkId + "']/parkingSpots/parkingSpot[name = " + "'" + spotId + "']";
            XmlNode node = doc.SelectSingleNode(xPath);

            if (node == null)
            {
                return false;
            }
            //remove child element <id> of parkingSpot
            XmlElement id = node["id"];
            node.RemoveChild(id);
            doc.Save(m_strFilenameParkingNodesXML);
            return true;
        }

        public static void SendConfigFile(CancellationToken ct, Task task)
        {
            Stopwatch timer;
            while (true)
            {
                try
                {
                    ct.ThrowIfCancellationRequested();

                    XmlDocument docConfigFile = new XmlDocument();
                    docConfigFile.Load(m_strFilenameParkingNodesXML);
                    Console.WriteLine("Config File Xml to send: " + docConfigFile.OuterXml);
                    
                    XmlNode root = docConfigFile.SelectSingleNode("/parkingLocation");
                    int refreshRate = Convert.ToInt32(root.Attributes["refreshRate"].Value);
                    string units = root.Attributes["units"].Value;
                    TimeSpan time;

                    switch (units)
                    {
                        case "milliseconds":
                            time = TimeSpan.FromMilliseconds(refreshRate);
                            break;
                        case "seconds":
                            time = TimeSpan.FromSeconds(refreshRate);
                            break;
                        case "minutes":
                            time = TimeSpan.FromMinutes(refreshRate);
                            break;
                        case "hours":
                            time = TimeSpan.FromHours(refreshRate);
                            break;
                        case "days":
                            time = TimeSpan.FromDays(refreshRate);
                            break;
                        default: //if the config file does not define a specific time, it will be sent at every minute
                            time = TimeSpan.FromMinutes(1);
                            break;
                    }

                    ct.ThrowIfCancellationRequested();

                    #region Connection with PARK TU
                    Console.WriteLine("Sending Config file XML to Park TU...");

                    string strMsgToSend = docConfigFile.OuterXml;

                    m_cClient.Publish(STR_CHANNEL_NAME, Encoding.UTF8.GetBytes(strMsgToSend));
                    #endregion

                    ct.ThrowIfCancellationRequested();

                    timer = new Stopwatch();
                    timer.Start();

                    while (timer.Elapsed <= time)
                    {
                        Thread.Sleep(1000); //verifies every second if stop connection token is up 
                        ct.ThrowIfCancellationRequested();
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine("Stop connection requested...");
                    return;
                }
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("*******************************  PARK DACE  *******************************");

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

                Console.WriteLine("---------------- GENERATE PARKING SPOT DLL -----------------------\n");

                #region ParkingSpotDLL
                //Inicialize DLL data generation
                dll.Initialize(CreateParkingSpotDLL, 30000);
                #endregion

                Console.WriteLine("---------------- GENERATE PARKING SPOT SOAP ----------------------\n");
                CreateParkingSpotSOAP();

                var tokenSource = new CancellationTokenSource();
                CancellationToken ct = tokenSource.Token;
                Task task = null;
                
                while (true)
                {
                    try
                    {
                        task = Task.Factory.StartNew(() => SendConfigFile(ct, task), tokenSource.Token);

                        do
                        {
                            Console.WriteLine("Press 'ESC' to stop connection.");

                        } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

                        tokenSource.Cancel();

                        task.Wait();

                        bool a = task.IsCanceled;

                        if (!task.IsCompleted)
                        {
                            throw new TaskCanceledException();
                        }
                    }
                    catch (TaskCanceledException ex)
                    {
                        Console.WriteLine("Not cancelled. Please try again.");
                        continue;
                    }
                    finally
                    {
                        task.Dispose();
                        tokenSource.Dispose();
                    }

                    break;
                }

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

                Console.WriteLine("Disconnected. Other applications to disconnect: PARK TU & PARK SS");
                Console.WriteLine("Press a key to exit.");
                Console.ReadKey();
            }
            catch (MqttClientException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
