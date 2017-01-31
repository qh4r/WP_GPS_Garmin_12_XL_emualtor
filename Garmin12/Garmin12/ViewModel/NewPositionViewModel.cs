using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Views;

    using Garmin12.Models;
    using Garmin12.Services;

    public class NewPositionViewModel : ViewModelBase
    {
        private readonly LocationService locationService;

        private readonly DataService dataService;

        private readonly NavigationService navigationService;

        private string newPositionName;

        private double longitude;

        private double latitude;

        public NewPositionViewModel(LocationService locationService, DataService dataService, NavigationService navigationService)
        {
            this.locationService = locationService;
            this.dataService = dataService;
            this.navigationService = navigationService;
            this.NewPositionName = string.Empty;
            this.Latitude = 0.0;
            this.Longitude = 0.0;
        }

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

        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                this.Set(ref this.longitude, value);
            }
        }

        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                this.Set(ref this.latitude, value);
            }
        }

        public RelayCommand BackCommand => new RelayCommand(
            () =>
                {
                    this.ClearFormData();
                    this.navigationService.GoBack();
                });

        public RelayCommand<string> SaveCommand => new RelayCommand<string>(
            name =>
                {
                    this.dataService.SavePosition(this.NewPositionName, new GpsPosition(this.Latitude, this.Longitude));
                    this.ClearFormData();
                    this.navigationService.GoBack();
                }, (name) => !string.IsNullOrWhiteSpace(this.NewPositionName));

        public RelayCommand SetCoordsCommand => new RelayCommand(
            () =>
                {
                    var currentPosition = this.locationService.GetLastPostion();
                    this.Latitude = currentPosition.Latitude;
                    this.Longitude = currentPosition.Longitude;
                });

        private void ClearFormData()
        {
            this.NewPositionName = string.Empty;
            this.Latitude = 0.0;
            this.Longitude = 0.0;
        }
    }
}
