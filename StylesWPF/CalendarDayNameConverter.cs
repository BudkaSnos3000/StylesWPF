using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace StylesWPF
{
    public class CalendarDayNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length > 0 && values[0] is int index)
            {
                var dayNames = culture?.DateTimeFormat?.AbbreviatedDayNames;
                if (dayNames?.Length > 0)
                {
                    var firstDayOfWeek = values.Length > 1 && values[1] is DayOfWeek d ? d : culture.DateTimeFormat.FirstDayOfWeek;
                    var dayName = dayNames[(index + (int)firstDayOfWeek) % dayNames.Length];

                    // Сделать первую букву заглавной
                    return char.ToUpper(dayName[0]) + dayName.Substring(1);
                }
            }

            return DependencyProperty.UnsetValue;
        }


        public object[]? ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => null;

        public CalendarDayNameConverter()
        {
            //Это нужно для установки культуры. 
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage("ru-RU")));
        }
    }
}
