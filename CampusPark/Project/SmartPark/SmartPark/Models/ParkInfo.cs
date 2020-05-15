using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SmartPark.Models
{    
    [DataContract]
    public class ParkInfo
    {
        [DataMember]
        public int Id
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }

        [DataMember]
        public int NumberOfSpots
        {
            get;
            set;
        }

        [DataMember]
        public int NumberOfSpecialSpots
        {
            get;
            set;
        }

        [DataMember]
        public string OperatingHours
        {
            get;
            set;
        }

        [DataMember]
        public string GeoLocationFile
        {
            get;
            set;
        }
    }
}
