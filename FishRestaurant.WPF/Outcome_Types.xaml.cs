using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Source;
using FishRestaurant.Model.Entities;
using System.Collections.Generic;
namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Outcome_Types.xaml
    /// </summary>
    public partial class Outcome_Types : Window
    {

        FRContext DB;


        public Outcome_Types()
        {
            InitializeComponent();
            DB = new FRContext();
            FillLB();
        }


        private void FillLB()
        {
            try
            {
                if (LB.IsEnabled)
                {
                    LB.ItemsSource = DB.Categories.Where(c => c.Name.StartsWith(Category_TB.Text)).OrderBy(o => o.Name).ToList();

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
                    if (LB.SelectedIndex == -1) { DB.Categories.Add(new Category() { Name = Category_TB.Text }); }
                    DB.SaveChanges();
                    Confirm.Check(true);
                }
                Category_TB.Text = "";
                if (!(bool)New.IsChecked)
                {
                    Main_GD.RowDefinitions[1].Height = new GridLength(0);
                    LB.IsEnabled = true;
                    FillLB();
                }
            }
            catch
            {
                Confirm.Check(false);
            }
        }

        private void EditPanel_Edit(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    LB.SelectedIndex = -1;
                }
                Main_GD.RowDefinitions[1].Height = new GridLength(35);
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
                if (Message.Show("هل تريد حذف هذا النوع من المصروفات", MessageBoxButton.YesNoCancel, 10) == MessageBoxResult.Yes)
                {
                    DB.Categories.Remove((Category)LB.SelectedItem);
                    DB.SaveChanges();
                    FillLB();
                }
            }
            catch
            {

            }
        }

        private void FillLB(object sender, EventArgs e)
        {
            try
            {
                FillLB();
            }
            catch
            {

            }
        }

    }
}
