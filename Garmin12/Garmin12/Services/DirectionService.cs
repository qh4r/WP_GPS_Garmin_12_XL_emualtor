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

        private readonly TimerService timerService;

        public DirectionService(SelectedPositionStore selectedPositionStore, LocationService locationService, CompassService compassService, Constants constants, TimerService timerService)
        {
            this.selectedPositionStore = selectedPositionStore;
            this.locationService = locationService;
            this.constants = constants;
            this.timerService = timerService;
            timerService.TimerUpdate += this.LocationServiceOnLocationUpdate;
        }

        private void LocationServiceOnLocationUpdate()
        {
            if (this.selectedPositionStore.IsPositionSelected)
            {
                var currentLocation = this.locationService.GetLastPostion();
                var targetLocation = this.selectedPositionStore.SelectedPosition;
                if (currentLocation != null && targetLocation != null)
                {
                    var direction = this.NormalizeDirection(this.CalculateDirection(currentLocation, targetLocation));
                    var distance = this.CalculateDistance(
                        currentLocation.Longitude,
                        currentLocation.Latitude,
                        targetLocation.Longitude,
                        targetLocation.Latitude);
                    this.NavigationDataUpdate?.Invoke(new NavigationData
                    {
                        DirectionRelatedToNorth = direction,
                        DistanceFromTarget = distance
                    });
                }
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

        private double CalculateDirection(GpsPosition currentPosition, PositionEntity targetLocation)
        {
            return Atan2(this.CalculateX(currentPosition, targetLocation), this.CalculateY(currentPosition, targetLocation)).RadianToDegree();
        }

        private double CalculateY(GpsPosition currentPosition, PositionEntity targetLocation)
        {
            return Cos(currentPosition.LatitudeRad) * Sin(targetLocation.LatitudeRad)
                   - Sin(currentPosition.LatitudeRad) * Cos(targetLocation.LatitudeRad)
                   * Cos(targetLocation.LongitudeRad - currentPosition.LongitudeRad);
        }

        private double CalculateX(GpsPosition currentPosition, PositionEntity targetLocation)
        {
            return Cos(targetLocation.LatitudeRad)
                   * Sin(targetLocation.LongitudeRad - currentPosition.LongitudeRad);
        }

        private double NormalizeDirection(double direction)
        {
            //return (direction + 360) % 360;
            return direction;
        }
    }
}
