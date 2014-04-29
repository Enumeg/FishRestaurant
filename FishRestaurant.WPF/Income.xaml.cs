using FishRestaurant.Model.Entities;
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

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Income.xaml
    /// </summary>
    public partial class Income : Page
    {
        FRContext DB;

        public Income()
        {
            InitializeComponent();
            DB = new FRContext();

        }


        private void FillDG()
        {

            try
            {
                IncomeDG.ItemsSource = DB.Transactions.Where(t => t.Date >= From_DTP.Value.Value && t.Date <= To_DTP.Value.Value &&
                    (t.Type == Transaction_Types.ReBuy || t.Type == Transaction_Types.Sell)).Select(t => new
                    {
                        Number = t.Number,
                        Date = t.Date,
                        Value = t.Paid,
                        Type = t.Type == Transaction_Types.Sell ? "" : "",
                        Person = t.Person.Name
                    }).Union(DB.Installments.Where(i => i.Date >= From_DTP.Value.Value && i.Date <= To_DTP.Value.Value && i.Person.Type == PersonTypes.Customer).Select(i => new
                    {
                        Number = 0,
                        Date = i.Date,
                        Value = i.Value,
                        Type = "",
                        Person = i.Person.Name
                    }));


            }
            catch
            {


            }
        }

        private void From_DTP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }


    }
}
