using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FishRestaurant.WPF.Styles
{
    partial class Popup : ResourceDictionary
    {
       public Popup()
        {
            InitializeComponent();
        }
                #region Header
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var f = (System.Windows.Controls.Button)((FrameworkElement)sender).TemplatedParent;
                var popup = f.Parent as System.Windows.Controls.Primitives.Popup;
                popup.IsOpen = false;
            }
            catch
            {

            }

        }
        #endregion

    }
}
