using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StylesWPF
{
    public class CalendarDayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var daynames = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;
            string dayname = value.ToString();

            return daynames.First(t => t.StartsWith(dayname)).Substring(0, 2);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
