namespace Garmin12.Converters
{
    using System;

    using Windows.UI.Xaml.Data;

    public class BooleanToNegativeOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value as bool? ?? false) ? 0 : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}