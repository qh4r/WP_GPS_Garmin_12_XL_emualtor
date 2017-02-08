using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Garmin12.Converters
{
    public class BooleanToNegativeVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as bool? ?? false) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}