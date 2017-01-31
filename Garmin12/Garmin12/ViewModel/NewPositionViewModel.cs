using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garmin12.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Views;

    using Garmin12.Services;

    public class NewPositionViewModel
    {
        private readonly LocationService locationService;

        private readonly NavigationService navigationService;

        public NewPositionViewModel(LocationService locationService, DataService dataService, NavigationService navigationService)
        {
            this.locationService = locationService;
            this.navigationService = navigationService;
        }

        public RelayCommand BackCommand => new RelayCommand(() => this.navigationService.GoBack());
    }
}
