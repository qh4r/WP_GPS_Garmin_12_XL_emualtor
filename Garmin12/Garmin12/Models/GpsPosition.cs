namespace Garmin12.Models
{
    using Garmin12.Extensions;

    public class GpsPosition
    {
        public GpsPosition(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public double Latitude { get; }

        public double Longitude { get; }
        
        public double LatitudeRad => this.Latitude.DegreeToRadians();

        public double LongitudeRad => this.Longitude.DegreeToRadians();
    }
}