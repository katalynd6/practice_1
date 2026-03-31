using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace tebenkova_wpf.Converters
{
    public class DiscountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int discount = 0;

            if (value is int intValue)
            {
                discount = intValue;
            }
            else if (value is string stringValue)
            {
                string cleanValue = stringValue.Replace("%", "").Trim();
                int.TryParse(cleanValue, out discount);
            }

            if (discount == 0)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E"));
            else if (discount == 5)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CD7F32"));
            else if (discount == 10)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C0C0C0"));
            else if (discount == 15)
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD700"));
            else
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}