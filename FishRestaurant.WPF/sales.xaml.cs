using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Printing;
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

        FrContext DB;
        decimal Amount;
        PrintDocument PD1;

        public Sales()
        {
            InitializeComponent();
            PD1 = new PrintDocument();
            PD1.PrintPage += new PrintPageEventHandler(PD1_PrintPage);
            if (App.User != null && App.User.Group == Groups.Cashier)
            {
                MainGrid.ColumnDefinitions[0].Width = new GridLength(0);
                ViewGrid.RowDefinitions[4].Height = new GridLength(35);
                EditGrid.DataContext = new SaleDetail() { Amount = 1 };
                Details_DG.ColumnHeaderHeight = 62;
                Details_GD.RowDefinitions[1].Height = new GridLength(28);
                Details_DG.IsReadOnly = false;
                LB.IsEnabled = false;
                Form.Set_Style(InfoGrid, Operations.Add);
                Form.Set_Style(TotalsGrid, Operations.Add);
                ViewGrid.DataContext = new Transaction() { Date = DateTime.Now.Date, Type = TransactionTypes.Order, Delivery = 0 };
                Number.Text = TransactionsService.GetNumber(DateTime.Now, TransactionTypes.Order);
                Pop.IsOpen = true;
            }
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
                ProductCB.ItemsSource = DB.Products.OrderBy(p => p.Name).ToList();
                PersonLB.ItemsSource = DB.People.Where(p => p.Type == PersonTypes.Customer).OrderBy(p => p.Name).ToList();
                TypeCB.ItemsSource = new[] { TransactionTypes.InHouse, TransactionTypes.Order, TransactionTypes.TakeAway, TransactionTypes.SellBack };
                TypeSearchCB.ItemsSource = new object[] { "الكل", TransactionTypes.InHouse, TransactionTypes.Order, TransactionTypes.TakeAway, TransactionTypes.SellBack };
                TypeCB.SelectedIndex = 1;
                var customers = DB.People.Where(p => p.Type == PersonTypes.Customer).OrderBy(p => p.Name).ToList();
                customers.Insert(0, new Person() { Id = 0, Name = "الكل" });
                PersonSearch.ItemsSource = customers;
                var categories = DB.Categories.Where(c => c.Type == CategoryTypes.Product).OrderBy(p => p.Name).ToList();
                categories.Insert(0, new Category() { Id = 0, Name = "الكل" });
                CategoryCB.ItemsSource = categories;
                CategoryCB.SelectedIndex = 0;
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
                var query = DB.Transactions.AsQueryable();
                if (NumberSearch.Text != "") { query = query.Where(p => p.Number == int.Parse(NumberSearch.Text)); }
                if (PersonSearch.SelectedIndex > 0) { query = query.Where(p => p.PersonId == (int)PersonSearch.SelectedValue); }
                if (TypeSearchCB.SelectedIndex > 0) { query = query.Where(p => p.Type == (TransactionTypes)TypeSearchCB.SelectedValue); }
                else { query = query.Where(t => t.Type != TransactionTypes.Buy && t.Type != TransactionTypes.ReBuy); }
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
                    ViewGrid.DataContext = new Transaction() { Date = DateDTP.Value.Value, Type = TransactionTypes.Order, Delivery = 0 };
                    Number.Text = TransactionsService.GetNumber(DateDTP.Value.Value, TransactionTypes.Order);
                    Pop.IsOpen = true;
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
                    if (Notify.validate("من فضلك أدخل العميل", PersonLB.SelectedIndex, Main.GetWindow(this))) { return; }
                    if (LB.SelectedIndex == -1)
                    {
                        DB.Transactions.Add((Transaction)ViewGrid.DataContext);
                    }
                    DB.SaveChanges();
                    Confirm.Check(true);
                    if (Message.Show("هل تريد طباعة فاتورة", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        PD1.Print();
                    }
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
                Pop.IsOpen = false;
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
                ViewGrid.DataContext = null;
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
                AddBTN.Focus();
                var SaleDetail = (SaleDetail)EditGrid.DataContext;
                var SaleDetails = ((Transaction)ViewGrid.DataContext).SaleDetails;
                if (AddBTN.Content.ToString() == "Add")
                {
                    var oldPurchaseDetail = SaleDetails.FirstOrDefault(p => p.Product == SaleDetail.Product);
                    if (oldPurchaseDetail != null)
                    {
                        oldPurchaseDetail.Amount += SaleDetail.Amount;
                        oldPurchaseDetail.OnPropertyChanged("Amount");
                        oldPurchaseDetail.OnPropertyChanged("Total");
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
                GetTotal();
                EditGrid.DataContext = new SaleDetail() { Amount = 1 };
                // if (!Pop.IsOpen) { Pop.IsOpen = true; }
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
                    AddBTN.Content = "Edit";
                    EditGrid.DataContext = Details_DG.SelectedItem;
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
                    var saledetail = (SaleDetail)EditGrid.DataContext;
                    if (saledetail.Id == 0)
                    {
                        ((Transaction)ViewGrid.DataContext).SaleDetails.Remove(saledetail);
                    }
                    else
                    {
                        DB.SaleDetails.Remove(saledetail);
                    }
                    GetTotal();
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
                Rest_TB.Text = (Purchase.Total - decimal.Parse(DiscountTB.Text) - Purchase.Paid).ToString("0.00");
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
                if (AddBTN.Content.ToString() != "Edit")
                    Price.Text = ((Product)ProductCB.SelectedItem).Price.ToString();// ya salam bs keda :D
            }
            catch
            {


            }
        }
        private void Search(object sender, EventArgs e)
        {
            Pop.IsOpen = true;
        }
        private void DiscountTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var Transaction = ((Transaction)ViewGrid.DataContext);
                if (Transaction != null && LB.IsEnabled == false)
                {
                    Paid_TB.Text = (Transaction.Total - decimal.Parse(DiscountTB.Text)).ToString("0.00");
                    Transaction.Paid = (Transaction.Total - decimal.Parse(DiscountTB.Text));
                }
            }
            catch
            {

            }
        }
        private void DeliveryTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var Transaction = ((Transaction)ViewGrid.DataContext);
                if (Transaction != null && LB.IsEnabled == false)
                {
                    var total = (Transaction.SaleDetails.Sum(p => (p.Price * p.Amount)) + decimal.Parse(DeliveryTB.Text));
                    Paid_TB.Text = Total_TB.Text = total.ToString("0.00");
                    Transaction.Paid = total;
                    Transaction.Total = total;
                }
            }
            catch
            {

            }
        }
        private void GetNumber(object sender, EventArgs e)
        {
            try
            {
                if (sender is ComboBox)
                {
                    if ((TransactionTypes)TypeCB.SelectedItem == TransactionTypes.Order)
                    {
                        TotalsGrid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Auto);
                        TotalsGrid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);
                    }
                    else if ((TransactionTypes)TypeCB.SelectedItem == TransactionTypes.SellBack)
                    {
                        Pop.IsOpen = false;
                    }
                    else
                    {
                        TotalsGrid.ColumnDefinitions[0].Width = TotalsGrid.ColumnDefinitions[1].Width = new GridLength(0);
                    }
                    if (LB.IsEnabled == false)
                        GetTotal();
                }
                if (LB.IsEnabled == false)
                    Number.Text = TransactionsService.GetNumber(DateDTP.Value.Value, (TransactionTypes)TypeCB.SelectedItem);
            }
            catch
            {

            }
        }
        private void GetTotal()
        {
            try
            {
                var transaction = ((Transaction)ViewGrid.DataContext);
                var total = transaction.SaleDetails.Sum(p => (p.Price * p.Amount));
                var type = (TransactionTypes)TypeCB.SelectedItem;
                switch (type)
                {
                    case TransactionTypes.InHouse:
                        total += total * 0.12m;
                        break;
                    case TransactionTypes.Order:
                        total += transaction.Delivery;
                        break;
                }
                transaction.Total = total; transaction.Paid = total;
                Paid_TB.Text = Total_TB.Text = total.ToString("0.00");
            }
            catch
            {

            }
        }
        private void CategoryCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var query = DB.Products.AsQueryable();
                if (CategoryCB.SelectedIndex != 0)
                {
                    query = query.Where(p => p.CategoryId == (int)CategoryCB.SelectedValue);
                }
                ProductsLB.ItemsSource = query.OrderBy(p => p.Name).ToList();
            }
            catch
            {

            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Details_DG.SelectedIndex = -1;
                ProductCB.SelectedItem = (e.OriginalSource as FrameworkElement).DataContext;
                //Pop.IsOpen = false;
                AmountTB.Focus();
                AmountTB.SelectAll();
            }
            catch
            {

            }
        }
        private void PD1_PrintPage(object sender, PrintPageEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            StringFormat sf1 = new StringFormat(StringFormatFlags.DirectionRightToLeft);
            sf1.Alignment = StringAlignment.Near;
            sf1.LineAlignment = StringAlignment.Near;
            StringFormat sf2 = new StringFormat();
            sf2.Alignment = StringAlignment.Far;
            sf2.LineAlignment = StringAlignment.Near;
            float current_height = e.MarginBounds.Top - 20;
            float temp_height = 0;
            var saleDetails = ((Transaction)Details_GD.DataContext).SaleDetails;
            var Customer = ((Transaction)Details_GD.DataContext).Person;
            e.Graphics.DrawString("أسماك السويس", new System.Drawing.Font("Cambria", 16), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Left, current_height, e.MarginBounds.Width, 30), sf);
            current_height += 20;
            e.Graphics.DrawString("Fish'N Prawns", new System.Drawing.Font("Cambria", 14), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Left, current_height, e.MarginBounds.Width, 30), sf);
            current_height += 50;

            e.Graphics.DrawString("الرقــم  :", new System.Drawing.Font("Arial", 14), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - 70, current_height, 70, 30), sf1);
            e.Graphics.DrawString(Number.Text, new System.Drawing.Font("tahoma", 14), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, e.MarginBounds.Width - 70, 30), sf2);
            current_height += 30;

            e.Graphics.DrawString("التاريخ :", new System.Drawing.Font("Arial", 14), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - 70, current_height, 70, 30), sf1);
            e.Graphics.DrawString(DateDTP.Value.Value.ToString(), new System.Drawing.Font("tahoma", 12), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, e.MarginBounds.Width - 70, 30), sf2);
            current_height += 30;
            e.Graphics.DrawString("العميل  :", new System.Drawing.Font("Arial", 14), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - 70, current_height, 70, 30), sf1);
            e.Graphics.DrawString(Customer.Name, new System.Drawing.Font("Arial", 12), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, e.MarginBounds.Width - 70, 30), sf2);
            current_height += 30;
            e.Graphics.DrawString("العنوان  :", new System.Drawing.Font("Arial", 14), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - 70, current_height, 70, 30), sf1);
            e.Graphics.DrawString(Customer.Address, new System.Drawing.Font("tahoma", 12), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, e.MarginBounds.Width - 70, 30), sf2);
            current_height += 30;
            e.Graphics.DrawString("التليفون  :", new System.Drawing.Font("Arial", 14), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - 70, current_height, 70, 30), sf1);
            e.Graphics.DrawString(Customer.Phone, new System.Drawing.Font("tahoma", 12), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, e.MarginBounds.Width - 70, 30), sf2);
            current_height += 35;

            e.Graphics.DrawLine(new Pen(Brushes.Black), e.MarginBounds.Left, current_height, e.MarginBounds.Right, current_height);
            current_height += 5;
            e.Graphics.DrawString("الصنف", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 120, current_height, e.MarginBounds.Width - 120, 30), sf1);
            e.Graphics.DrawString("السعر", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 80, current_height, 40, 30), sf1);
            e.Graphics.DrawString("الكمية", new System.Drawing.Font("Arial", 9), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 50, current_height, 30, 30), sf1);
            e.Graphics.DrawString("الإجمالي", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, 50, 30), sf1);
            current_height += 22;
            e.Graphics.DrawLine(new Pen(Brushes.Black), e.MarginBounds.Left, current_height, e.MarginBounds.Right, current_height);
            current_height += 5;
            foreach (var saledetail in saleDetails)
            {
                var Product = saledetail.Product;
                temp_height = e.Graphics.MeasureString(Product.Name, new System.Drawing.Font("Arial", 8), e.MarginBounds.Width - 120).Height;
                temp_height = temp_height < 20 ? 15 : 30;
                e.Graphics.DrawString(Product.Name, new System.Drawing.Font("Arial", 8), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 120, current_height, e.MarginBounds.Width - 120, temp_height), sf1);
                e.Graphics.DrawString(Product.Price.ToString(), new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 80, current_height, 40, temp_height), sf2);
                e.Graphics.DrawString(saledetail.Amount.ToString(), new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 50, current_height, 30, temp_height), sf2);
                e.Graphics.DrawString(saledetail.Total.ToString(), new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, 50, temp_height), sf2);
                current_height += temp_height;
            }
            e.Graphics.DrawLine(new Pen(Brushes.Black), e.MarginBounds.Left, current_height, e.MarginBounds.Right, current_height);
            current_height += 5;
            if ((TransactionTypes)TypeCB.SelectedItem == TransactionTypes.Order)
            {
                e.Graphics.DrawString("المجموع", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 50, current_height, 50, 30), sf1);
                e.Graphics.DrawString(saleDetails.Sum(s => s.Amount * s.Price).ToString("0.00"), new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, 50, 30), sf2);
                current_height += 30;
                e.Graphics.DrawString("التوصيل", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 50, current_height, 50, 30), sf1);
                e.Graphics.DrawString(DeliveryTB.Text, new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, 50, 30), sf2);
                current_height += 30;

            }
            else if ((TransactionTypes)TypeCB.SelectedItem == TransactionTypes.InHouse)
            {
                e.Graphics.DrawString("المجموع", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 50, current_height, 50, 30), sf1);
                e.Graphics.DrawString(saleDetails.Sum(s => s.Amount * s.Price).ToString("0.00"), new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, 50, 30), sf2);
                current_height += 30;
                e.Graphics.DrawString("الخدمة", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 50, current_height, 50, 30), sf1);
                e.Graphics.DrawString((decimal.Parse(Total_TB.Text) * 0.12m).ToString("0.00"), new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, 50, 30), sf2);
                current_height += 30;

            }
            e.Graphics.DrawString("الإجمالي", new System.Drawing.Font("Arial", 10), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width + 50, current_height, 50, 30), sf1);
            e.Graphics.DrawString(Total_TB.Text, new System.Drawing.Font("Tahoma", 7), System.Drawing.Brushes.Black, new RectangleF(e.MarginBounds.Right - e.MarginBounds.Width, current_height, 50, 30), sf2);
        }
        private void Print_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PD1.Print();
            }
            catch
            {

            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Notify.validate("من فضلك أدخل العميل", PersonLB.SelectedIndex, Main.GetWindow(this))) { return; }
                DB.Transactions.Add((Transaction)ViewGrid.DataContext);
                DB.SaveChanges();
                Pop.IsOpen = false;
                Confirm.Check(true);
                if (Message.Show("هل تريد طباعة فاتورة", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    PD1.Print();
                }
                ViewGrid.DataContext = null; EditGrid.DataContext = null;
                EditGrid.DataContext = new SaleDetail() { Amount = 1 };
                Form.Set_Style(TotalsGrid, Operations.Add);
                ViewGrid.DataContext = new Transaction() { Date = DateDTP.Value.Value, Type = (TransactionTypes)TypeCB.SelectedItem, Delivery = 0 };
                Number.Text = TransactionsService.GetNumber(DateDTP.Value.Value, TransactionTypes.Order);
                Pop.IsOpen = true;

            }
            catch
            {
                Confirm.Check(false);
            }
        }
        private void Number_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Enter && Number.Text != "" && (TransactionTypes)TypeCB.SelectedItem == TransactionTypes.SellBack)
                {
                    var num = int.Parse(Number.Text);
                    foreach (var trd in DB.Transactions.FirstOrDefault(t => t.Number == num && (t.Type != TransactionTypes.Buy && t.Type != TransactionTypes.ReBuy)).SaleDetails)
                    {
                        ((Transaction)ViewGrid.DataContext).SaleDetails.Add(new SaleDetail()
                        {
                            Product = trd.Product,
                            ProductId = trd.ProductId,
                            Amount = trd.Amount,
                            Price = trd.Price,
                            Total = trd.Total
                        });
                    }
                    GetTotal();
                    e.Handled = true;
                }
            }
            catch
            {

            }
        }

        private void SelectCustomer(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (DB.People.SingleOrDefault(p => p.Name == PersonTB.Text) != null || PersonTB.Text == "")
                { return; }

                CustomersPop.IsOpen = true;
                int phone = 0;
                if (int.TryParse(PersonTB.Text, out phone))
                    PersonLB.ItemsSource = DB.People.Where(p => p.Type == PersonTypes.Customer && (p.Phone.StartsWith(PersonTB.Text) || p.Mobile.StartsWith(PersonTB.Text))).OrderBy(p => p.Name).ToList();
                else
                    PersonLB.ItemsSource = DB.People.Where(p => p.Type == PersonTypes.Customer && p.Name.StartsWith(PersonTB.Text)).OrderBy(p => p.Name).ToList();
            }
            catch
            {

            }
        }

        private void PersonLBChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                CustomersPop.IsOpen = false;
        }

        private void PersonLB_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CustomersPop.IsOpen = false;
        }

        private void CustomerDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Down)
                    PersonLB.Focus();
            }
            catch
            {

            }
        }

        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            try
            {
                Pop.IsOpen = false;
                AddCustomer a = new AddCustomer(DB, PersonTB.Text);
                a.ShowDialog();
                Pop.IsOpen = true;
                PersonTB.Text = "";
                PersonLB.ItemsSource = DB.People.Where(p => p.Type == PersonTypes.Customer).OrderBy(p => p.Name).ToList();
                PersonLB.SelectedItem = FishRestaurant.WPF.AddCustomer.Customer;
                CustomersPop.IsOpen = false;
            }
            catch
            {

            }
        }

    }
}
