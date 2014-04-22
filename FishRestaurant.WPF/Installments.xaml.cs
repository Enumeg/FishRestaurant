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
using System.Data.Entity;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Installments.xaml
    /// </summary>
    public partial class Installments : Page
    {
        FRContext DB;
        PersonTypes Type;
        public Installments(PersonTypes type)
        {
            InitializeComponent();
            Type = type;
            Title += type == PersonTypes.Customer ? "العملاء" : "الموردين";
            PersonTK.Text = type == PersonTypes.Customer ? "العميل :" : "المورد :";

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
                PersonCB.ItemsSource = DB.People.Where(p => p.Type == Type).OrderBy(p => p.Name).ToList();
                FillDG();
            }
            catch
            {

            }
        }
        private void FillDG()
        {
            try
            {
                InstallmentsDG.ItemsSource = DB.Installments.Where(i => i.Person.Type == Type && DbFunctions.TruncateTime(i.Date) >= DbFunctions.TruncateTime(From_DTP.Value.Value)
                    && DbFunctions.TruncateTime(i.Date) <= DbFunctions.TruncateTime(To_DTP.Value.Value)).OrderBy(i => i.Date).ToList();
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
                    pop.DataContext = new Installment() { Date= DateTime.Now};
                }
                else
                {
                    pop.DataContext = InstallmentsDG.SelectedItem as Installment;
                }

                pop.IsOpen = true;

            }
            catch
            {

            }
        }
        private void EditPanel_Delete(object sender, EventArgs e)
        {
            try
            {
                if (InstallmentsDG.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا القسط", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        DB.Installments.Remove((Installment)InstallmentsDG.SelectedItem);
                        DB.SaveChanges();
                        FillDG();
                    }
                }
            }
            catch
            {

            }
        }
        private void FillDG(object sender, EventArgs e)
        {
            if(this.IsLoaded)
            FillDG();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Installment = pop.DataContext as Installment;
                if (Installment.Id == 0) { DB.Installments.Add(Installment); }
                DB.SaveChanges();
                if ((bool)New.IsChecked)
                {
                    pop.DataContext = new Installment();
                }
                else
                {
                    pop.IsOpen = false;
                }
                Confirm.Check(true);
                FillDG();
            }
            catch
            {
                Confirm.Check(false);
            }
        }
    }
}
