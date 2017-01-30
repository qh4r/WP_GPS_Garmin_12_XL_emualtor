/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Garmin12"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Garmin12.ViewModel
{
    using Garmin12.Models;
    using Garmin12.Resources;
    using Garmin12.Services;
    using Garmin12.Store;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<CompassService>();
            SimpleIoc.Default.Register<SelectedPositionStore>();
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<PositionsStore>();
            SimpleIoc.Default.Register<Constants>();
            SimpleIoc.Default.Register<LocationService>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}