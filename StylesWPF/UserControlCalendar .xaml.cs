using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StylesWPF
{
    /// <summary>
    /// Логика взаимодействия для UserControlCalendar.xaml
    /// </summary>
    public partial class UserControlCalendar : UserControl
    {
        public ObservableCollection<string> WeekDays { get; set; }
        public UserControlCalendar()
        {
            InitializeComponent();
            Loaded += UserControl_Loaded;

            System.Windows.Controls.Calendar wpfCalendar = FindName("MyCalendar") as System.Windows.Controls.Calendar;
            wpfCalendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
            WeekDays = new ObservableCollection<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.Calendar wpfCalendar = sender as System.Windows.Controls.Calendar;
            DateTime selectedDate = wpfCalendar.SelectedDate.HasValue ? wpfCalendar.SelectedDate.Value : DateTime.MinValue;
            DateSelected?.Invoke(this, selectedDate);
        }

        public event EventHandler<DateTime> DateSelected;

        private void MonthCombo_Loaded(object sender, RoutedEventArgs e)
        {
            // Получаем массив строк с названиями месяцев
            string[] months = DateTimeFormatInfo.CurrentInfo.MonthNames;

            // Находим ComboBox в визуальном дереве
            ComboBox monthCombo = sender as ComboBox;

            // Проверяем, найден ли ComboBox
            if (monthCombo != null)
            {
                // Преобразуем каждую строку, чтобы начинать с заглавной буквы
                for (int i = 0; i < months.Length; i++)
                {
                    months[i] = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(months[i]);
                }

                // Устанавливаем массив строк в качестве источника данных для ComboBox
                monthCombo.ItemsSource = months[..^1];
            }
        }

        private void FillYearsComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Определяем текущий год
            int currentYear = DateTime.Now.Year;

            int startYear = currentYear + 2;

            int[] years = new int[103];

            // Заполняем массив годами
            for (int i = 0; i < 103; i++)
            {
                years[i] = startYear - i;
            }

            // Находим ComboBox в визуальном дереве
            ComboBox yearsCombo = sender as ComboBox;

            // Проверяем, найден ли ComboBox
            if (yearsCombo != null)
            {
                // Устанавливаем массив лет в качестве источника данных для ComboBox
                yearsCombo.ItemsSource = years;
            }
        }

        private void Calendar_DisplayDateChanged(object sender, CalendarDateChangedEventArgs e)
        {
            System.Windows.Controls.Calendar calendar = sender as System.Windows.Controls.Calendar;

            if (calendar != null)
            {
                ComboBox monthCombo = FindComboBoxInVisualTree<ComboBox>(this, "MonthCombo");
                ComboBox yearsCombo = FindComboBoxInVisualTree<ComboBox>(this, "YearsCombo");

                if (monthCombo != null && yearsCombo != null)
                {
                    DateTime newDate = calendar.DisplayDate;

                    // Получаем название месяца с заглавной буквы
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(newDate.Month);
                    monthName = char.ToUpper(monthName[0]) + monthName.Substring(1).ToLower();

                    // Обновляем ComboBox с месяцами
                    monthCombo.SelectedItem = monthName;

                    // Обновляем ComboBox с годами
                    yearsCombo.SelectedItem = newDate.Year;
                }
            }
        }

        private T FindComboBoxInVisualTree<T>(DependencyObject parent, string name) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child is T element && element.Name == name)
                {
                    return element;
                }
                else
                {
                    T foundElement = FindComboBoxInVisualTree<T>(child, name);
                    if (foundElement != null)
                        return foundElement;
                }
            }
            return null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Получаем текущую дату
            DateTime currentDate = DateTime.Now;

            // Находим ComboBox'ы
            ComboBox monthCombo = FindComboBoxInVisualTree<ComboBox>(this, "MonthCombo");
            ComboBox yearsCombo = FindComboBoxInVisualTree<ComboBox>(this, "YearsCombo");

            if (monthCombo != null && yearsCombo != null)
            {
                // Получаем название месяца с заглавной буквы
                string currentMonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentDate.Month);
                currentMonthName = char.ToUpper(currentMonthName[0]) + currentMonthName.Substring(1).ToLower();

                // Устанавливаем значения в ComboBox'ы
                monthCombo.SelectedItem = currentMonthName;
                yearsCombo.SelectedItem = currentDate.Year;
            }
        }

        private void MonthCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox monthCombo = sender as ComboBox;

            if (monthCombo != null && monthCombo.SelectedItem != null)
            {
                // Преобразуем первую букву месяца к верхнему регистру
                string selectedMonth = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(monthCombo.SelectedItem.ToString());

                // Получаем номер месяца по его названию
                int monthNumber = DateTime.ParseExact(selectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;

                // Явно указываем тип System.Windows.Controls.Calendar
                System.Windows.Controls.Calendar wpfCalendar = FindName("MyCalendar") as System.Windows.Controls.Calendar;

                DateTime newDate;
                if (DateTime.TryParse($"{wpfCalendar.DisplayDate.Year}-{monthNumber}-1", out newDate))
                {
                    wpfCalendar.DisplayDate = newDate;
                }
            }
        }

        private void YearsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox yearsCombo = sender as ComboBox;

            if (yearsCombo != null && yearsCombo.SelectedItem != null)
            {
                // Явно указываем тип System.Windows.Controls.Calendar
                System.Windows.Controls.Calendar wpfCalendar = FindName("MyCalendar") as System.Windows.Controls.Calendar;

                DateTime newDate;
                if (DateTime.TryParse($"{(int)yearsCombo.SelectedItem}-{wpfCalendar.DisplayDate.Month}-1", out newDate))
                {
                    wpfCalendar.DisplayDate = newDate;
                }
            }
        }



    }
}
