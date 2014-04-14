using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Maintenance.Styles
{
    partial class DateTimePicker : ResourceDictionary
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }
        private void TB_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = (System.Windows.Controls.DatePicker)((FrameworkElement)sender).TemplatedParent;
                f.SelectedDate = System.DateTime.Parse(f.Text, new System.Globalization.CultureInfo("ar-eg"));
            }
            catch
            {

            }

        }
        private void TB_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    var f = (System.Windows.Controls.DatePicker)((FrameworkElement)sender).TemplatedParent;
                    f.SelectedDate = System.DateTime.Parse(f.Text, new System.Globalization.CultureInfo("ar-eg"));
                }
            }
            catch
            {

            }

        }
        private void Part_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var f = (Xceed.Wpf.Toolkit.DateTimePicker)((FrameworkElement)sender).TemplatedParent;
                f.IsOpen = false;
            }
            catch
            {

            }
        }

    }
}
