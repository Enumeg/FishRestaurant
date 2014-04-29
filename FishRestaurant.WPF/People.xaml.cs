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
            DB = new FrContext();
            Type = type;
            Title = type == PersonTypes.Customer ? "العملاء" : "الموردين";
            FillLB();
        }
        private void FillLB()
        {
            try
            {
                if (LB.IsEnabled)
                {
                    var query = DB.People.Where(c => c.Type == Type && c.Name.StartsWith(NameSearchTB.Text));
                    if(MobileSearchTB.Text != ""){query = query.Where(c=>c.Mobile.StartsWith(MobileSearchTB.Text));}
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
                        if (DB.People.First(p => p.Address == AddressTB.Text && p.Phone == TelephoneTB.Text) != null)
                            DB.People.Add(MainGrid.DataContext as Person);
                        else
                            Message.Show("يوجد عميل بنفس العنوان ورقم التليفون", MessageBoxButton.OKCancel);
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
            }
            catch
            {

            }
        }

        private void GetAccounts(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
