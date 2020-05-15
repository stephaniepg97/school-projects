using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web.Script.Serialization;
using SmartPark.Models;
using RestSharp;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using System.Threading;

namespace ParkSS
{
    class Program
    {
        private static string baseURI = @"http://localhost:49601/api";
        private const String STR_CHANNEL_NAME = "PARK_SS";

        //MqttClient m_cClient = new MqttClient(IPAddress.Parse("192.168.237.155"));
        static MqttClient m_cClient = new MqttClient("127.0.0.1");
        static string[] m_strTopicsInfo = { STR_CHANNEL_NAME };

        static void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string parksXml = Encoding.UTF8.GetString(e.Message);

            if (parksXml.Trim().Length <= 0)
            {
                Console.WriteLine("Invalid message.");
                return;
            }

            Console.WriteLine("Received = " + parksXml + " on topic " + e.Topic);

            Console.WriteLine("-------------- POST PARKS AND PARKING SPOTS ON DATABASE -----------------\n");

            XmlDocument docParks = new XmlDocument();
            docParks.LoadXml(parksXml);
            XmlNodeList parkNodes = docParks.SelectNodes("//parkInfo");

            foreach (XmlNode node in parkNodes)
            {
                ParkInfo park = new ParkInfo();

                string name = node["id"].InnerText;
                var client = new RestClient($"{baseURI}/parks/{name}");
                var request = new RestRequest();
                request.Method = Method.GET;
                request.AddHeader("accept", "application/json");
                var response = client.Execute(request);

                //show response
                string str_response;
                JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    #region If the Park already exists
                    str_response = response.Content.ToString();
                    List<ParkInfo> parks = scriptSerializer.Deserialize<List<ParkInfo>>(str_response);
                    if (parks.Count > 1)
                    {
                        Console.WriteLine("ERROR: Park found duplicated on database. Try to resolve it...");
                        #region Delete duplicated rows on database
                        ParkInfo[] @for = parks.ToArray();
                        int i = parks.Count;
                        while (--i > 0)
                        {
                            client = new RestClient($"{baseURI}/parks/{@for[i].Id}");
                            request = new RestRequest();
                            request.Method = Method.DELETE;
                            request.AddHeader("accept", "application/json");
                            response = client.Execute(request);

                            //show response
                            scriptSerializer = new JavaScriptSerializer();
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                Console.WriteLine($"OK: Park Deleted ID: {@for[i].Id}");
                            }
                            else
                            {
                                Console.WriteLine($"ERROR: {response.StatusDescription}");
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        park = parks.First();
                        Console.WriteLine($"OK: Park Found Id: {park.Id}");

                    }
                    #endregion
                }
                else
                {
                    Console.WriteLine("Park still does not exist!");
                    #region Create ParkInfo Data Contact
                    park.Description = node["description"].InnerText;
                    park.GeoLocationFile = node["geoLocationFile"].InnerText;
                    park.Name = node["id"].InnerText;
                    park.NumberOfSpecialSpots = Convert.ToInt32(node["numberOfSpecialSpots"].InnerText);
                    park.NumberOfSpots = Convert.ToInt32(node["numberOfSpots"].InnerText);
                    park.OperatingHours = node["operatingHours"].InnerText;
                    #endregion

                    Console.WriteLine("POST PARK:");
                    #region POST Current Park On Database
                    string str = scriptSerializer.Serialize(park); //serialize ParkInfo Data Contract

                    client = new RestClient($"{baseURI}/parks");
                    request = new RestRequest();
                    request.Method = Method.POST;
                    request.AddHeader("accept", "application/json");
                    request.AddParameter("application/json; charset=utf-8", str, ParameterType.RequestBody);
                    request.RequestFormat = DataFormat.Json;
                    response = client.Execute(request);

                    //show response
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        str_response = response.Content.ToString();
                        park = scriptSerializer.Deserialize<ParkInfo>(str_response);
                        Console.WriteLine($"OK: Park Found Id: {park.Id}");
                    }
                    else
                    {
                        Console.WriteLine($"ERROR: {response.StatusDescription}");
                    }
                    #endregion
                }

                if (node["parkingSpots"] != null)
                {
                    #region Get ParkingSpots From Current Park
                    XmlNodeList parkingSpotsNodes = node["parkingSpots"].ChildNodes;
                    #endregion

                    ParkingSpot parkingSpot;
                    foreach (XmlNode parkingSpotNode in parkingSpotsNodes)
                    {
                        string timestamp = parkingSpotNode["status"].LastChild.InnerText;
                        string spotId = parkingSpotNode["name"].InnerText;
                        client = new RestClient($"{baseURI}/parks/{park.Id}/parkingSpots?spotId={spotId}&timestamp={timestamp}");
                        request = new RestRequest();
                        request.Method = Method.GET;
                        request.AddHeader("accept", "application/json");
                        response = client.Execute(request);

                        //show response
                        scriptSerializer = new JavaScriptSerializer();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            #region If the Parking Spot already exists
                            str_response = response.Content.ToString();
                            List<ParkingSpot> parkingSpots = scriptSerializer.Deserialize<List<ParkingSpot>>(str_response);
                            if (parkingSpots.Count > 1)
                            {
                                Console.WriteLine("ERROR: Parking Spot found duplicated on database. Try to resolve it...");
                                #region Delete duplicated rows on database
                                ParkingSpot[] @for = parkingSpots.ToArray();
                                int i = parkingSpots.Count;
                                while (--i > 0)
                                {
                                    client = new RestClient($"{baseURI}/parkingSpots/{@for[i].Id}");
                                    request = new RestRequest();
                                    request.Method = Method.DELETE;
                                    request.AddHeader("accept", "application/json");
                                    response = client.Execute(request);

                                    //show response
                                    scriptSerializer = new JavaScriptSerializer();
                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        Console.WriteLine($"OK: Parking Spot Deleted ID: {@for[i].Id}");
                                    }
                                    else
                                    {
                                        Console.WriteLine($"ERROR: {response.StatusDescription}");
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                parkingSpot = parkingSpots.First();
                                Console.WriteLine($"OK: Parking Spot Found ID: {parkingSpot.Id}");
                                
                            }
                            #endregion
                        }
                        else
                        {
                            Console.WriteLine("NOT FOUND: Parking Spot still does not exist!");
                            #region Create ParkingSpot Data Contact
                            parkingSpot = new ParkingSpot();
                            parkingSpot.Location = parkingSpotNode["location"].InnerText;
                            parkingSpot.SpotId = parkingSpotNode["name"].InnerText;
                            parkingSpot.Status = new Status();
                            parkingSpot.Status.Value = parkingSpotNode["status"].FirstChild.InnerText == "free";
                            parkingSpot.Status.Timestamp = DateTime.Parse(parkingSpotNode["status"].LastChild.InnerText);
                            parkingSpot.Type = parkingSpotNode["type"].InnerText;
                            parkingSpot.ParkId = park.Id;
                            parkingSpot.BatteryStatus = parkingSpotNode["batteryStatus"].InnerText == "1";
                            #endregion

                            Console.WriteLine("POST PARKING SPOT: ");
                            #region POST Current Parking Spot On Database
                            scriptSerializer = new JavaScriptSerializer();
                            string str = scriptSerializer.Serialize(parkingSpot);
                            client = new RestClient($"{baseURI}/parkingSpots");
                            request = new RestRequest();
                            request.Method = Method.POST;
                            request.AddHeader("accept", "application/json");
                            request.AddParameter("application/json; charset=utf-8", str, ParameterType.RequestBody);
                            request.RequestFormat = DataFormat.Json;
                            response = client.Execute(request);

                            //show response
                            if (response.StatusCode == HttpStatusCode.OK)
                            {
                                str_response = response.Content.ToString();
                                parkingSpot = scriptSerializer.Deserialize<ParkingSpot>(str_response);
                                Console.WriteLine($"OK: Parking Spot Saved ID: {parkingSpot.Id}");
                            }
                            else
                            {
                                Console.WriteLine($"ERROR: {response.StatusDescription}");
                            }
                            #endregion
                        }
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("*******************************  PARK SS  *******************************");

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

                #region Connection with ParkTU
                Console.WriteLine("Waiting to receive Parking Spots And Parks XML info from PARK TU...");

                //Subscribe chat channel
                byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };//QoS
                m_cClient.Subscribe(m_strTopicsInfo, qosLevels);
                
                do
                {
                    Console.WriteLine("Press 'ESC' to stop connection.");

                    //receive message
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

                Console.WriteLine("Disconnected. Other applications to disconnect: PARK TU & PARK DACE");
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
