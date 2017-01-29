using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Garmin12.Models
{
    public class PositionEntity
    {
        [PrimaryKey, AutoIncrement]
        public long Id { get; set; }
        public double Latitude { get; set; }    
        public double Longitude { get; set; }
    }
}
