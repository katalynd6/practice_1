using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace tebenkova_wpf.Converters
{
    public class StatusColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorHex)
            {
                return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorHex));
            }

            if (value is string status)
            {
                switch (status)
                {
                    case "Норма":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
                    case "Требуется закуп":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));
                    case "Отсутствует":
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F44336"));
                    default:
                        return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                }
            }

            return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}