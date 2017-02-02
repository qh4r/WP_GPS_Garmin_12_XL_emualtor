namespace Garmin12.Models
{
    using Garmin12.Extensions;

    public class GpsPosition
    {
        public GpsPosition(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Altitude = 0;
            this.Speed = -1.0;
        }

        public GpsPosition(double latitude, double longitude, double altitude, double? speed)
        {
            this.Altitude = altitude;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Speed = speed ?? -1.0;
        }

        public double Latitude { get; }

        public double Longitude { get; }
        
        public double LatitudeRad => this.Latitude.DegreeToRadians();

        public double LongitudeRad => this.Longitude.DegreeToRadians();

        public double Altitude { get; }

        public double Speed { get; }
    }
}