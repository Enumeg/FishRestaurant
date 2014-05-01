using FishRestaurant.Model.Entities;
using FishRestaurant.Model.ViewModels;
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
                var inc_list = new List<IncomeDG>();

                decimal total = 0;

                var ins = DB.Installments.Where(i => DbFunctions.TruncateTime(i.Date) >= DbFunctions.TruncateTime(From_DTP.Value.Value)
                     && DbFunctions.TruncateTime(i.Date) <= DbFunctions.TruncateTime(To_DTP.Value.Value) && i.Person.Type == PersonTypes.Customer).ToList();


                total += ins.Sum(c=> c.Value);

                foreach (var i in ins)
                {
                   
                    inc_list.Add(new IncomeDG()
                    {

                        Date = i.Date,
                        Number = i.Number,
                        Type = "قسط عميل",
                        Value = i.Value,
                        Person = i.Person.Name

                         

                    });
                }




                var inc = DB.Transactions.Where(t => DbFunctions.TruncateTime(t.Date) >= DbFunctions.TruncateTime(From_DTP.Value.Value)
                                               && DbFunctions.TruncateTime(t.Date) <= DbFunctions.TruncateTime(To_DTP.Value.Value) &&
                                               (t.Type != TransactionTypes.Buy && t.Type != TransactionTypes.SellBack)).ToList();

                total += inc.Sum(c => c.Paid);

                foreach (var i in inc)
                {
                  inc_list.Add(new IncomeDG()
                  {
                        Date   = i.Date,
                        Number = i.Number,
                        Type   = i.Type == TransactionTypes.Order ? "Order" : i.Type == TransactionTypes.InHouse ? "In House" : i.Type == TransactionTypes.TakeAway ? "Take Away" : "مرتجع شراء",
                        Value  = i.Paid,
                        Person = i.Person.Name



                   });
                }


                inc_list.Add(new IncomeDG()
                {
                    
                    Value = total

                });


                IncomeDG.ItemsSource = inc_list;

             
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
