using System;
using System.Data;
using System.Windows;
using FishRestaurant.Model.Entities;
using System.Linq;
using Source;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class AddCustomer : Window
    {
        FrContext DB;
       public static Person Customer;
        public AddCustomer(FrContext Db, string phone)
        {
            InitializeComponent();
            this.DataContext = new Person() { Type= PersonTypes.Customer, Balance=0, Phone = phone };
            this.DB = Db;
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            try
            {
                var Cust = this.DataContext as Person;
                DB.People.Add(Cust);
                DB.SaveChanges();
                Customer = Cust;
                Confirm.Check(true);
            }
            catch
            {
                Confirm.Check(false);
            }
        }

    }
}
