using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Services
{
    using Windows.Devices.Geolocation;

    using Garmin12.Models;

    public class LocationService
    {
        public event Action<GpsPosition> LocationUpdate;
        private readonly Geolocator geolocator;

        private GpsPosition lastPosition;

        public LocationService()
        {
            this.geolocator = new Geolocator
                                  {
                                      DesiredAccuracyInMeters = 20,
                                      MovementThreshold = 10.0,
                                      ReportInterval = 2000
                                  };

            this.geolocator.PositionChanged += this.GeolocatorOnPositionChanged;
        }

        public GpsPosition GetLastPostion()
        {
            return this.lastPosition;
        }

        private void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            this.lastPosition = new GpsPosition(
                args.Position.Coordinate.Point.Position.Latitude,
                args.Position.Coordinate.Point.Position.Longitude);
            this.LocationUpdate?.Invoke(this.lastPosition);
        }
    }
}
