using GalaSoft.MvvmLight;

namespace Garmin12.ViewModel
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Windows.UI.Xaml.Controls;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Threading;
    using GalaSoft.MvvmLight.Views;

    using Garmin12.Models;
    using Garmin12.Services;
    using Garmin12.Store;

    public class MainViewModel : ViewModelBase
    {
        private readonly CompassService compassService;

        private readonly NavigationService navigationService;

        private readonly DirectionService directionService;

        private readonly TimerService timerService;

        private GpsPosition position;

        private CompassData compassDirection;

        private double distanceToTarget;

        private double offsetFromNorth;

        private string timeNow;

        private int pivotIndex;

        public MainViewModel(LocationService locationService,
            DataService dataDataService,
            SelectedPositionStore selectedPositionStore,
            CompassService compassService,
            NavigationService navigationService,
            DirectionService directionService,
            TimerService timerService)
        {
            this.Position = new GpsPosition(0, 0);
            this.CompassDirection = new CompassData(0);
            this.OffsetFromNorth = 0;
            this.LocationService = locationService;
            this.compassService = compassService;
            this.navigationService = navigationService;
            this.directionService = directionService;
            this.timerService = timerService;
            this.PositionStore = selectedPositionStore;
            this.DataService = dataDataService;
            this.LocationService.LocationUpdate += this.OnLocationUpdate;
            this.compassService.OnCompassReading += this.CompassReadingUpdate;
            directionService.NavigationDataUpdate += this.OnNavigationDataUpdate;
            timerService.TimerUpdate += this.TimerServiceOnTimerUpdate;            
        }

        public string TimeNow
        {
            get
            {
                return this.timeNow;
            }
            set
            {
                this.Set(ref this.timeNow, value);
            }
        }

        public double OffsetFromNorth
        {
            get
            {
                return this.offsetFromNorth;
            }
            set
            {
                this.Set(ref this.offsetFromNorth, value);
                this.RaisePropertyChanged(() => this.TargetDirection);
            }
        }

        public double DistanceToTarget
        {
            get
            {
                return this.distanceToTarget;
            }
            set
            {
                this.Set(ref this.distanceToTarget, value);
            }
        }

        public GpsPosition Position
        {
            get
            {
                return this.position;
            }
            private set
            {
                this.Set(ref this.position, value);
                this.RaisePropertyChanged(() => this.PositionFormatted);
                this.RaisePropertyChanged(() => this.SpeedFormatted);
                this.RaisePropertyChanged(() => this.AltitudeFormatted);
            }
        }

        public string PositionFormatted => $"{this.GetLongitudeSign(this.Position.Longitude)} {Math.Round(Position.Longitude, 5)}\n{this.GetLatitudeSign(this.Position.Latitude)} {Math.Round(this.Position.Latitude, 5)}";

        public string AltitudeFormatted => $"{Math.Round(this.Position.Altitude, 5)} m {this.GetAltitudeSign(this.Position.Altitude)}";

        public string SpeedFormatted => $"{Math.Round(this.Position.Speed,2)} km/h";

        public DataService DataService { get; }

        public RelayCommand<PositionEntity> DeletePositionCommand
            => new RelayCommand<PositionEntity>(position => this.DataService.DeletePositon(position));

        public RelayCommand ItemSelectionCommand => new RelayCommand(
            () =>
                {
                    Debug.WriteLine($"{this.PositionStore.SelectedPosition?.Name} : {this.PositionStore.SelectedPosition?.Longitude} x {this.PositionStore.SelectedPosition?.Latitude}");
                });

        public SelectedPositionStore PositionStore { get; }

        public CompassData CompassDirection
        {
            get
            {
                return this.compassDirection;
            }
            set
            {
                this.Set(ref this.compassDirection, value);
                this.RaisePropertyChanged(() => this.CompassDirectionDisplay);
                this.RaisePropertyChanged(() => this.CompassDirectionNormalized);
                this.RaisePropertyChanged(() => this.TargetDirection);
            }
        }

        public string CompassDirectionDisplay => $"{this.CompassDirection.North}";

        public double CompassDirectionNormalized => (360 + this.CompassDirection.North) % 360;

        public double TargetDirection => this.PositionStore.IsPositionSelected ? (this.CompassDirection.North + this.OffsetFromNorth +360) % 360 : 0;

        public RelayCommand GoToPointCreation => new RelayCommand(() => this.navigationService.NavigateTo("newPosition"));

        public RelayCommand GoToPointUpdate => new RelayCommand(() => this.navigationService.NavigateTo("newPosition", this.PositionStore.SelectedPosition));

        public RelayCommand ClearFilter => new RelayCommand(() => this.DataService.NameFilter = string.Empty);

        public int PivotIndex
        {
            get { return pivotIndex; }
            set { Set(ref pivotIndex, value); }
        }

        public LocationService LocationService { get; }

        private async void OnLocationUpdate(GpsPosition gpsPosition)
        {
            await DispatcherHelper.RunAsync(
                () =>
                    {
                        this.Position = gpsPosition;
                    });
        }

        private async void CompassReadingUpdate(CompassData compassData)
        {
            await DispatcherHelper.RunAsync(
               () =>
               {
                   this.CompassDirection = compassData;
               });
        }

        private string GetLatitudeSign(double latitude) => latitude > 0 ? "E" : "W";

        private string GetLongitudeSign(double longitude) => longitude > 0 ? "N" : "S";

        private string GetAltitudeSign(double altitude) => altitude > 0 ? "npm" : "ppm";

        private async void TimerServiceOnTimerUpdate()
        {
            await DispatcherHelper.RunAsync(
                () =>
                {
                    this.TimeNow = DateTime.Now.ToString("HH:mm:ss");
                });
        }

        private async void OnNavigationDataUpdate(NavigationData navigationData)
        {
            await DispatcherHelper.RunAsync(
                () =>
                {
                    this.DistanceToTarget = navigationData.DistanceFromTarget;
                    this.OffsetFromNorth = navigationData.DirectionRelatedToNorth;
                });
        }

    }
}