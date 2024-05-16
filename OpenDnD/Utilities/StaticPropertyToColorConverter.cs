using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenDnD.Utilities
{
    public class StaticPropertyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Type infoType && parameter is string propertyName)
            {
                var propertyInfo = infoType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static);
                if (propertyInfo != null)
                {
                    var propertyValue = propertyInfo.GetValue(null);
                    if (propertyValue is Color color)
                    {
                        return color;
                    }
                }
            }

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
