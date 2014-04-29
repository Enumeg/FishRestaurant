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
    public partial class OutcomeTypes : Window
    {
        FrContext DB;
        public OutcomeTypes()
        {
            InitializeComponent();
            DB = new FrContext();
            FillLB();
        }
        private void FillLB()
        {
            try
            {
                if (LB.IsEnabled)
                {
                    LB.ItemsSource = DB.OutcomeTypes.Where(c => c.Name.StartsWith(Name.Text)).OrderBy(o => o.Name).ToList();
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
                    if (LB.SelectedIndex == -1) { DB.OutcomeTypes.Add(new OutcomeType() { Name = Name.Text }); }
                    DB.SaveChanges();
                    Confirm.Check(true);
                }
                Name.Text = "";
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
                    DB.OutcomeTypes.Remove((OutcomeType)LB.SelectedItem);
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
