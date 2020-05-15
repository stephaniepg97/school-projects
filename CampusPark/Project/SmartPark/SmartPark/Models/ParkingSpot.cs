using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace SmartPark.Models
{
    [DataContract]
    public class ParkingSpot
    {
        [DataMember]
        public int Id
        {
            get;
            set;
        }

        [DataMember]
        public bool BatteryStatus
        {
            get;
            set;
        }

        [DataMember]
        public string Type
        {
            get;
            set;
        }

        [DataMember]
        public int ParkId
        {
            get;
            set;
        }

        [DataMember]
        public string SpotId
        {
            get;
            set;
        }

        [DataMember]
        public Status Status
        {
            get;
            set;
        }

        [DataMember]
        public string Location
        {
            get;
            set;
        }
    }
}