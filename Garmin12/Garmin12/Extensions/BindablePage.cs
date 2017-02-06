using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Garmin12.Extensions
{
    public class BindablePage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var t = e.Content;
            var q = e.Parameter;
            var navigableViewModel = this.DataContext as INavigable;
            navigableViewModel?.Activate(e.Parameter);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            var navigableViewModel = this.DataContext as INavigable;
            navigableViewModel?.Deactivate(e.Parameter);
        }
    }
}