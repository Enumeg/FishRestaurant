using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FishRestaurant.Model.Entities;
using System.Data.Entity;
using Source;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using FishRestaurant.WPF.Services;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Purchases : Page
    {
        FRContext DB;
        Transaction_Types Type;
        decimal Amount;
        Units Unit;
        public Purchases(Transaction_Types type)
        {
            InitializeComponent();
            Type = type;
            Title = type == Transaction_Types.Buy ? "المشتريات" : "مرتجعات المشتريات";

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Initialize();
            }
            catch
            {

            }
        }
        private void Initialize()
        {
            try
            {
                DB = new FRContext();
                ComponentCB.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
                PersonCB.ItemsSource = DB.People.Where(p => p.Type == PersonTypes.Supplier).OrderBy(c => c.Name).ToList();
                var suppliers = DB.People.Where(p => p.Type == PersonTypes.Supplier).OrderBy(c => c.Name).ToList();
                suppliers.Insert(0, new Person() { Id = 0, Name = "الكل" });
                PersonSearch.ItemsSource = suppliers;
                UnitCB.ItemsSource = Enum.GetValues(typeof(Units));
                FillLB();
            }
            catch
            {

            }
        }
        private void FillLB()
        {

            try
            {
                var query = DB.Transactions.Where(p => p.Type == Type);
                if (NumberSearch.Text != "") { query = query.Where(p => p.Number == int.Parse(NumberSearch.Text)); }
                if (PersonSearch.SelectedIndex > 0) { query = query.Where(p => p.PersonId == (int)PersonSearch.SelectedValue); }
                if (DateSearch.Text != "") { query = query.Where(p => DbFunctions.TruncateTime(p.Date) == DbFunctions.TruncateTime(DateSearch.Value.Value)); }
                LB.ItemsSource = query.Include(p => p.PurchaseDetails).OrderBy(p => p.Number).ToList();
            }
            catch
            {

            }

        }
        private void EditPanel_Edit(object sender, EventArgs e)
        {
            try
            {
                MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
                LB.IsEnabled = false;
                ViewGrid.RowDefinitions[3].Height = new GridLength(35);
                EditGrid.DataContext = new PurchaseDetail() { Amount = 1 };
                Details_DG.ColumnHeaderHeight = 62;
                Details_GD.RowDefinitions[1].Height = new GridLength(28);
                Details_DG.IsReadOnly = false;
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    LB.SelectedIndex = -1;
                    Form.Set_Style(InfoGrid, Operations.Add);
                    Form.Set_Style(TotalsGrid, Operations.Add);
                    ViewGrid.DataContext = new Transaction() { Date = DateDTP.Value.Value.Date, Type = Type };
                    Number.Text = TransactionsService.GetNumber(DateDTP.Value.Value, Type);
                }
                else
                {
                    Form.Set_Style(InfoGrid, Operations.Edit);
                    Form.Set_Style(TotalsGrid, Operations.Edit);
                }

            }
            catch
            {

            }
        }
        private void EditPanel_Delete(object sender, EventArgs e)
        {
            try
            {
                if (LB.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذه الفاتورة", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        //decimal amount;
                        //foreach (var p in ((Transaction)LB.SelectedItem).PurchaseDetails)
                        //{
                        //    amount = Type == Transaction_Types.Buy ? p.Amount : p.Amount * -1;
                        //    if (p.Unit == Units.جرام) { amount *= 0.001m; }
                        //    DB.Components.Find(p.ComponentId).Stock -= amount;
                        //}
                        DB.Transactions.Remove((Transaction)LB.SelectedItem);
                        DB.SaveChanges();
                        FillLB();
                    }
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
                    if (Notify.validate("من فضلك أدخل المورد", PersonCB.SelectedIndex, Main.GetWindow(this))) { return; }
                    if (LB.SelectedIndex == -1)
                    {
                        DB.Transactions.Add((Transaction)ViewGrid.DataContext);
                    }
                    DB.SaveChanges();
                    Confirm.Check(true);
                }
                else
                {
                    DB.Dispose();
                    Initialize();
                }
                Details_DG.IsReadOnly = true;
                LB.IsEnabled = true;
                FillLB();
                MainGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                ViewGrid.RowDefinitions[3].Height = new GridLength(0);
                Form.Set_Style(InfoGrid, Operations.View);
                Form.Set_Style(TotalsGrid, Operations.View);
                Details_DG.ColumnHeaderHeight = 32;
                Details_GD.RowDefinitions[1].Height = new GridLength(0);
                LB.SelectedIndex = -1;

            }
            catch
            {
                Confirm.Check(false);
            }
        }
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ViewGrid.DataContext = LB.SelectedItem;
            }
            catch
            {

            }
        }
        private void FillLB(object sender, EventArgs e)
        {
            if (this.IsLoaded)
                FillLB();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal amount;
                var PurchaseDetails = ((Transaction)ViewGrid.DataContext).PurchaseDetails;
                var PurchaseDetail = (PurchaseDetail)EditGrid.DataContext;
                if (AddBTN.Content.ToString() == "Add")
                {
                    amount = Type == Transaction_Types.Buy ? PurchaseDetail.Amount : PurchaseDetail.Amount * -1;
                    if (PurchaseDetail.Unit == Units.جرام) { amount *= 0.001m; }
                    var oldPurchaseDetail = PurchaseDetails.FirstOrDefault(p => p.Component.Id == PurchaseDetail.Component.Id && p.Unit == PurchaseDetail.Unit && p.Price == PurchaseDetail.Price);
                    if (oldPurchaseDetail != null)
                    {
                        oldPurchaseDetail.Amount += PurchaseDetail.Amount;
                        oldPurchaseDetail.OnPropertyChanged("Amount");
                    }
                    else
                        PurchaseDetails.Add(PurchaseDetail);
                }
                else
                {
                    Amount = Unit == Units.جرام ? Amount *= 0.001m : Amount;
                    if (PurchaseDetail.Unit == Units.جرام)
                    {
                        amount = Type == Transaction_Types.Buy ? PurchaseDetail.Amount * 0.001m - Amount : Amount - PurchaseDetail.Amount * 0.001m;
                    }
                    else
                    {
                        amount = Type == Transaction_Types.Buy ? PurchaseDetail.Amount - Amount : Amount - PurchaseDetail.Amount;
                    }
                    PurchaseDetail.OnPropertyChanged("Amount");
                }
                AddBTN.Content = "Add";
                PurchaseDetail.Total = Math.Round(PurchaseDetail.Amount * PurchaseDetail.Price, 2);
                PurchaseDetail.OnPropertyChanged("Total");
                //DB.Components.Find(PurchaseDetail.Component.Id).Stock += amount;
                Paid_TB.Text = Total_TB.Text = PurchaseDetails.Sum(p => (p.Price * p.Amount)).ToString("0.00");
                EditGrid.DataContext = new PurchaseDetail() { Amount = 1 };
            }
            catch
            {

            }
        }
        private void Details_DG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Details_DG.SelectedIndex != -1)
                {
                    EditGrid.DataContext = Details_DG.SelectedItem;
                    AddBTN.Content = "Edit";
                    var PurchaseDetail = ((PurchaseDetail)Details_DG.SelectedItem);
                    Amount = PurchaseDetail.Amount;
                    Unit = PurchaseDetail.Unit;
                }
                else
                {
                    EditGrid.DataContext = new PurchaseDetail() { Amount = 1 };
                    AddBTN.Content = "Add";
                }
            }
            catch
            {

            }
        }
        private void Details_DG_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (!Details_DG.IsReadOnly && e.Key== System.Windows.Input.Key.Delete)
                {
                    var PurchaseDetail = (PurchaseDetail)EditGrid.DataContext;
                    var PurchaseDetails = ((Transaction)ViewGrid.DataContext).PurchaseDetails;
                    //var amount = Type == Transaction_Types.Buy ? PurchaseDetail.Amount : PurchaseDetail.Amount * -1;
                    //if (PurchaseDetail.Unit == Units.جرام) { amount *= 0.001m; }
                    ////DB.Components.Find(PurchaseDetail.Component.Id).Stock -= amount;
                    //PurchaseDetails.Remove(PurchaseDetail);
                    DB.PurchaseDetails.Remove(PurchaseDetail);
                    Paid_TB.Text = Total_TB.Text = PurchaseDetails.Sum(p => (p.Price * p.Amount)).ToString("0.00");
                }
            }
            catch
            {

            }
        }
        private void Paid_TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var Transaction = ((Transaction)ViewGrid.DataContext);
                if (Transaction != null)
                    Rest_TB.Text = (Transaction.Total - Transaction.Paid).ToString("0.00");
            }
            catch
            {

            }
        }
    }
}
