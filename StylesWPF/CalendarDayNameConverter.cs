using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace StylesWPF
{
    public class CalendarDayNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0 && values[0] is int index)
            {
                CultureInfo systemCulture = CultureInfo.CurrentCulture;
                var dayNames = systemCulture?.DateTimeFormat?.AbbreviatedDayNames;
                if (dayNames?.Length > 0)
                {
                    var firstDayOfWeek = values.Length > 1 && values[1] is DayOfWeek d ? d : systemCulture.DateTimeFormat.FirstDayOfWeek;
                    var day = dayNames[(index + (int)firstDayOfWeek) % dayNames.Length].ToString();
                    return day = char.ToUpper(day[0]) + day.Substring(1);
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) =>
            null;
    }
}
