using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FishRestaurant.Model.Entities;
using FishRestaurant.WPF.Services;
using Source;


namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for sales.xaml
    /// </summary>
    public partial class Sales : Page
    {

        FRContext DB;
        Transaction_Types Type;
        decimal Amount;
        public Sales(Transaction_Types type)
        {
            InitializeComponent();
            Type = type;
            Title = type == Transaction_Types.SellIn ? "المبيعات" : "مرتجعات المبيعات";
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
                ProductCB.ItemsSource = DB.Products.OrderBy(p => p.Name).ToList();
                PersonCB.ItemsSource = DB.People.Where(p => p.Type == PersonTypes.Customer).OrderBy(p => p.Name).ToList();
                var customers = DB.People.Where(p => p.Type == PersonTypes.Customer).OrderBy(p => p.Name).ToList();
                customers.Insert(0, new Person() { Id = 0, Name = "الكل" });
                PersonSearch.ItemsSource = customers;
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
                EditGrid.DataContext = new SaleDetail() { Amount = 1 };
                Details_DG.ColumnHeaderHeight = 62;
                Details_GD.RowDefinitions[1].Height = new GridLength(28);
                Details_DG.IsReadOnly = false;
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    LB.SelectedIndex = -1;
                    Form.Set_Style(InfoGrid, Operations.Add);
                    Form.Set_Style(TotalsGrid, Operations.Add);
                    ViewGrid.DataContext = new Transaction() { Date = DateDTP.Value.Value, Type = Type };
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
                    if (Notify.validate("من فضلك أدخل العميل", PersonCB.SelectedIndex, Main.GetWindow(this))) { return; }
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
                var SaleDetail = (SaleDetail)EditGrid.DataContext;
                var SaleDetails = ((Transaction)ViewGrid.DataContext).SaleDetails;
                if (AddBTN.Content.ToString() == "Add")
                {
                    var oldPurchaseDetail = SaleDetails.FirstOrDefault(p => p.Product == SaleDetail.Product);
                    if (oldPurchaseDetail != null)
                    {
                        oldPurchaseDetail.Amount += SaleDetail.Amount;
                        oldPurchaseDetail.OnPropertyChanged("Amount");
                    }
                    else
                    {
                        SaleDetails.Add(SaleDetail);
                    }
                }
                else
                {
                    SaleDetail.OnPropertyChanged("Amount");
                }
                AddBTN.Content = "Add";
                SaleDetail.Total = Math.Round(SaleDetail.Amount * SaleDetail.Price, 2);
                SaleDetail.OnPropertyChanged("Total");
                Paid_TB.Text = Total_TB.Text = ((Transaction)ViewGrid.DataContext).SaleDetails.Sum(p => (p.Price * p.Amount)).ToString("0.00");
                EditGrid.DataContext = new SaleDetail() { Amount = 1 };
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
                    Amount = ((SaleDetail)Details_DG.SelectedItem).Amount;
                }
                else
                {
                    EditGrid.DataContext = new SaleDetail() { Amount = 1 };
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
                if (!Details_DG.IsReadOnly && e.Key == System.Windows.Input.Key.Delete)
                {                  
                    DB.SaleDetails.Remove((SaleDetail)EditGrid.DataContext);
                    Paid_TB.Text = Total_TB.Text = ((Transaction)ViewGrid.DataContext).SaleDetails.Sum(p => (p.Price * p.Amount)).ToString("0.00");
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
                var Purchase = ((Transaction)ViewGrid.DataContext);
                Rest_TB.Text = (Purchase.Total - Purchase.Paid).ToString("0.00");
            }
            catch
            {

            }
        }
        private void ProductCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                /// 3awzeeeno lma ye5tar yenzel el Price Automatic
                Price.Text = ((Product)ProductCB.SelectedItem).Price.ToString();// ya salam bs keda :D
            }
            catch
            {


            }
        }

        private void Search(object sender, EventArgs e)
        { }

    }
}
