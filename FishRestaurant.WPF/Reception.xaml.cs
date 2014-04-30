using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Reception.xaml
    /// </summary>
    public partial class Reception : Window
    {

        People CustomersPage;
        Sales SalesPage;
 


        DoubleAnimation ani;
        string Selected_Button = "";

        public Reception()
        {
            InitializeComponent();
            ani = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 700));
            CustomersPage = new People(Model.Entities.PersonTypes.Customer);

            SalesPage = new Sales();
        }

        private void Customer_BTN_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Frame.Navigate(CustomersPage);

                Set_Selected(Customer_BTN);
                
            }
            catch 
            {
                
                
            }
        }

      
        private void Sales_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Frame.Navigate(SalesPage);

                Set_Selected(Sales_BTN);
            }
            catch
            {


            }
        }


        private void Set_Selected(Button button)
        {
            try
            {

                if (Selected_Button != "")
                {
                    ((Button)FindName(Selected_Button)).Style = FindResource("Side") as Style;
                }
                button.Style = FindResource("Selected_Side") as Style;
                Selected_Button = button.Name;

            }
            catch
            {

            }
        }


        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                Page page = Frame.Content as Page;
                page.BeginAnimation(Page.OpacityProperty, ani);

            }
            catch
            {

            }
        }

    
    }
}
