using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPF.Shared.Converters
{
    public class StringComparatorVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return Visibility.Collapsed;
            }
            else
            {
                string stringValue = value.ToString();
                string stringParameter = parameter.ToString();
                if (stringValue.Equals(stringParameter))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
