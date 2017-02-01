using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.Services
{
    using Garmin12.Extensions;
    using Garmin12.Models;
    using Garmin12.Resources;
    using Garmin12.Store;

    using static Math;

    public class DirectionService
    {
        public event Action<NavigationData> NavigationDataUpdate;

        private readonly SelectedPositionStore selectedPositionStore;

        private readonly LocationService locationService;

        private readonly Constants constants;

        public DirectionService(SelectedPositionStore selectedPositionStore, LocationService locationService, CompassService compassService, Constants constants)
        {
            this.selectedPositionStore = selectedPositionStore;
            this.locationService = locationService;
            this.constants = constants;
            locationService.LocationUpdate += x => this.LocationServiceOnLocationUpdate();
            selectedPositionStore.SelectionChanged += x => this.LocationServiceOnLocationUpdate();
            compassService.OnCompassReading += x => this.LocationServiceOnLocationUpdate();
        }

        private void LocationServiceOnLocationUpdate()
        {
            if (this.selectedPositionStore.IsPositionSelected)
            {
                var currentLocation = this.selectedPositionStore.SelectedPosition;
                var targetLocation = this.locationService.GetLastPostion();
                var direction = this.CalculateDirection(currentLocation, targetLocation);
                var distance = this.CalculateDistance(currentLocation.Longitude, currentLocation.Latitude, targetLocation.Longitude, targetLocation.Latitude);
                this.NavigationDataUpdate?.Invoke(new NavigationData
                {
                    DirectionRelatedToNorth = direction,
                    DistanceFromTarget = distance
                });
            }
        }

        public double CalculateDistance(double startLongitude, double startLatitude, double endLongitude, double endLatitude)
        {
            var longitudeDelta = (endLongitude - startLongitude).DegreeToRadians();
            var latitudeDelta = (endLatitude - startLatitude).DegreeToRadians();

            var a = (Sin(latitudeDelta / 2) * Sin(latitudeDelta / 2)) + Cos(startLatitude.DegreeToRadians()) * Cos(endLatitude.DegreeToRadians()) * (Sin(longitudeDelta / 2) * Sin(longitudeDelta / 2));
            var angle = 2 * Atan2(Sqrt(a), Sqrt(1 - a));
            return angle * this.constants.R;
        }

        private double CalculateDirection(PositionEntity currentPosition, GpsPosition targetLocation)
        {
            return Atan2(this.CalculateY(currentPosition, targetLocation), this.CalculateX(currentPosition, targetLocation)).RadianToDegree();
        }

        private double CalculateY(PositionEntity currentPosition, GpsPosition targetLocation)
        {
            return Cos(currentPosition.LatitudeRad) * Sin(targetLocation.LatitudeRad)
                   - Sin(currentPosition.LatitudeRad) * Cos(targetLocation.LatitudeRad)
                   * Cos((targetLocation.Longitude - currentPosition.Longitude).DegreeToRadians());
        }

        private double CalculateX(PositionEntity currentPosition, GpsPosition targetLocation)
        {
            return Cos(targetLocation.LatitudeRad)
                   * Sin((targetLocation.Longitude - currentPosition.Longitude).DegreeToRadians());
        }
    }
}
