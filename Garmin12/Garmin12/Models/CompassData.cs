using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Models
{
    public class CompassData
    {
        public CompassData(double north)
        {
            this.North = north;
        }
        public double North { get; }
    }
}
