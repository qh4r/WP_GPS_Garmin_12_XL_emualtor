using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Extensions
{
    public static class MathExtensions
    {
        public static double DegreeToRadians(this double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public static double RadianToDegree(this double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
