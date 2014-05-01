using FishRestaurant.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        FrContext DB;

        public Income()
        {
            InitializeComponent();
            DB = new FrContext();
            FillDG();
        }


        private void FillDG()
        {

            try
            {       
         
                var inc = DB.Transactions.Where(t => DbFunctions.TruncateTime(t.Date) >= DbFunctions.TruncateTime(From_DTP.Value.Value)
                                                && DbFunctions.TruncateTime(t.Date) <= DbFunctions.TruncateTime(To_DTP.Value.Value) &&
                                                (t.Type != TransactionTypes.Buy && t.Type != TransactionTypes.SellBack));


                var ins = DB.Installments.Where(i => DbFunctions.TruncateTime(i.Date) >= DbFunctions.TruncateTime(From_DTP.Value.Value)
                        && DbFunctions.TruncateTime(i.Date) <= DbFunctions.TruncateTime(To_DTP.Value.Value) && i.Person.Type == PersonTypes.Customer);

              

                IncomeDG.ItemsSource = inc.Select(t => new
                    {
                        Number = t.Number,
                        Date = t.Date,
                        Value = t.Paid,
                        Type = t.Type == TransactionTypes.Order ? "Order" : t.Type== TransactionTypes.InHouse ? "In House" : t.Type== TransactionTypes.TakeAway ? "Take Away" : "مرتجع شراء" ,

                        Person = t.Person.Name
                    }).Union(ins.Select(i => new
                    {
                        Number = 0,
                        Date = i.Date,
                        Value = i.Value,
                        Type = "قسط عميل",
                        Person = i.Person.Name
                    })).ToList();


                  var tot = inc.Sum(i => i.Paid) + ins.Sum(inss=> inss.Value);

                 

            }
            catch
            {


            }
        }

        private void From_DTP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            FillDG();
        }


    }
}
