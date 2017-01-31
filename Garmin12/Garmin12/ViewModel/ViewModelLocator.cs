using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Garmin12.ViewModel
{
    using GalaSoft.MvvmLight.Views;

    using Garmin12.Models;
    using Garmin12.Resources;
    using Garmin12.Services;
    using Garmin12.Store;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {                       
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<NavigationService>();
            SimpleIoc.Default.GetInstance<NavigationService>().Configure("newPosition", typeof(NewPositionPage));
            SimpleIoc.Default.GetInstance<NavigationService>().Configure("main", typeof(MainPage));
            SimpleIoc.Default.Register<CompassService>();
            SimpleIoc.Default.Register<SelectedPositionStore>();
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<PositionsStore>();
            SimpleIoc.Default.Register<Constants>();
            SimpleIoc.Default.Register<LocationService>();
            SimpleIoc.Default.Register<NewPositionViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public NewPositionViewModel NewPosition => ServiceLocator.Current.GetInstance<NewPositionViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}