using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Garmin12.Extensions;

namespace Garmin12.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Views;

    using Garmin12.Models;
    using Garmin12.Services;

    public class NewPositionViewModel : ViewModelBase, INavigable
    {
        private readonly LocationService locationService;

        private readonly DataService dataService;

        private readonly NavigationService navigationService;

        private string newPositionName;

        private string longitude;

        private string latitude;
        private PositionEntity pointToEdit;

        public NewPositionViewModel(LocationService locationService, DataService dataService, NavigationService navigationService)
        {            
            this.locationService = locationService;
            this.dataService = dataService;
            this.navigationService = navigationService;            
            this.NewPositionName = string.Empty;
            this.ClearFormData();
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

        public string Longitude
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

        public string Latitude
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
                    if (PointToEdit != null)
                    {
                        this.dataService.UpdatePosition(PointToEdit.Id, this.NewPositionName, new GpsPosition(this.ToDouble(this.Latitude), this.ToDouble(this.Longitude)));
                    }
                    else
                    {
                        this.dataService.SavePosition(this.NewPositionName, new GpsPosition(this.ToDouble(this.Latitude), this.ToDouble(this.Longitude)));
                    }
                    this.navigationService.GoBack();
                }, (name) => !string.IsNullOrWhiteSpace(this.NewPositionName));

        public RelayCommand SetCoordsCommand => new RelayCommand(
            () =>
                {
                    var currentPosition = this.locationService.GetLastPostion();
                    this.Latitude = currentPosition.Latitude.ToString();
                    this.Longitude = currentPosition.Longitude.ToString();
                });

        public PositionEntity PointToEdit
        {
            get { return this.pointToEdit; }
            set { this.Set(ref this.pointToEdit, value); }
        }

        public void Activate(object parameter)
        {
            var point = parameter as PositionEntity;
            if (point != null)
            {
                this.PointToEdit = point;
                this.SetFormData(this.PointToEdit);
            }           
        }

        public void Deactivate(object parameter)
        {
            this.ClearFormData();
            this.PointToEdit = null;
        }

        private void SetFormData(PositionEntity point)
        {
            this.NewPositionName = point.Name;
            this.Latitude = point.Latitude.ToString();
            this.Longitude = point.Longitude.ToString();
        }

        private void ClearFormData()
        {
            this.NewPositionName = string.Empty;
            this.Latitude = string.Empty;
            this.Longitude = string.Empty;
        }

        private double ToDouble(string value)
        {
            double output;
            var result =  double.TryParse(value, out output) ? output : 0;
            return result;
        }
    }
}
