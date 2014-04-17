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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Source;
using FishRestaurant.Model.Entities;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Page
    {
        FRContext DB;

        public Customers()
        {
            InitializeComponent();
            DB = new FRContext();
            Fill_LB();
        }

        private void Fill_LB()
        {
            try
            {
                if (LB.IsEnabled)
                {
                    LB.ItemsSource = DB.Customers.Where(c => c.Name.StartsWith(NameSearchTB.Text) && c.Mobile.StartsWith(MobileSearchTB.Text)).OrderBy(o => o.Name).ToList();
                }
            }
            catch
            {
                
                
            }
        // End Fill
        }




        private void EditPanel_Edit(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    LB.SelectedIndex = -1;
                }
             
                LB.IsEnabled = false;
            }
            catch
            {

            }
        }

        private void EditPanel_Delete(object sender, EventArgs e)
        {

            try
            {
                if (Message.Show("هل تريد حذف هذا العميل", MessageBoxButton.YesNoCancel, 10) == MessageBoxResult.Yes)
                {
                    DB.Customers.Remove((Customer)LB.SelectedItem);
                    DB.SaveChanges();
                    Fill_LB();
                }
            }
            catch
            {

            }
        }



        private void Submit(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).Name.Split('_')[0] == "Save")
                {
                    if (LB.SelectedIndex == -1) 
                    { 
                        DB.Customers.Add(new Customer() { Name = NameTB.Text, Address = AddressTB.Text,Phone = TelephoneTB.Text,Mobile = MobileTB.Text }); 
                    }

                    DB.SaveChanges();
                    Confirm.Check(true);
                }


                    NameTB.Text = AddressTB.Text = TelephoneTB.Text = MobileTB.Text = "";  
                    LB.IsEnabled = true;
                    Fill_LB();
                

            }
            catch
            {
                Confirm.Check(false);
            }
        }

    }
}
