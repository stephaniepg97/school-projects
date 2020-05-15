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
    [RoutePrefix("api/parkingSpots")]
    public class ParkingSpotsController : ApiController
    {
        public static string str_connectionDB = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Stephanie\Documents\IS-Projeto\Project\SmartPark\SmartPark\App_Data\CampusParkDB.mdf;Integrated Security=True";

        // GET: api/parkingSpots/[id]OR[?batteryStatus=][?spotId=][?timestamp=]

        [Route("{id:int?}")]
        public IHttpActionResult GetParkingSpots(int id = -1, string batteryStatus = null, string spotId = null, string timestamp = null, string begin = null, string end = null)
        {
            try
            {
                List<ParkingSpot> parkingSpots = new List<ParkingSpot>();
                ParkingSpot parkingSpot;
                SqlConnection connection = new SqlConnection(str_connectionDB);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "select * from ParkingSpots";
                if ((id != -1) && (timestamp == null) && (spotId == null) && (batteryStatus == null))
                {
                    cmd.CommandText += " where id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                }
                else if (id == -1)
                {
                    if (timestamp != null)
                    {
                        if ((begin != null) || (end != null))
                        {
                            return BadRequest("Too much parammeters. You have requested {begin} and/or {end} timestamps with {timestamp} parammeter.");
                        }

                        if (DateTime.Parse(timestamp.Trim()) == null)
                        {
                            return BadRequest("Timestamp invalid! Pattern example: yyyy-mm-ddThh:mm:ss.");
                        }

                        cmd.CommandText += " where timestamp = @timestamp";
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
                        cmd.CommandText += " where timestamp >= @begin and timestamp <= @end";
                        cmd.Parameters.AddWithValue("@begin", DateTime.Parse(begin.Trim()));
                        cmd.Parameters.AddWithValue("@end", DateTime.Parse(end.Trim()));
                    }

                    if (spotId != null)
                    {

                        if (!Regex.IsMatch(spotId.Trim(), "^[A-Z]-[1-9][0-9]?$"))
                        {
                            return BadRequest("{spotId} parammeter invalid! Accepts: 'A-1' to 'Z-99' patterns.");
                        }

                        cmd.CommandText += ((timestamp != null) || ((begin != null) && (end != null))) ? " and spotId = @spotId" : " where spotId = @spotId";
                        cmd.Parameters.AddWithValue("@spotId", spotId.Trim());
                    }

                    if (batteryStatus != null) //8.
                    {
                        if ((batteryStatus.Trim() != "true") && (batteryStatus.Trim() != "false"))
                        {
                            return BadRequest("{batteryStatus} parammeter invalid! Accepts only 'true' or 'false'");
                        }
                        cmd.CommandText += ((timestamp != null) || ((begin != null) && (end != null)) || (spotId != null)) ? " and batteryStatus = @batteryStatus" : " where batteryStatus = @batteryStatus";
                        cmd.Parameters.AddWithValue("@batteryStatus", (batteryStatus.Trim() == "true") ? true : false);
                    }
                }
                else if ((id != -1) && ((timestamp != null) || ((begin != null) && (end != null)) || (spotId != null) || (batteryStatus != null)))
                {
                    return BadRequest("Too much parammeters to search for a specific Parking Spot.");
                }
                else
                {
                    return BadRequest("No parammeters found to search for a specific Parking Spot.");
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

                    parkingSpots.Add(parkingSpot);
                }

                reader.Close();
                connection.Close();

                if (parkingSpots.Count == 0)
                {
                    return NotFound();
                }

                return Ok(parkingSpots);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST: api/parkingSpots
        [Route("")]
        public IHttpActionResult PostParkingSpot(ParkingSpot parkingSpot)
        {
            try
            {
                SqlConnection connection = new SqlConnection(str_connectionDB);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "insert into ParkingSpots (" +
                    "parkId, " +
                    "spotId, " +
                    "type, " +
                    "status, " +
                    "timestamp, " +
                    "batteryStatus, " +
                    "location) " +
                    "values (" +
                    "@parkId, " +
                    "@spotId, " +
                    "@type, " +
                    "@status, " +
                    "@timestamp, " +
                    "@batteryStatus, " +
                    "@location);" +
                    "SELECT CAST(scope_identity() AS int);";
                cmd.Parameters.AddWithValue("@parkId", parkingSpot.ParkId);
                cmd.Parameters.AddWithValue("@spotId", parkingSpot.SpotId);
                cmd.Parameters.AddWithValue("@type", parkingSpot.Type);
                cmd.Parameters.AddWithValue("@status", parkingSpot.Status.Value);
                cmd.Parameters.AddWithValue("@timestamp", parkingSpot.Status.Timestamp);
                cmd.Parameters.AddWithValue("@batteryStatus", parkingSpot.BatteryStatus);
                cmd.Parameters.AddWithValue("@location", parkingSpot.Location);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                connection.Open();
                Int32 id = (Int32)cmd.ExecuteScalar();
                connection.Close();
                if (id != 0)
                {
                    parkingSpot.Id = id;
                    return Ok(parkingSpot);
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

        // PUT: api/parkingSpots/{id:int}
        [Route("{id:int}")]
        public IHttpActionResult PutParkingSpot(int id, ParkingSpot parkingSpot)
        {
            try
            {
                SqlConnection connection = new SqlConnection(str_connectionDB);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "update ParkingSpots set parkId=@parkId, spotId=@spotId, type=@type, status=@status, timestamp=@timestamp, batteryStatus=@batteryStatus, location=@location where id=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@parkId", parkingSpot.ParkId);
                cmd.Parameters.AddWithValue("@spotId", parkingSpot.SpotId);
                cmd.Parameters.AddWithValue("@type", parkingSpot.Type);
                cmd.Parameters.AddWithValue("@status", parkingSpot.Status.Value);
                cmd.Parameters.AddWithValue("@timestamp", parkingSpot.Status.Timestamp);
                cmd.Parameters.AddWithValue("@batteryStatus", parkingSpot.BatteryStatus);
                cmd.Parameters.AddWithValue("@location", parkingSpot.Location);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                int nRet = cmd.ExecuteNonQuery();
                connection.Close();
                if (nRet > 0)
                {
                    return Ok(parkingSpot);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/parkingSpots/{id:int}
        [Route("{id:int}")]
        public IHttpActionResult DeleteParkingSpot(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(str_connectionDB);
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "delete from ParkingSpots where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = connection;
                int nRet = cmd.ExecuteNonQuery();
                connection.Close();
                if (nRet > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}