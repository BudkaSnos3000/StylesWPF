using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StylesWPF
{
    public class VMCalendar : BaseInpc
    {
        private DateTime? _selectedDate;
        public DateTime? SelectedDate { get => _selectedDate; set => Set(ref _selectedDate, value); }
    }
}

