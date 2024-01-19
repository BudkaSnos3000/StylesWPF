using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StylesWPF
{
    public class MonthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int month = (int)value;
            return DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).Select((m, i) => new { Index = i + 1, Name = m })
                .OrderBy(m => m.Index)
                .Select(m => new { m.Index, m.Name })
                .ToList();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
