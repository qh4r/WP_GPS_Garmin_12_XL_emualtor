using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;

namespace Garmin12.Services
{
    using Windows.Devices.Geolocation;

    using Garmin12.Models;

    public class LocationService : ViewModelBase
    {
        public event Action<GpsPosition> LocationUpdate;
        private readonly Geolocator geolocator;

        private GpsPosition lastPosition;
        private bool isLocalizationAvailable;

        public LocationService()
        {
            this.geolocator = new Geolocator
                                  {
                                      DesiredAccuracyInMeters = 20,
                                      MovementThreshold = 10.0,
                                      ReportInterval = 2000
                                  };

            this.geolocator.PositionChanged += this.GeolocatorOnPositionChanged;
            this.geolocator.StatusChanged += this.OnStatusChanged;
        }

        public bool IsLocalizationAvailable
        {
            get { return isLocalizationAvailable; }
            set { this.Set(ref isLocalizationAvailable, value); }
        }

        public GpsPosition GetLastPostion()
        {
            return this.lastPosition;
        }

        private void GeolocatorOnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {            
            this.lastPosition = new GpsPosition(
                args.Position.Coordinate.Point.Position.Latitude,
                args.Position.Coordinate.Point.Position.Longitude,
                args.Position.Coordinate.Point.Position.Altitude,
                args.Position.Coordinate.Speed);
            this.LocationUpdate?.Invoke(this.lastPosition);
        }

        private async void OnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            await DispatcherHelper.RunAsync(() => this.IsLocalizationAvailable = args.Status == PositionStatus.Ready);
        }
    }
}
