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
using System.Data.Entity;
using Source;

using System.Collections.ObjectModel;

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
            DB = new FRContext();
            Type = type;
            FillLB();
            InitializeLookups();
           
        }
        private void InitializeLookups()
        {
            try
            {
               
                //ProductCB.ItemsSource = DB.Products.OrderBy(c =>c.Name).ToList(); 
                ProductCB.ItemsSource = DB.Products.OrderBy(p => p.Name).ToList();


                CustomerCB.ItemsSource = DB.Customers.OrderBy(c => c.Name).ToList();

                var customers = DB.Customers.OrderBy(c => c.Name).ToList();
                customers.Insert(0, new Customer() { Id = 0, Name = "الكل" });
                CustomerSearch.ItemsSource = customers;
            }
            catch
            {

            }
        }
        private void FillLB()
        {

            try
            {

                var query = DB.Sales.Where(p => p.Type == Type);

                if (CustomerSearch.SelectedIndex > 0) { query = query.Where(p => p.CustomerId == (int)CustomerSearch.SelectedValue); }
                if (DateSearch.Text != "")
                {
                    var date = DateSearch.Value.Value.Date;
                    query = query.Where(p => p.Date == date);
                }
                
                LB.ItemsSource = query.Include(p => p.SaleDetails).OrderBy(p => p.Number).ToList();
                
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
                    ViewGrid.DataContext = new Sale() { Date = DateTime.Now.Date, Type = Type };
                    Form.Set_Style(InfoGrid, Operations.Add);
                    Form.Set_Style(TotalsGrid, Operations.Add);
                    GetNumber();
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
                        //foreach (var p in ((Sale)LB.SelectedItem).SaleDetails)
                        //{
                          //  amount = Type == Transaction_Types.In ? p.Amount : p.Amount * -1;
                            //DB.Components.Find(p.ProductId).Stock -= amount;
                        //}
                        DB.Sales.Remove((Sale)LB.SelectedItem);
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
                    if (Notify.validate("من فضلك أدخل العميل", CustomerCB.SelectedIndex, Main.GetWindow(this))) { return; }
                    if (LB.SelectedIndex == -1)
                    {
                        DB.Sales.Add((Sale)ViewGrid.DataContext);
                    }
                    DB.SaveChanges();
                    Confirm.Check(true);
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
            FillLB();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                var SalesDetail = (SaleDetail)EditGrid.DataContext;

                if (AddBTN.Content.ToString() == "Add")
                {
                    var oldPurchaseDetail = ((Sale)ViewGrid.DataContext).SaleDetails.FirstOrDefault(p => p.Product == SalesDetail.Product);
                    if (oldPurchaseDetail != null) { oldPurchaseDetail.Amount += SalesDetail.Amount; }
                    else
                        ((Sale)ViewGrid.DataContext).SaleDetails.Add(SalesDetail);
                }
                AddBTN.Content = "Add"; 

                             
                Total_TB.Text = Paid_TB.Text = ((Sale)ViewGrid.DataContext).SaleDetails.Sum(p => (p.Price * p.Amount)).ToString("0.00");
                Details_DG.ItemsSource = null;
                Details_DG.SetBinding(DataGrid.ItemsSourceProperty, "SaleDetails");
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
        private void GetNumber()
        {
            try
            {
                if (!LB.IsEnabled)
                {
                    var date = DateDTP.Value.Value.Date;
                    var num = DB.Purchases.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month);
                    Number.Text = num.Count() != 0 ? (num.Max(p => p.Number) + 1).ToString() : string.Format("{0}{1}001", date.Year, date.Month);
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
                if (!Details_DG.IsReadOnly)
                {
                    var SalesDetails = (SaleDetail)EditGrid.DataContext;
                    var amount = Type == Transaction_Types.Out ? SalesDetails.Amount : SalesDetails.Amount * -1;
                    //DB.Components.Find(SalesDetails.Product.Id).Stock -= amount;
                    ((Sale)ViewGrid.DataContext).SaleDetails.Remove(SalesDetails);
                    Details_DG.ItemsSource = null;
                    Details_DG.SetBinding(DataGrid.ItemsSourceProperty, "SaleDetails");
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
                var Purchase = ((Purchase)ViewGrid.DataContext);
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



    }
}
