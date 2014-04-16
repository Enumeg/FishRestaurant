using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Source;
using FishRestaurant.Model.Entities;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Purchase : Page
    {
        FRContext DBContext;
        public Purchase()
        {
            InitializeComponent();
            DBContext = new FRContext();
            Fill_Patients_LB();
            InitializeLookups();
        }

        private void InitializeLookups()
        {
            try
            {
                //Company_CB.ItemsSource = Health_Company_CB.ItemsSource = DBContext.companies.OrderBy(c => c.name).ToList();
                //Gender_CB.ItemsSource = Enum.GetValues(typeof(Gender));
            }
            catch
            {

            }
        }
        private void Fill_Patients_LB()
        {

            try
            {
                //Patients_LB.ItemsSource = DBContext.patients.Where(p => p.name.StartsWith(Name_TB.Text) && p.phone.StartsWith(Tele_TB.Text)).OrderBy(P => P.name).ToList();
            }
            catch
            {

            }

        }

        private void EditPanel_Edit(object sender, EventArgs e)
        {
            try
            {
                //if (((Button)sender).Name.Split('_')[0] == "Add")
                //{
                //    Patients_LB.SelectedIndex = -1;
                //    Main_Grid.DataContext = new Patient() { gender = Gender.ذكر };
                //    Form.Set_Style(Main_Grid, Operations.Add);
                //}
                //else
                //{
                //    Form.Set_Style(Main_Grid, Operations.Edit);
                //}
                //Main_Grid.RowDefinitions[4].Height = new GridLength(35);
                //Patients_LB.IsEnabled = false;
            }
            catch
            {

            }
        }

        private void EditPanel_Delete(object sender, EventArgs e)
        {
            try
            {
                //if (Patients_LB.SelectedIndex != -1)
                //{

                //    if (Message.Show("هل تريد حذف هذا العميل", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                //    {
                //        DBContext.patients.Remove((Patient)Patients_LB.SelectedItem);
                //        DBContext.SaveChanges();
                //        Fill_Patients_LB();
                //    }
                //}
            }
            catch
            {

            }
        }

        private void Submit(object sender, EventArgs e)
        {
            try
            {
                //if (((Button)sender).Name.Split('_')[0] == "Save")
                //{
                //    if (Patients_LB.SelectedIndex == -1)
                //    {
                //        DBContext.patients.Add((Patient)Main_Grid.DataContext);
                //    }
                //    DBContext.SaveChanges();
                //}
                //Main_Grid.SetBinding(DataContextProperty, new Binding() { ElementName = "Patients_LB", Path = new PropertyPath("SelectedItem") });
                //Main_Grid.RowDefinitions[4].Height = new GridLength(0);
                //Patients_LB.IsEnabled = true;
                //Fill_Patients_LB();
                //Form.Set_Style(Main_Grid, Operations.View);
                //Confirm.Check(true);
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

            }
            catch
            {

            }
        }

        private void Fill_LB(object sender, TextChangedEventArgs e)
        {
            Fill_Patients_LB();
        }


    }
}
