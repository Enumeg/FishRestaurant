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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Management.xaml
    /// </summary>
    public partial class Management : Window
    {

        //Data
        Products ProductsPage;
        People CustomersPage;
        People SuppliersPage;
        Users UsersPage;
        Components ComponentPage;

        //Accounts
        Outcomes Outcome_Page;
        Income Income_Page;
        Installments CustomerInstallmentsPage;
        Installments SupplierInstallmentsPage;
        Stock StockPage;
 

        //Transactions
        Sales SalesPage;
        Purchases PurchasesPage;
        Purchases PurchasesBackPage;
        ComponentsDamage ComponentDamagePage;
        ProductsDamage ProductsDamagePage;


        string Selected_Button = "";
        DoubleAnimation ani;
     


        public Management()
        {
            InitializeComponent();
            ani = new DoubleAnimation(0, 1, new TimeSpan(0, 0, 0, 0, 700));
            InitalizePages();
        }

        private void InitalizePages()
        {
             ProductsPage  =  new Products();
             CustomersPage = new People(Model.Entities.PersonTypes.Customer) ;
             SuppliersPage = new People( Model.Entities.PersonTypes.Supplier);
             UsersPage =  new Users();
             ComponentPage =  new Components();

            //Accounts
             Outcome_Page =  new Outcomes();
             Income_Page =  new Income();
             CustomerInstallmentsPage =  new Installments(Model.Entities.PersonTypes.Customer);
             SupplierInstallmentsPage =  new Installments(Model.Entities.PersonTypes.Supplier);
             StockPage = new Stock();

            //Transactions
             SalesPage =  new Sales();
             PurchasesPage= new Purchases(Model.Entities.TransactionTypes.Buy);
             PurchasesBackPage = new Purchases(Model.Entities.TransactionTypes.ReBuy);

             ComponentDamagePage =  new ComponentsDamage();
             ProductsDamagePage = new ProductsDamage();

        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            if (btn == null)
            {
                return;
            }
            var Page = new Page();
            try
            {

                switch (btn.Name)
                {

                    case "BTN_Supplier": Page = SuppliersPage;break;
                    case "BTN_Customers": Page = CustomersPage; break;
                    case "BTN_Users":  UsersPage.ShowDialog(); break;
                    case "BTN_Components": Page = ComponentPage; break;
                    case "BTN_Products": Page = ProductsPage; break;

                    case "BTN_Incomes": Page = Income_Page; break;
                    case "BTN_Outcomes": Page = Outcome_Page; break;
                    case "BTN_Customers_payments": Page = CustomerInstallmentsPage; break;
                    case "BTN_Suppliers_payments": Page = SupplierInstallmentsPage; break;
                    case "BTN_Stock": Page = StockPage; break;

                    case "BTN_sales": Page = SalesPage; break;
                    case "BTN_Purchases": Page = PurchasesPage; break;
                    case "BTN_Purchases_Back": Page = PurchasesBackPage; break;
                    case "BTN_Damaged_Component": Page = ComponentDamagePage; break;
                    case "BTN_Damaged_Product": Page = ProductsDamagePage; break;
                }

                Frame.Navigate(Page);
                 
                Set_Selected(btn);
                

            }
            catch 
            {
                
                throw;
            }

        }



        private void Set_Selected(Button button)
        {
            try
            {

                if (Selected_Button != "")
                {
                    ((Button)FindName(Selected_Button)).Style = FindResource("Side") as Style;
                }
                    button.Style = FindResource("Selected_Side") as Style;
                    Selected_Button = button.Name;
                
            }
            catch
            {

            }
        }


        private void Frame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            try
            {
                Page page = Frame.Content as Page;
                page.BeginAnimation(Page.OpacityProperty, ani);
               
            }
            catch
            {

            }
        }

        ///////////////////////////
    
    }
}
