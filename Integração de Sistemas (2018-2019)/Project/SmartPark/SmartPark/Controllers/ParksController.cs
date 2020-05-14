using SmartPark.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace SmartPark.Controllers
{
    [RoutePrefix("api/parks")]
    public class ParksController : ApiController
    {
        public static string str_connectionDB = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Stephanie\Documents\IS-Projeto\Project\SmartPark\SmartPark\App_Data\CampusParkDB.mdf;Integrated Security=True";

        //1.
        [Route("{name:minlength(16):regex(^Campus_[1-5]_[A-Z]_Park[1-9]$)?}")] //6.
        public IHttpActionResult GetAllParks(string name = null)
        {
            try
            {
                SqlConnection connection = new SqlConnection(str_connectionDB);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Parks";

                if (name != null) { 
                   cmd.CommandText += " where name = @name";
                   cmd.Parameters.AddWithValue("@name", name);
                }

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                ParkInfo park;
                List<ParkInfo> parks = new List<ParkInfo>();

                while (reader.Read())
                {
                    park = new ParkInfo();
                    park.Id = (int)reader["Id"];
                    park.Name = (string)reader["Name"];
                    park.Description = (string)reader["Description"];
                    park.NumberOfSpots = (int)reader["NumberOfSpots"];
                    park.OperatingHours = (string)reader["OperatingHours"];
                    park.NumberOfSpecialSpots = (int)reader["NumberOfSpecialSpots"];
                    park.GeoLocationFile = (string)reader["GeoLocationFile"];

                    parks.Add(park);
                }

                reader.Close();
                connection.Close();

                if (parks.Count == 0)
                {
                    return BadRequest();
                }

                return Ok(parks);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{id:int:min(1)}")]
        public IHttpActionResult GetPark(int id)
        {
            try
            {
                ParkInfo park;
                SqlConnection connection = new SqlConnection(str_connectionDB);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Parks where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    park = new ParkInfo();
                    park.Id = (int)reader["Id"];
                    park.Name = (string)reader["Name"];
                    park.Description = (string)reader["Description"];
                    park.NumberOfSpots = (int)reader["NumberOfSpots"];
                    park.OperatingHours = (string)reader["OperatingHours"];
                    park.NumberOfSpecialSpots = (int)reader["NumberOfSpecialSpots"];
                    park.GeoLocationFile = (string)reader["GeoLocationFile"];
                    reader.Close();
                    connection.Close();
                    return Ok(park);
                }
                reader.Close();
                connection.Close();
                return BadRequest();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //5.
        [Route("{id:int:min(1)}/parkingSpots/{status?}")]
        public IHttpActionResult GetParkingSpotsByPark(int id, string status = null, string batteryStatus = null, string timestamp = null, string begin = null, string end = null)
        {
            try
            {
                List<ParkingSpot> parkingSpotsByPark = new List<ParkingSpot>();
                ParkingSpot parkingSpot;
                SqlConnection connection = new SqlConnection(str_connectionDB);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ParkingSpots where parkId = @parkId";
                cmd.Parameters.AddWithValue("@parkId", id);

                if (timestamp != null)
                {   
                    if ((begin != null) || (end != null))
                    {
                        return BadRequest("Too much parammeters. You have requested {begin} and/or {end} timestamps with {timestamp} parammeter.");
                    }

                    if (DateTime.Parse(timestamp.Trim()) == null)
                    {
                        return BadRequest("Timestamp invalid!");
                    }

                    //2.
                    cmd.CommandText += " and timestamp = @timestamp";
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Parse(timestamp.Trim()));
                }
                else if ((begin != null) && (end != null))
                {
                    if (DateTime.Parse(begin.Trim()) == null)
                    {
                        return BadRequest("Begin timestamp invalid! Pattern example: yyyy-mm-ddThh:mm:ss.");
                    }

                    if (DateTime.Parse(end.Trim()) == null)
                    {
                        return BadRequest("End timestamp invalid! Pattern example: yyyy-mm-ddThh:mm:ss.");
                    }
                    //3.
                    cmd.CommandText += " and timestamp >= @begin and timestamp <= @end";
                    cmd.Parameters.AddWithValue("@begin", DateTime.Parse(begin.Trim()));
                    cmd.Parameters.AddWithValue("@end", DateTime.Parse(end.Trim()));                   
                }

                if (batteryStatus != null) //8.
                {
                    if ((batteryStatus.Trim() != "true") && (batteryStatus.Trim() != "false"))
                    {
                        return BadRequest("{batteryStatus} parammeter invalid! Accepts only 'true' or 'false'");
                    }
                    cmd.CommandText += " and batteryStatus = @batteryStatus";
                    cmd.Parameters.AddWithValue("@batteryStatus", (batteryStatus.Trim() == "true") ? true : false);
                }

                if (status != null) //4.
                {
                    if ((status.Trim() != "free") && (status.Trim() != "occupied"))
                    {
                        return BadRequest("{status} parammeter invalid! Accepts only 'free' or 'occupied'");
                    }
                    cmd.CommandText += " and status = @status";
                    cmd.Parameters.AddWithValue("@status", (status.Trim() == "free") ? true : false);
                }

                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    parkingSpot = new ParkingSpot();
                    parkingSpot.Id = (int)reader["Id"];
                    parkingSpot.ParkId = (int)reader["ParkId"];
                    parkingSpot.SpotId = (string)reader["SpotId"];
                    parkingSpot.Type = (string)reader["Type"];
                    parkingSpot.Status = new Status();
                    parkingSpot.Status.Value = (bool)reader["Status"];
                    parkingSpot.Status.Timestamp = (DateTime)reader["Timestamp"];
                    parkingSpot.BatteryStatus = (bool)reader["BatteryStatus"];
                    parkingSpot.Location = (string)reader["Location"];

                    parkingSpotsByPark.Add(parkingSpot);
                }

                reader.Close();
                connection.Close();

                if (parkingSpotsByPark.Count == 0)
                {
                    return BadRequest();
                }

                return Ok(parkingSpotsByPark);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //5.
        [Route("{name:minlength(16):regex(^Campus_[1-5]_[A-Z]_Park[1-9]$)}/parkingSpots/{status?}")]
        public IHttpActionResult GetParkingSpotsByParkWithName(string name, string spotId = null, string status = null, string batteryStatus = null, string timestamp = null, string begin = null, string end = null)
        {
            try
            {
                
                SqlConnection connection = new SqlConnection(str_connectionDB);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from Parks where name = @name";
                cmd.Parameters.AddWithValue("@name", name);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                int id = -1;
                if (reader.Read())
                {
                    id = (int)reader["Id"];
                    
                }

                reader.Close();
                connection.Close();

                if (id < 1)
                {
                    return NotFound();
                }

               
                List<ParkingSpot> parkingSpotsByPark = new List<ParkingSpot>();
                ParkingSpot parkingSpot;
                connection = new SqlConnection(str_connectionDB);
                cmd = new SqlCommand();
                cmd.CommandText = "select * from ParkingSpots where parkId = @parkId";
                cmd.Parameters.AddWithValue("@parkId", id);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;

                if (timestamp != null && ((begin == null) || (end == null)))
                {
                    if ((begin != null) || (end != null))
                    {
                        return BadRequest("Too much parammeters. You have requested {begin} and/or {end} timestamps with {timestamp} parammeter.");
                    }

                    if (DateTime.Parse(timestamp.Trim()) == null)
                    {
                        return BadRequest("Timestamp invalid!");
                    }

                    //2.
                    cmd.CommandText += " and timestamp = @timestamp";
                    cmd.Parameters.AddWithValue("@timestamp", DateTime.Parse(timestamp.Trim()));
                }
                else if ((begin != null) && (end != null) && (timestamp == null))
                {
                    if (DateTime.Parse(begin.Trim()) == null)
                    {
                        return BadRequest("Begin timestamp invalid! Pattern example: yyyy-mm-ddThh:mm:ss.");
                    }

                    if (DateTime.Parse(end.Trim()) == null)
                    {
                        return BadRequest("End timestamp invalid! Pattern example: yyyy-mm-ddThh:mm:ss.");
                    }
                    //3.
                    cmd.CommandText += " and timestamp >= @begin and timestamp <= @end";
                    cmd.Parameters.AddWithValue("@begin", DateTime.Parse(begin.Trim()));
                    cmd.Parameters.AddWithValue("@end", DateTime.Parse(end.Trim()));
                }

                if (batteryStatus != null) //8.
                {
                    if ((batteryStatus.Trim() != "true") && (batteryStatus.Trim() != "false"))
                    {
                        return BadRequest("{batteryStatus} parammeter invalid! Accepts only 'true' or 'false'");
                    }
                    cmd.CommandText += " and batteryStatus = @batteryStatus";
                    cmd.Parameters.AddWithValue("@batteryStatus", (batteryStatus.Trim() == "true") ? true : false);
                }

                if (status != null) //4.
                {
                    if ((status.Trim() != "free") && (status.Trim() != "occupied"))
                    {
                        return BadRequest("{status} parammeter invalid! Accepts only 'free' or 'occupied'");
                    }
                    cmd.CommandText += " and status = @status";
                    cmd.Parameters.AddWithValue("@status", (status.Trim() == "free") ? true : false);
                }

                if (spotId != null)
                {
                    if (!Regex.IsMatch(spotId.Trim(), "^[A-Z]-[1-9][0-9]?$"))
                    {
                        return BadRequest("{spotId} parammeter invalid! Accepts: 'A-1' to 'Z-99' patterns.");
                    }
                    cmd.CommandText += " and spotId = @spotId";
                    cmd.Parameters.AddWithValue("@spotId", spotId.Trim());
                }

                connection.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    parkingSpot = new ParkingSpot();
                    parkingSpot.Id = (int)reader["Id"];
                    parkingSpot.ParkId = (int)reader["ParkId"];
                    parkingSpot.SpotId = (string)reader["SpotId"];
                    parkingSpot.Type = (string)reader["Type"];
                    parkingSpot.Status = new Status();
                    parkingSpot.Status.Value = (bool)reader["Status"];
                    parkingSpot.Status.Timestamp = (DateTime)reader["Timestamp"];
                    parkingSpot.BatteryStatus = (bool)reader["BatteryStatus"];
                    parkingSpot.Location = (string)reader["Location"];

                    parkingSpotsByPark.Add(parkingSpot);
                }

                reader.Close();
                connection.Close();

                if (parkingSpotsByPark.Count == 0)
                {
                    return BadRequest();
                }

                return Ok(parkingSpotsByPark);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //10.
        [Route("{id:int:min(1)}/occupancy")]
        public IHttpActionResult GetParkOccupancyRate(int id)
        {
            try
            {
                int total = 0, occupied = 0;
                SqlConnection connection = new SqlConnection(str_connectionDB);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ParkingSpots where status = 1 and parkId = @parkId";
                cmd.Parameters.AddWithValue("@parkId", id);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    occupied++;
                }
                

                if (occupied == 0)
                {
                    reader.Close();
                    connection.Close();
                    return Ok("0.00%");
                }

                reader.Close();
                cmd = new SqlCommand();
                cmd.CommandText = "select * from ParkingSpots where parkId = @parkId";
                cmd.Parameters.AddWithValue("@parkId", id);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    total++;
                }

                reader.Close();
                connection.Close();

                if (total == 0)
                {
                    return Ok("0.00%");
                }

                double occupancy = .00;
                occupancy = occupied *100.00/total;
                
                return Ok(occupancy.ToString()+"%");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/parks
        [Route("")]
        public IHttpActionResult PostPark(ParkInfo park)
        {
            try
            {
                SqlConnection connection = new SqlConnection(str_connectionDB);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into Parks (" +
                    "name, " +
                    "description, " +
                    "numberOfSpots, " +
                    "operatingHours, " +
                    "numberOfSpecialSpots, " +
                    "geoLocationFile) " +
                    "values (" +
                    "@name, " +
                    "@description, " +
                    "@numberOfSpots, " +
                    "@operatingHours, " +
                    "@numberOfSpecialSpots, " +
                    "@geoLocationFile);" +
                    "SELECT CAST(scope_identity() AS int);";
                cmd.Parameters.AddWithValue("@name", park.Name);
                cmd.Parameters.AddWithValue("@description", park.Description);
                cmd.Parameters.AddWithValue("@numberOfSpots", park.NumberOfSpots);
                cmd.Parameters.AddWithValue("@operatingHours", park.OperatingHours);
                cmd.Parameters.AddWithValue("@numberOfSpecialSpots", park.NumberOfSpecialSpots);
                cmd.Parameters.AddWithValue("@geoLocationFile", park.GeoLocationFile);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                Int32 id = (Int32)cmd.ExecuteScalar();
                connection.Close();
                if (id != 0)
                {
                    park.Id = id;
                    return Ok(park);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // PUT api/parks/{id:int}
        [Route("{id:int:min(1)}")]
        public IHttpActionResult PutPark(int id, ParkInfo park)
        {
            try
            {
                SqlConnection conn = new SqlConnection(str_connectionDB);
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "update Parks set name=@name, description=@description, numberOfSpots=@numberOfSpots, operatingHours=@operatingHours, numberOfSpecialSpots=@numberOfSpecialSpots, geoLocationFile=@geoLocationFile where id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", park.Name);
                cmd.Parameters.AddWithValue("@description", park.Description);
                cmd.Parameters.AddWithValue("@numberOfSpots", park.NumberOfSpots);
                cmd.Parameters.AddWithValue("@operatingHours", park.OperatingHours);
                cmd.Parameters.AddWithValue("@numberOfSpecialSpots", park.NumberOfSpecialSpots);
                cmd.Parameters.AddWithValue("@geoLocationFile", park.GeoLocationFile);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = conn;
                int nRet = cmd.ExecuteNonQuery();
                conn.Close();
                if (nRet > 0)
                {
                    return Ok(park);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE api/parks/{id:int}
        [Route("{id:int:min(1)}")]
        public IHttpActionResult DeletePark(int id)
        {
            try
            {
                #region Delete Parking Spots from that specific Park
                SqlConnection connection = new SqlConnection(str_connectionDB);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from ParkingSpots where parkId = @parkId";
                cmd.Parameters.AddWithValue("@parkId", id);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                int nRet = cmd.ExecuteNonQuery();
                connection.Close();
                if (nRet > 0)
                {
                    #region Delete Park
                    cmd.CommandText = "delete from Parks where id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    nRet = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (nRet > 0)
                    {
                        return Ok();
                    }
                    else
                    {
                        throw new Exception("Impossible to delete that specific Park.");
                    }
                    #endregion
                }
                else
                {
                    throw new Exception("Impossible to delete Parking Spots from that specific Park.");
                }
                #endregion
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}