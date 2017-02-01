using GalaSoft.MvvmLight;

namespace Garmin12.ViewModel
{
    using System;
    using System.Diagnostics;
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
        private readonly LocationService locationService;

        private readonly CompassService compassService;

        private readonly NavigationService navigationService;

        private readonly DirectionService directionService;

        private GpsPosition position;

        private CompassData compassDirection;

        private double distanceToTarget;

        private double offsetFromNorth;

        public MainViewModel(LocationService locationService,
            DataService dataDataService,
            SelectedPositionStore selectedPositionStore,
            CompassService compassService,
            NavigationService navigationService,
            DirectionService directionService)
        {
            this.Position = new GpsPosition(0, 0);
            this.CompassDirection = new CompassData(0);
            this.OffsetFromNorth = 0;
            this.locationService = locationService;
            this.compassService = compassService;
            this.navigationService = navigationService;
            this.directionService = directionService;
            this.PositionStore = selectedPositionStore;
            this.DataService = dataDataService;
            this.locationService.LocationUpdate += this.OnLocationUpdate;
            this.compassService.OnCompassReading += this.CompassReadingUpdate;
            directionService.NavigationDataUpdate += this.OnNavigationDataUpdate;
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
            }
        }

        public string PositionFormatted => $"X: {this.Position.Latitude},\nY: {this.Position.Longitude}";



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

        public double CompassDirectionNormalized => 360 - this.CompassDirection.North;

        public double TargetDirection => this.PositionStore.IsPositionSelected ? this.CompassDirectionNormalized + this.OffsetFromNorth : 0;

        public RelayCommand GoToPointCreation => new RelayCommand(() => this.navigationService.NavigateTo("newPosition"));

        public RelayCommand ClearFilter => new RelayCommand(() => this.DataService.NameFilter = string.Empty);

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
    }
}