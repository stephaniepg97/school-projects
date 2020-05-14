using SmartPark.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;

namespace Admin_Dashboard
{
    public partial class Form1 : Form
    {
        private static string baseURI = @"http://localhost:49601/api";

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_GetAllParksOrParkingSpots_Click(object sender, EventArgs e)
        {
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            RestClient client = null;
            IRestResponse response = null;
            string content = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string option = (string) comboBox_All.SelectedItem;
            list_All.Items.Clear();

            switch (option)
            {
                case "Park":
                    client = new RestClient($"{baseURI}/parks");
                    response = client.Execute(request);
                    content = response.Content.ToString();
                    List<ParkInfo> lstParks = new List<ParkInfo>();
                    lstParks = serializer.Deserialize<List<ParkInfo>>(content);
                    
                    foreach (ParkInfo park in lstParks)
                    {
                        list_All.Items.Add($"({park.Id}) {park.Name} - {park.Description}");
                    }
                    break;
                case "Parking Spot":
                    client = new RestClient($"{baseURI}/parkingSpots");
                    response = client.Execute(request);
                    content = response.Content.ToString();
                    List<ParkingSpot> lstParkingSpots = new List<ParkingSpot>();
                    lstParkingSpots = serializer.Deserialize<List<ParkingSpot>>(content);
                    foreach (ParkingSpot parkingSpot in lstParkingSpots)
                    {
                        list_All.Items.Add($"({ parkingSpot.Id}) { parkingSpot.SpotId} - { parkingSpot.Type} (Park #{parkingSpot.ParkId})");
                    }
                    break;
            }
        }

        private void btn_GetByIDOrName_Click(object sender, EventArgs e)
        {
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            RestClient client = null;
            IRestResponse response = null;
            string content = null;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            list_ByID.Items.Clear();
            string id = text_ByID.Text.Trim();
            string name = text_ByName.Text.Trim();
            
            string type = (string)comboBox_selectType.SelectedItem;
            string option = (string)comboBox_NameOrId.SelectedItem;
            string uri = null;
            switch (type)
            {
                case "Park":
                    switch (option)
                    {
                        case "Id":
                            if (id == "")
                            {
                                MessageBox.Show($"Error: Please define an id to search!");
                                return;
                            }
                            uri = $"{baseURI}/parks/{id}";
                            break;
                        case "Name":
                            if (name == "")
                            {
                                MessageBox.Show($"Error: Please define a name to search!");
                                return;
                            }
                            if (!Regex.IsMatch(name, "^Campus_[1-5]_[A-Z]_Park[1-9]$"))
                            {
                                MessageBox.Show("Field 'name' format invalid! Example pattern: 'Campus_[1-5]_[A-Z]_Park[1-9]'");
                                return;
                            }
                            uri = $"{baseURI}/parks/{name}";
                            break;
                        default:
                            MessageBox.Show($"Error: Please choose what kind of 'search by' you want. By 'name' or by 'id'?");
                            return;
                    }
                    client = new RestClient(uri);
                    response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        content = response.Content.ToString();
                        ParkInfo park = new ParkInfo();
                        park = serializer.Deserialize<List<ParkInfo>>(content).First();
                        list_ByID.Items.Add($"{park.Name}: # of Spots/Special Spots - {park.NumberOfSpots}/{park.NumberOfSpecialSpots} ({park.OperatingHours})");
                    }
                    else
                    {
                        MessageBox.Show($"Error: Park does not exist! {response.StatusDescription}");
                    }
                    break;
                case "Parking Spot":
                    
                    switch (option)
                    {
                        case "Id":
                            if (id == "")
                            {
                                MessageBox.Show($"Error: Please define an id to search!");
                                return;
                            }
                            uri = $"{baseURI}/parkingSpots/{id}";
                            break;
                        case "Name":
                            if (name == "")
                            {
                                MessageBox.Show($"Error: Please define a name to search!");
                                return;
                            }
                            if (!Regex.IsMatch(name, "^[A-Z]-[1-9][0-9]?$"))
                            {
                                MessageBox.Show("Field 'name' format invalid! Accepts: 'A-1' to 'Z-99' patterns.");
                                return;
                            }
                            uri = $"{baseURI}/parkingSpots?spotId={name}";
                            break;
                        default:
                            MessageBox.Show($"Error: Please choose what kind of 'search by' you want. By 'name' or by 'id'?");
                            return;
                    }

                    client = new RestClient(uri);
                    response = client.Execute(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        content = response.Content.ToString();
                        ParkingSpot parkingSpot = new ParkingSpot();
                        parkingSpot = serializer.Deserialize<List<ParkingSpot>>(content).First();

                        string isFree = parkingSpot.Status.Value ? "occupied" : "free";
                        list_ByID.Items.Add($"{parkingSpot.SpotId}: {parkingSpot.Type} is {isFree} (Park #{parkingSpot.ParkId})");
                    }
                    else
                    {
                        MessageBox.Show($"Error: Parking Spot does not exist! {response.StatusDescription}");
                    }
                    break;
                default:
                    MessageBox.Show($"Error: Please choose what type of search you want. A 'Park' or a 'Parking Spot'?");
                    return;
            }
        }

        private void btn_ParkPOST_Click(object sender, EventArgs e)
        {
            ParkInfo park = new ParkInfo();

            string name = text_ParkName.Text.Trim();

            if (!Regex.IsMatch(name, "^Campus_[1-5]_[A-Z]_Park[1-9]$"))
            {
                MessageBox.Show("Field 'name' format invalid! Example pattern: 'Campus_[1-5]_[A-Z]_Park[1-9]'");
                return;
            }

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
                    Console.WriteLine($"ERROR: Park with name {name} found duplicated on database. Press OK to resolve it...");
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
                            MessageBox.Show($"OK: Park Deleted ID: {@for[i].Id}");
                        }
                        else
                        {
                            MessageBox.Show($"ERROR: {response.StatusDescription}");
                        }
                    }
                    #endregion
                    MessageBox.Show($"Info: Remaining Park ID: {@for[i].Id}");
                }
                else
                {
                    park = parks.First();
                    MessageBox.Show($"Error: Park with that name already exists! Park Found Id: {park.Id}");
                }
                #endregion
            }
            else
            {
                MessageBox.Show("Park still does not exist!");
                #region Create ParkInfo Data Contact
                park.Name = name;
                park.Description = text_ParkDescription.Text.Trim();
                park.NumberOfSpots = Convert.ToInt32(numericUpDown_ParkNumberOfSpots.Text);
                park.OperatingHours = text_ParkOperatingHours.Text.Trim();
                park.NumberOfSpecialSpots = Convert.ToInt32(numericUpDown_ParkNumberOfSpecialSpots.Text);
                park.GeoLocationFile = text_ParkGeoLocationFile.Text.Trim();
                #endregion

                #region POST Park On Database
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string content = serializer.Serialize(park);

                client = new RestClient($"{baseURI}/parks");
                request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("accept", "application/json");
                request.AddParameter("application/json; charset=utf-8", content, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    str_response = response.Content.ToString();
                    park = serializer.Deserialize<ParkInfo>(str_response);
                    MessageBox.Show($"Success: Park Created Id: {park.Id}");
                }
                else
                {
                    MessageBox.Show($"Error: Park not created! {response.StatusDescription}");
                }
                #endregion
            }
        }

        private void btn_ParkPUT_Click(object sender, EventArgs e)
        {
            string name = text_ParkName.Text.Trim();
            if (!Regex.IsMatch(name, "^Campus_[1-5]_[A-Z]_Park[1-9]$"))
            {
                MessageBox.Show("Field 'name' format invalid! Example pattern: 'Campus_[1-5]_[A-Z]_Park[1-9]'");
                return;
            }

            #region Create ParkInfo Data Contact
            ParkInfo park = new ParkInfo();
            park.Name = name;
            park.Description = text_ParkDescription.Text.Trim();
            park.NumberOfSpots = Convert.ToInt32(numericUpDown_ParkNumberOfSpots.Text);
            park.OperatingHours = text_ParkOperatingHours.Text.Trim();
            park.NumberOfSpecialSpots = Convert.ToInt32(numericUpDown_ParkNumberOfSpecialSpots.Text);
            park.GeoLocationFile = text_ParkGeoLocationFile.Text.Trim();
            #endregion

            #region PUT Park On Database
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string content = serializer.Serialize(park);

            string id = text_ParkID.Text.Trim();
            if (id == null)
            {
                MessageBox.Show($"Error: Please define the id of the Park that you want to update!");
                return;
            }
            var client = new RestClient($"{baseURI}/parks/{id}");
            var request = new RestRequest();
            request.Method = Method.PUT;
            request.AddHeader("accept", "application/json");
            request.AddParameter("application/json; charset=utf-8", content, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show($"Success: Park Updated Id: {id}");
            }
            else
            {
                MessageBox.Show($"Error: Park not updated! {response.StatusDescription}");
            }
            #endregion
        }

        private void btn_ParkDELETE_Click(object sender, EventArgs e)
        {
            string id = text_ParkID.Text.Trim();
            if (id == null)
            {
                MessageBox.Show($"Error: Please define the id of the Park that you want to delete!");
                return;
            }
            var client = new RestClient($"{baseURI}/parks/{id}");
            var request = new RestRequest();
            request.Method = Method.DELETE;
            request.AddHeader("accept", "application/json");
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show($"Success: Park Deleted Id: {id}");
            }
            else
            {
                MessageBox.Show($"Error: Park not deleted! {response.StatusDescription}");
            }

        }

        private void btn_SpotPOST_Click(object sender, EventArgs e)
        {
            string parkId = textBox_SpotParkID.Text.Trim();
            string timestamp = dateTimePicker_SpotDateTime.Text.Trim();
            string spotId = text_SpotSpotID.Text.Trim();
            if (!Regex.IsMatch(spotId, "^[A-Z]-[1-9][0-9]?$"))
            {
                MessageBox.Show("Field 'Spot Id' format invalid! Accepts: 'A-1' to 'Z-99' patterns.");
                return;
            }
            var client = new RestClient($"{baseURI}/parks/{parkId}/parkingSpots?spotId={spotId}&timestamp={timestamp}");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            var response = client.Execute(request);
            ParkingSpot parkingSpot = null;

            //show response
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                #region If the Parking Spot already exists
                string str_response = response.Content.ToString();
                List<ParkingSpot> parkingSpots = scriptSerializer.Deserialize<List<ParkingSpot>>(str_response);
                if (parkingSpots.Count > 1)
                {
                    Console.WriteLine("ERROR: Parking Spot found duplicated on database. Press OK to resolve it...");
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
                            MessageBox.Show($"OK: Parking Spot Deleted ID: {@for[i].Id}");
                        }
                        else
                        {
                            MessageBox.Show($"ERROR: {response.StatusDescription}");
                        }
                    }
                    #endregion
                    MessageBox.Show($"Info: Remaining Parking Spot ID: {@for[i].Id}");
                }
                else
                {
                    parkingSpot = parkingSpots.First();
                    MessageBox.Show($"OK: Parking Spot Found ID: {parkingSpot.Id}");
                }
                #endregion
            }
            else
            {
                #region Create Parking Spot Data Contact
                parkingSpot = new ParkingSpot();
                parkingSpot.ParkId = Convert.ToInt32(textBox_SpotParkID.Text.Trim());
                parkingSpot.SpotId = spotId;
                parkingSpot.Type = comboBox_SpotType.Text;
                parkingSpot.Status = new Status();
                parkingSpot.Status.Value = Convert.ToBoolean(comboBox_SpotStatus.Text);
                parkingSpot.Status.Timestamp = DateTime.Parse(dateTimePicker_SpotDateTime.Text);
                parkingSpot.BatteryStatus = Convert.ToBoolean(comboBox_SpotBatteryStatus.Text);
                parkingSpot.Location = text_SpotLocation.Text.Trim();
                #endregion

                #region POST Parking Spot On Database
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                string content = serializer.Serialize(parkingSpot);
                client = new RestClient($"{baseURI}/parkingSpots");
                request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("accept", "application/json");
                request.AddParameter("application/json; charset=utf-8", content, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;

                response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string str_response = response.Content.ToString();
                    parkingSpot = serializer.Deserialize<ParkingSpot>(str_response);
                    MessageBox.Show($"Success: Parking Spot Created Id: {parkingSpot.Id}");
                }
                else
                {
                    MessageBox.Show($"Error: Parking Spot not created! {response.ErrorMessage}");
                }
                #endregion
            }
        }

        private void btn_SpotPUT_Click(object sender, EventArgs e)
        {
            string spotId = text_SpotSpotID.Text.Trim();
            if (!Regex.IsMatch(spotId, "^[A-Z]-[1-9][0-9]?$"))
            {
                MessageBox.Show("Field 'Spot Id' format invalid! Accepts: 'A-1' to 'Z-99' patterns.");
                return;
            }
            #region Create Parking Spot Data Contact
            ParkingSpot parkingSpot = new ParkingSpot();
            parkingSpot.ParkId = Convert.ToInt32(textBox_SpotParkID.Text.Trim());
            parkingSpot.SpotId = spotId;
            parkingSpot.Type = comboBox_SpotType.Text;
            parkingSpot.Status = new Status();
            parkingSpot.Status.Value = Convert.ToBoolean(comboBox_SpotStatus.SelectedItem.ToString());
            parkingSpot.Status.Timestamp = DateTime.Parse(dateTimePicker_SpotDateTime.Text);
            parkingSpot.BatteryStatus = Convert.ToBoolean(comboBox_SpotBatteryStatus.SelectedItem.ToString());
            parkingSpot.Location = text_SpotLocation.Text.Trim();
            #endregion

            #region PUT Parking Spot On Database
            JavaScriptSerializer ser = new JavaScriptSerializer();
            string content = ser.Serialize(parkingSpot);
            string id = text_SpotID.Text.Trim();
            if (id == null)
            {
                MessageBox.Show($"Error: Please define the id of the Parking Spot that you want to update!");
                return;
            }
            var client = new RestClient($"{baseURI}/parkingSpots/{id}");
            var request = new RestRequest();
            request.Method = Method.PUT;
            request.AddHeader("accept", "application/json");
            request.AddParameter("application/json; charset=utf-8", content, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show($"Success: Parking Spot Updated Id: {id}");
            }
            else
            {
                MessageBox.Show("Error: Parking Spot not updated!");
            }
            #endregion
        }

        private void btn_SpotDELETE_Click(object sender, EventArgs e)
        {
            string id = text_SpotID.Text.Trim();
            if (id == null)
            {
                MessageBox.Show($"Error: Please define the id of the Parking Spot that you want to delete!");
                return;
            }
            var client = new RestClient($"{baseURI}/parkingSpots/{id}");
            var request = new RestRequest();
            request.Method = Method.DELETE;
            request.AddHeader("accept", "application/json");
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show($"Success: Parking Spot Deleted Id: {id}");
            }
            else
            {
                MessageBox.Show($"Error: Parking Spot not deleted! {response.ErrorMessage}");
            }
        }

        private void btn_GetParkingSpotsSpecificMoment_Click(object sender, EventArgs e)
        {
            string timestamp = DateTime.Parse(dateTime_SpecificMoment.Text).ToString("o");
            list_All.Items.Clear();

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var client = new RestClient($"{baseURI}/parkingSpots?timestamp={timestamp}");
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = response.Content.ToString();
                List<ParkingSpot> lstParkingSpots = new List<ParkingSpot>();
                lstParkingSpots = serializer.Deserialize<List<ParkingSpot>>(content);
                foreach (ParkingSpot parkingSpot in lstParkingSpots)
                {
                    list_All.Items.Add($"({ parkingSpot.Id}) { parkingSpot.SpotId} - { parkingSpot.Type} (Park #{parkingSpot.ParkId})");
                }
            }
            else
            {
                MessageBox.Show($"Error: No Parking Spots detected! {response.StatusDescription}");
            }
        }

        private void btn_GetParkingSpotsSpecificTimePeriod_Click(object sender, EventArgs e)
        {
            string start = DateTime.Parse(dateTime_Start.Text).ToString("o");
            string end = DateTime.Parse(dateTime_End.Text).ToString("o");
            list_All.Items.Clear();

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var client = new RestClient($"{baseURI}/parkingSpots?begin={start}&end={end}");
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = response.Content.ToString();
                List<ParkingSpot> lstParkingSpots = new List<ParkingSpot>();
                lstParkingSpots = serializer.Deserialize<List<ParkingSpot>>(content);
                foreach (ParkingSpot parkingSpot in lstParkingSpots)
                {
                    list_All.Items.Add($"({ parkingSpot.Id}) { parkingSpot.SpotId} - { parkingSpot.Type} (Park #{parkingSpot.ParkId})");
                }
            }
            else
            {
                MessageBox.Show($"Error: No Parking Spots detected! {response.StatusDescription}");
            }
        }

        private void btn_GetParkingSpotsSpecificPark_Click(object sender, EventArgs e)
        {
            string option = (string)comboBox_NameOrIdByPark.SelectedItem;
            string parkName = text_ParkByName.Text.Trim();
            string parkId = text_ParkByID.Text.Trim();

            string uri = null;

            switch (option)
            {
                case "Name":
                    if (!Regex.IsMatch(parkName, "^Campus_[1-5]_[A-Z]_Park[1-9]$"))
                    {
                        MessageBox.Show("Field 'name' format invalid! Example pattern: 'Campus_[1-5]_[A-Z]_Park[1-9]'");
                        return;
                    }
                    uri = $"{baseURI}/parks/{parkName}/parkingSpots";
                    break;
                case "Id":
                    if (parkId == null)
                    {
                        MessageBox.Show($"Error: Please define the id of the Park!");
                        return;
                    }
                    uri = $"{baseURI}/parks/{parkId}/parkingSpots";
                    break;
                default:
                    MessageBox.Show($"Error: Please choose what kind of search by Park you want. By 'name' or by 'id'?");
                    return;
            }

            list_All.Items.Clear();

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var client = new RestClient(uri);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = response.Content.ToString();
                List<ParkingSpot> lstParkingSpots = new List<ParkingSpot>();
                lstParkingSpots = serializer.Deserialize<List<ParkingSpot>>(content);
                foreach (ParkingSpot parkingSpot in lstParkingSpots)
                {
                    list_All.Items.Add($"({ parkingSpot.Id}) { parkingSpot.SpotId} - { parkingSpot.Type} (Park #{parkingSpot.ParkId})");
                }
            }
            else
            {
                MessageBox.Show($"Error: No Parking Spots detected! {response.StatusDescription}");
            }

        }

        private void btn_GetParkingSpotsFromParkSpecificMoment_Click(object sender, EventArgs e)
        {
            string option = (string)comboBox_NameOrIdByPark.SelectedItem;

            string parkName = text_ParkByName.Text.Trim();
            string parkId = text_ParkByID.Text.Trim();
            string timestamp = DateTime.Parse(dateTime_SpecificMoment.Text).ToString("o");
            string uri = null;

            switch (option)
            {
                case "Name":
                    if (!Regex.IsMatch(parkName, "^Campus_[1-5]_[A-Z]_Park[1-9]$"))
                    {
                        MessageBox.Show("Field 'name' format invalid! Example pattern: 'Campus_[1-5]_[A-Z]_Park[1-9]'");
                        return;
                    }
                    uri = $"{baseURI}/parks/{parkName}/parkingSpots?timestamp={timestamp}";
                    break;
                case "Id":
                    if (parkId == null)
                    {
                        MessageBox.Show($"Error: Please define the id of the Park!");
                        return;
                    }
                    uri = $"{baseURI}/parks/{parkId}/parkingSpots?timestamp={timestamp}";
                    break;
                default:
                    MessageBox.Show($"Error: Please choose what kind of search by Park you want. By 'name' or by 'id'?");
                    return;
            }

            list_All.Items.Clear();

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var client = new RestClient(uri);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = response.Content.ToString();
                List<ParkingSpot> lstParkingSpots = new List<ParkingSpot>();
                lstParkingSpots = serializer.Deserialize<List<ParkingSpot>>(content);
                foreach (ParkingSpot parkingSpot in lstParkingSpots)
                {
                    list_All.Items.Add($"({ parkingSpot.Id}) { parkingSpot.SpotId} - { parkingSpot.Type} (Park #{parkingSpot.ParkId})");
                }
            }
            else
            {
                MessageBox.Show($"Error: No Parking Spots detected! {response.StatusDescription}");
            }

        }

        private void btn_GetParkingSpotsFromParkSpecificTimePeriod_Click(object sender, EventArgs e)
        {
            string option = (string)comboBox_NameOrIdByPark.SelectedItem;

            string parkName = text_ParkByName.Text.Trim();
            string parkId = text_ParkByID.Text.Trim();
            string start = DateTime.Parse(dateTime_Start.Text).ToString("o");
            string end = DateTime.Parse(dateTime_End.Text).ToString("o");

            string uri = null;

            switch (option)
            {
                case "Name":
                    if (!Regex.IsMatch(parkName, "^Campus_[1-5]_[A-Z]_Park[1-9]$"))
                    {
                        MessageBox.Show("Field 'name' format invalid! Example pattern: 'Campus_[1-5]_[A-Z]_Park[1-9]'");
                        return;
                    }
                    uri = $"{baseURI}/parks/{parkName}/parkingSpots?begin={start}&end={end}";
                    break;
                case "Id":
                    if (parkId == null)
                    {
                        MessageBox.Show($"Error: Please define the id of the Park!");
                        return;
                    }
                    uri = $"{baseURI}/parks/{parkId}/parkingSpots?begin={start}&end={end}";
                    break;
                default:
                    MessageBox.Show($"Error: Please choose what kind of search by Park you want. By 'name' or by 'id'?");
                    return;
            }

            list_All.Items.Clear();

            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("accept", "application/json");
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var client = new RestClient(uri);
            var response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = response.Content.ToString();
                List<ParkingSpot> lstParkingSpots = new List<ParkingSpot>();
                lstParkingSpots = serializer.Deserialize<List<ParkingSpot>>(content);
                foreach (ParkingSpot parkingSpot in lstParkingSpots)
                {
                    list_All.Items.Add($"({ parkingSpot.Id}) { parkingSpot.SpotId} - { parkingSpot.Type} (Park #{parkingSpot.ParkId})");
                }
            }
            else
            {
                MessageBox.Show($"Error: No Parking Spots detected! {response.StatusDescription}");
            }
        }
    }
}
