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
using FishRestaurant.Model.ViewModels;
using System.Data.Entity;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for People.xaml
    /// </summary>
    public partial class People : Page
    {
        FrContext DB;
        PersonTypes Type;
        public People(PersonTypes type)
        {
            InitializeComponent();

            Type = type;
            Title = type == PersonTypes.Customer ? "العملاء" : "الموردين";
            if (App.User != null && App.User.Group == Groups.Cashier)
            {
                InfoGrid.RowDefinitions[1].Height = InfoGrid.RowDefinitions[2].Height = new GridLength(0);
            }
        }
        private void FillLB()
        {
            try
            {
                if (LB.IsEnabled)
                {
                    var query = DB.People.Where(c => c.Type == Type && c.Name.StartsWith(NameSearchTB.Text));
                    if (MobileSearchTB.Text != "") { query = query.Where(c => c.Mobile.StartsWith(MobileSearchTB.Text)); }
                    LB.ItemsSource = query.OrderBy(o => o.Name).ToList();
                }
            }
            catch
            {


            }
        }
        private void EditPanel_Edit(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    LB.SelectedIndex = -1;
                    Form.Set_Style(MainGrid, Operations.Add);
                    MainGrid.DataContext = new Person() { Balance = 0, Type = Type };
                }
                else
                {
                    Form.Set_Style(MainGrid, Operations.Edit);
                }
                Save.Visibility = System.Windows.Visibility.Visible;
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
                    DB.People.Remove((Person)LB.SelectedItem);
                    DB.SaveChanges();
                    FillLB();
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
                        if (DB.People.FirstOrDefault(p => p.Address == AddressTB.Text && p.Phone == TelephoneTB.Text && p.Type == Type) == null)
                            DB.People.Add(MainGrid.DataContext as Person);
                        else
                        {
                            Message.Show("يوجد عميل بنفس العنوان ورقم التليفون", MessageBoxButton.OKCancel);
                            return;
                        }
                    }
                    DB.SaveChanges();
                    Confirm.Check(true);
                }
                Form.Set_Style(MainGrid, Operations.View);
                Save.Visibility = System.Windows.Visibility.Hidden;
                LB.IsEnabled = true;
                FillLB();
            }
            catch
            {
                Confirm.Check(false);
            }
        }
        private void FillLB(object sender, EventArgs e)
        {
            FillLB();
        }
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MainGrid.DataContext = LB.SelectedItem;
                GetAccounts();
            }
            catch
            {

            }
        }
        private void GetAccounts(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            GetAccounts();
        }

        private void GetAccounts()
        {
            try
            {
                var accounts = new List<Accounts>();
                var balance = DB.People.Find((int)LB.SelectedValue).Balance;
                var from = From_DTP.Value.Value.Date;
                var to = To_DTP.Value.Value.Date;
                if (Type == PersonTypes.Customer)
                {
                    var sales = DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type != TransactionTypes.SellBack && DbFunctions.TruncateTime(t.Date) < from);

                    var resales = DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type == TransactionTypes.SellBack && DbFunctions.TruncateTime(t.Date) < from);
                    var installments = DB.Installments.Where(i => i.PersonId == (int)LB.SelectedValue && DbFunctions.TruncateTime(i.Date) < from);

                    balance += (installments.Count() > 0 ? installments.Sum(i => i.Value) : 0)
                        - (sales.Count() > 0 ? sales.Sum(t => t.Total) + sales.Sum(t => t.Paid) : 0) + (resales.Count() > 0 ? resales.Sum(t => t.Total) - resales.Sum(t => t.Paid) : 0);

                    foreach (var ins in DB.Installments.Where(i => i.PersonId == (int)LB.SelectedValue && DbFunctions.TruncateTime(i.Date) >= from && DbFunctions.TruncateTime(i.Date) <= to))
                    {
                        accounts.Add(new Accounts() { Number = ins.Number, Creditor = ins.Value, Description = ins.Description, Date = ins.Date });
                    }
                    foreach (var trs in DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type != TransactionTypes.SellBack && DbFunctions.TruncateTime(t.Date) >= from && DbFunctions.TruncateTime(t.Date) <= to))
                    {
                        accounts.Add(new Accounts() { Number = trs.Number, Creditor = trs.Paid, Debtor = trs.Total, Description = "بيع", Date = trs.Date });
                    }
                    foreach (var trs in DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type == TransactionTypes.SellBack && DbFunctions.TruncateTime(t.Date) >= from && DbFunctions.TruncateTime(t.Date) <= to))
                    {
                        accounts.Add(new Accounts() { Number = trs.Number, Creditor = trs.Total, Debtor = trs.Paid, Description = "مرتجع", Date = trs.Date });
                    }
                }
                else
                {
                    var purchases = DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type == TransactionTypes.Buy && DbFunctions.TruncateTime(t.Date) < from);

                    var repurchases = DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type == TransactionTypes.ReBuy && DbFunctions.TruncateTime(t.Date) < from);
                    var installments = DB.Installments.Where(i => i.PersonId == (int)LB.SelectedValue && DbFunctions.TruncateTime(i.Date) < from);

                    balance += (purchases.Count() > 0 ? purchases.Sum(t => t.Total) - purchases.Sum(t => t.Paid) : 0) - (repurchases.Count() > 0 ? repurchases.Sum(t => t.Total) + repurchases.Sum(t => t.Paid) : 0)
                    - (installments.Count() > 0 ? installments.Sum(i => i.Value) : 0);
                    foreach (var ins in DB.Installments.Where(i => i.PersonId == (int)LB.SelectedValue && DbFunctions.TruncateTime(i.Date) >= from && DbFunctions.TruncateTime(i.Date) <= to))
                    {
                        accounts.Add(new Accounts() { Number = ins.Number, Debtor = ins.Value, Description = ins.Description, Date = ins.Date });
                    }
                    foreach (var trs in DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type == TransactionTypes.ReBuy && DbFunctions.TruncateTime(t.Date) >= from && DbFunctions.TruncateTime(t.Date) <= to))
                    {
                        accounts.Add(new Accounts() { Number = trs.Number, Creditor = trs.Paid, Debtor = trs.Total, Description = "مرتجع", Date = trs.Date });
                    }
                    foreach (var trs in DB.Transactions.Where(t => t.PersonId == (int)LB.SelectedValue && t.Type == TransactionTypes.Buy && DbFunctions.TruncateTime(t.Date) >= from && DbFunctions.TruncateTime(t.Date) <= to))
                    {
                        accounts.Add(new Accounts() { Number = trs.Number, Creditor = trs.Total, Debtor = trs.Paid, Description = "شراء", Date = trs.Date });
                    }
                }
                accounts = accounts.OrderBy(a => a.Date).ToList();
                accounts.Insert(0, new Accounts() { Balance = balance, Date = From_DTP.Value.Value, Debtor = balance > 0 ? 0 : balance, Creditor = balance > 0 ? balance : 0 });
                foreach (var acc in accounts)
                {
                    if (accounts.IndexOf(acc) == 0)
                    {
                        continue;
                    }
                    acc.Balance = accounts[accounts.IndexOf(acc) - 1].Balance + acc.Creditor - acc.Debtor;
                }
                Account_DG.ItemsSource = accounts;
            }
            catch
            {

            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DB = new FrContext();
                FillLB();
                GetAccounts();
            }
            catch
            {


            }
        }
    }
}
