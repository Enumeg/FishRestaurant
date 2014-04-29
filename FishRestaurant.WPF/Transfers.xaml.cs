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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Transfers : Page
    {
        FrContext DB;
        TransactionTypes Type;
        decimal Amount;
        Units Unit;
        public Transfers(TransactionTypes type)
        {
            InitializeComponent();
            Type = type; Title = type == TransactionTypes.Out ? "سحب أصناف" : "تخزين أصناف";
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
                DB = new FrContext();
                ComponentCB.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
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
                var query = DB.Transfers.Where(p => p.Type == Type);
                if (NumberSearch.Text != "") { query = query.Where(p => p.Number == int.Parse(NumberSearch.Text)); }
                if (DateSearch.Text != "") { query = query.Where(p => DbFunctions.TruncateTime(p.Date) == DbFunctions.TruncateTime(DateSearch.Value.Value)); }
                LB.ItemsSource = query.Include(p => p.TransferDetails).OrderBy(p => p.Number).ToList();
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
                ViewGrid.RowDefinitions[2].Height = new GridLength(35);
                EditGrid.DataContext = new TransferDetail() { Amount = 1 };
                Details_DG.ColumnHeaderHeight = 62;
                Details_GD.RowDefinitions[1].Height = new GridLength(28);
                Details_DG.IsReadOnly = false;
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    LB.SelectedIndex = -1;
                    Form.Set_Style(InfoGrid, Operations.Add);
                    ViewGrid.DataContext = new Transfer() { Date = DateDTP.Value.Value, Type = Type };
                    Number.Text = TransactionsService.GetNumber(DateDTP.Value.Value, Type);
                }
                else
                {
                    Form.Set_Style(InfoGrid, Operations.Edit);
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
                        foreach (var p in ((Transfer)LB.SelectedItem).TransferDetails)
                        {
                            amount = Type == TransactionTypes.In ? p.Amount : p.Amount * -1;
                            if (p.Unit == Units.جرام) { amount *= 0.001m; }
                            //DB.Components.Find(p.ComponentId).Stock -= amount;
                        }
                        DB.Transfers.Remove((Transfer)LB.SelectedItem);
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
                    if (LB.SelectedIndex == -1)
                    {
                        DB.Transfers.Add((Transfer)ViewGrid.DataContext);
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
                ViewGrid.RowDefinitions[2].Height = new GridLength(0);
                Form.Set_Style(InfoGrid, Operations.View);
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
                var TransferDetail = (TransferDetail)EditGrid.DataContext;
                var TransferDetails = ((Transfer)ViewGrid.DataContext).TransferDetails;
                if (AddBTN.Content.ToString() == "Add")
                {
                    amount = Type == TransactionTypes.In ? TransferDetail.Amount : TransferDetail.Amount * -1;
                    if (TransferDetail.Unit == Units.جرام) { amount *= 0.001m; }
                    var oldTransferDetail = TransferDetails.FirstOrDefault(p => p.Component.Id == TransferDetail.Component.Id && p.Unit == TransferDetail.Unit);
                    if (oldTransferDetail != null)
                    {
                        oldTransferDetail.Amount += TransferDetail.Amount;
                        oldTransferDetail.OnPropertyChanged("Amount");
                    }
                    else
                        ((Transfer)ViewGrid.DataContext).TransferDetails.Add(TransferDetail);
                }
                else
                {
                    Amount = Unit == Units.جرام ? Amount *= 0.001m : Amount;
                    TransferDetail.OnPropertyChanged("Amount");
                    if (TransferDetail.Unit == Units.جرام)
                    {
                        amount = Type == TransactionTypes.In ? TransferDetail.Amount * 0.001m - Amount : Amount - TransferDetail.Amount * 0.001m;
                    }
                    else
                    {
                        amount = Type == TransactionTypes.In ? TransferDetail.Amount - Amount : Amount - TransferDetail.Amount;
                    }
                }
                AddBTN.Content = "Add";
                //DB.Components.Find(TransferDetail.Component.Id).Stock += amount;
                EditGrid.DataContext = new TransferDetail() { Amount = 1 };
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
                    var TransferDetail = ((TransferDetail)Details_DG.SelectedItem);
                    Amount = TransferDetail.Amount;
                    Unit = TransferDetail.Unit;
                }
                else
                {
                    EditGrid.DataContext = new TransferDetail() { Amount = 1 };
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
                    var TransferDetail = (TransferDetail)EditGrid.DataContext;
                    var amount = Type == TransactionTypes.In ? TransferDetail.Amount : TransferDetail.Amount * -1;
                    if (TransferDetail.Unit == Units.جرام) { amount *= 0.001m; }
                    //DB.Components.Find(TransferDetail.Component.Id).Stock -= amount;
                    DB.TransferDetails.Remove(TransferDetail);
                }
            }
            catch
            {

            }
        }

        private void DateDTP_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                if (LB.IsEnabled == false)
                    Number.Text = TransactionsService.GetNumber(DateDTP.Value.Value, Type);
            }
            catch
            {

            }
        }
    }
}
