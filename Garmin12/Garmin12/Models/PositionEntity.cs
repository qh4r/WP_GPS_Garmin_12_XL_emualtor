using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Garmin12.Models
{
    using Garmin12.Extensions;

    public class PositionEntity
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }    
        public double Longitude { get; set; }
        [Ignore]
        public double LatitudeRad => this.Latitude.DegreeToRadians();
        [Ignore]
        public double LongitudeRad => this.Longitude.DegreeToRadians();
    }
}
