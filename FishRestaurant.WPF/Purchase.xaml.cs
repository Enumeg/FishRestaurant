using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FishRestaurant.Model.Entities;
using System.Data.Entity;
using Source;
using System.Windows.Data;
using System.Collections.ObjectModel;

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
        public Purchases(Transaction_Types type)
        {
            InitializeComponent();
            DB = new FRContext();
            Type = type;
            FillLB();
            InitializeLookups();
            UnitCB.ItemsSource = Enum.GetValues(typeof(Units));
        }
        private void InitializeLookups()
        {
            try
            {
                ComponentCB.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
                SupplierCB.ItemsSource = DB.Suppliers.OrderBy(c => c.Name).ToList();
                var suppliers = DB.Suppliers.OrderBy(c => c.Name).ToList();
                suppliers.Insert(0, new Supplier() { Id = 0, Name = "الكل" });
                SupplierSearch.ItemsSource = suppliers;
            }
            catch
            {

            }
        }
        private void FillLB()
        {

            try
            {

                var query = DB.Purchases.Where(p => p.Type == Type);
                if (SupplierSearch.SelectedIndex > 0) { query = query.Where(p => p.SupplierId == (int)SupplierSearch.SelectedValue); }
                if (DateSearch.Text != "")
                {
                    var date = DateSearch.Value.Value.Date;
                    query = query.Where(p => p.Date == date);
                }
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
                    ViewGrid.DataContext = new Purchase() { Date = DateTime.Now.Date, Type = Type };
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
                        decimal amount;
                        foreach (var p in ((Purchase)LB.SelectedItem).PurchaseDetails)
                        {
                            amount = Type == Transaction_Types.In ? p.Amount : p.Amount * -1;
                            DB.Components.Find(p.ComponentId).Stock -= amount;
                        }
                        DB.Purchases.Remove((Purchase)LB.SelectedItem);
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
                    if (Notify.validate("من فضلك أدخل المورد", SupplierCB.SelectedIndex, Main.GetWindow(this))) { return; }
                    if (LB.SelectedIndex == -1)
                    {
                        DB.Purchases.Add((Purchase)ViewGrid.DataContext);
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
                decimal amount;
                var PurchaseDetail = (PurchaseDetail)EditGrid.DataContext;
                if (AddBTN.Content.ToString() == "Add")
                {
                    amount = Type == Transaction_Types.In ? PurchaseDetail.Amount : PurchaseDetail.Amount * -1;                    
                    var oldPurchaseDetail = ((Purchase)ViewGrid.DataContext).PurchaseDetails.FirstOrDefault(p => p.Component == PurchaseDetail.Component && p.Unit == PurchaseDetail.Unit);
                    if (oldPurchaseDetail != null) { oldPurchaseDetail.Amount += PurchaseDetail.Amount; }
                    else
                        ((Purchase)ViewGrid.DataContext).PurchaseDetails.Add(PurchaseDetail);
                }
                else
                {
                    amount = Type == Transaction_Types.In ? PurchaseDetail.Amount -Amount   : Amount - PurchaseDetail.Amount;
                }
                AddBTN.Content = "Add";
                DB.Components.Find(PurchaseDetail.Component.Id).Stock += amount;
                Total_TB.Text = Paid_TB.Text = ((Purchase)ViewGrid.DataContext).PurchaseDetails.Sum(p => (p.Price * p.Amount)).ToString("0.00");
                Details_DG.ItemsSource = null;
                Details_DG.SetBinding(DataGrid.ItemsSourceProperty, "PurchaseDetails");
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
                    Amount = ((PurchaseDetail)Details_DG.SelectedItem).Amount;
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
                    var PurchaseDetail = (PurchaseDetail)EditGrid.DataContext;
                    var amount = Type == Transaction_Types.In ? PurchaseDetail.Amount : PurchaseDetail.Amount * -1;
                    DB.Components.Find(PurchaseDetail.Component.Id).Stock -= amount;
                    ((Purchase)ViewGrid.DataContext).PurchaseDetails.Remove(PurchaseDetail);
                    Details_DG.ItemsSource = null;
                    Details_DG.SetBinding(DataGrid.ItemsSourceProperty, "PurchaseDetails");
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



    }
}
