using GalaSoft.MvvmLight;

namespace Garmin12.ViewModel
{
    using System;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Threading;

    using Garmin12.Models;
    using Garmin12.Services;

    public class MainViewModel : ViewModelBase
    {
        private readonly LocationService locationService;

        private int counter;

        private GpsPosition position;

        private string newPositionName;

        public MainViewModel(LocationService locationService, DataService dataDataService)
        {
            this.Position = new GpsPosition(0, 0);
            this.locationService = locationService;
            this.DataService = dataDataService;
            this.Counter = 0;
            this.locationService.LocationUpdate += this.OnLocationUpdate;
        }

        public int Counter
        {
            get
            {
                return this.counter;
            }
            set
            {
                this.Set(ref this.counter, value);
            }
        }

        public RelayCommand RiseCommand => new RelayCommand(() => this.Counter += 1);

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

        public string NewPositionName
        {
            get
            {
                return this.newPositionName;
            }
            set
            {
                this.Set(ref this.newPositionName, value);
            }
        }

        public RelayCommand SavePositionCommand => new RelayCommand(
            () =>
            {
                this.DataService.SavePosition(this.NewPositionName, this.Position);
                this.NewPositionName = string.Empty;
            });

        public DataService DataService { get; }

        public RelayCommand<PositionEntity> DeletePositionCommand
            => new RelayCommand<PositionEntity>(
                position => this.DataService.DeletePositon(position));

        private async void OnLocationUpdate(GpsPosition gpsPosition)
        {
            await DispatcherHelper.RunAsync(
                () =>
                    {
                        this.Position = gpsPosition;
                    });
        }
    }
}