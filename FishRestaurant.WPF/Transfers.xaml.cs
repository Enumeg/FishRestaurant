using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FishRestaurant.Model.Entities;
using Source;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Transfers : Page
    {
        FRContext DB;
        Transaction_Types Type;
        decimal Amount;
        public Transfers(Transaction_Types type)
        {
            InitializeComponent();
            DB = new FRContext();
            Type = type;
            InitializeLookups();
            FillLB();
        }
        private void InitializeLookups()
        {
            try
            {
                ComponentCB.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
                UnitCB.ItemsSource = Enum.GetValues(typeof(Units));
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
                if (DateSearch.Text != "")
                {
                    var date = DateSearch.Value.Value.Date;
                    query = query.Where(p => p.Date == date);
                }
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
                    ViewGrid.DataContext = new Transfer() { Date = DateTime.Now.Date, Type = Type };
                    Form.Set_Style(InfoGrid, Operations.Add);
                    GetNumber();
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
                            amount = Type == Transaction_Types.In ? p.Amount : p.Amount * -1;
                            DB.Components.Find(p.ComponentId).Stock -= amount;
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
            FillLB();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal amount;
                var TransferDetail = (TransferDetail)EditGrid.DataContext;
                if (AddBTN.Content.ToString() == "Add")
                {
                    amount = Type == Transaction_Types.In ? TransferDetail.Amount : TransferDetail.Amount * -1;
                    var oldTransferDetail = ((Transfer)ViewGrid.DataContext).TransferDetails.FirstOrDefault(p => p.Component == TransferDetail.Component && p.Unit == TransferDetail.Unit);
                    if (oldTransferDetail != null) { oldTransferDetail.Amount += TransferDetail.Amount; }
                    else
                        ((Transfer)ViewGrid.DataContext).TransferDetails.Add(TransferDetail);
                }
                else
                {
                    amount = Type == Transaction_Types.In ? TransferDetail.Amount - Amount : Amount - TransferDetail.Amount;
                }
                AddBTN.Content = "Add";
                DB.Components.Find(TransferDetail.Component.Id).Stock += amount;
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
                    Amount = ((TransferDetail)Details_DG.SelectedItem).Amount;
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
        private void GetNumber()
        {
            try
            {
                if (!LB.IsEnabled)
                {
                    var date = DateDTP.Value.Value.Date;
                    var num = DB.Transfers.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month);
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
                    var TransferDetail = (TransferDetail)EditGrid.DataContext;
                    var amount = Type == Transaction_Types.In ? TransferDetail.Amount : TransferDetail.Amount * -1;
                    DB.Components.Find(TransferDetail.Component.Id).Stock -= amount;
                    ((Transfer)ViewGrid.DataContext).TransferDetails.Remove(TransferDetail);
                }
            }
            catch
            {

            }
        }


    }
}
