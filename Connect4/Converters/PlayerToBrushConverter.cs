using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Connect4.Models;

namespace Connect4.Converters;

public class PlayerToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Player player)
            return Brushes.Transparent;

        return player switch
        {
            Player.Red => new SolidColorBrush(Color.FromRgb(211, 47, 47)),
            Player.Yellow => new SolidColorBrush(Color.FromRgb(251, 192, 45)),
            _ => new SolidColorBrush(Color.FromRgb(227, 242, 253))
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}

public class WinningCellBorderConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 || values[0] is not bool isWinning)
            return Brushes.Transparent;

        return isWinning ? Brushes.White : Brushes.Transparent;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
