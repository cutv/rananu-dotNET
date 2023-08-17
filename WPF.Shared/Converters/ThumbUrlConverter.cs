using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace WPF.Shared.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ThumbUrlConverter : MarkupExtension, IValueConverter
    {
        private static ThumbUrlConverter _instance;

        public ThumbUrlConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { // do not let the culture default to local to prevent variable outcome re decimal syntax
           return ((string)value).Replace("thumb", "regular");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { // read only converter...
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new ThumbUrlConverter());
        }

    }
}
