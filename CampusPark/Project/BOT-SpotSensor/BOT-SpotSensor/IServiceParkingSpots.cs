using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BOT_SpotSensor
{
    [ServiceContract]
    public interface IServiceParkingSpots
    {
        [OperationContract]
        Boolean CreateParkingSpotXML(ParkingSpot newParkingSpot);

        [OperationContract]
        ParkingSpot GetParkingSpot();

        [OperationContract]
        string GetParkingSpotXML();
    }


    [DataContract]
    public class ParkingSpot
    {
        private string parkId;
        private Type type;
        private string spotId;
        private string location;
        private Status status;
        private bool batteryStatus;

        public ParkingSpot(string parkId, Type type, string spotId, string location, bool spotStatus, DateTime timestamp, bool batteryStatus)
        {
            this.parkId = parkId;
            this.type = type;
            this.spotId = spotId;
            this.location = location;
            this.status = new Status(spotStatus, timestamp);
            this.batteryStatus = batteryStatus;
        }

        [DataMember]
        public bool BatteryStatus
        {
            get { return this.batteryStatus; }
            set { this.batteryStatus = value; }
        }

        [DataMember]
        public Type Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        [DataMember]
        public string ParkId
        {
            get { return this.parkId; }
            set { this.parkId = value; }
        }

        [DataMember]
        public string SpotId
        {
            get { return this.spotId; }
            set { this.spotId = value; }
        }

        [DataMember]
        public Status Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        [DataMember]
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
    }

    [DataContract]
    public enum Type
    {
        [EnumMember]
        ParkingSpot,
        [EnumMember]
        SpecialSpot,
        [EnumMember]
        TeacherSpot,
        [EnumMember]
        BicycleSpot,
        [EnumMember]
        MotoSpot
    };

    [DataContract]
    public class Status
    {
        private bool value;
        private DateTime timestamp;

        public Status(bool value, DateTime timestamp)
        {
            this.value = value;
            this.timestamp = timestamp;
        }

        [DataMember]
        public bool Value
        {
            get { return this.value; }
            set { this.value = value; }
        }


        [DataMember]
        public DateTime Timestamp
        {
            get { return this.timestamp; }
            set { this.timestamp = value; }
        }
    };


}
