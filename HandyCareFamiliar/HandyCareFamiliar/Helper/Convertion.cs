using System;
using System.Globalization;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Forms;

namespace HandyCareFamiliar.Helper
{
    public class Convertion : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as InlineToggledEventArgs;
            return eventArgs.selectedAppointment;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
