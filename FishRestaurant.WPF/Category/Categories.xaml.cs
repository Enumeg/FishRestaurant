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
    /// Interaction logic for Type.xaml
    /// </summary>
    public partial class Categories : Window
    {
        FRContext DB;
        CategoryTypes Type;
        public Categories(CategoryTypes type)
        {
            InitializeComponent();
            DB = new FRContext();
            Type = type;
            FillLB();
        }
        private void FillLB()
        {
            try
            {
                if (LB.IsEnabled)
                {
                    LB.ItemsSource = DB.Categories.Where(c => c.Name.StartsWith(Category_TB.Text) && c.Type == Type).OrderBy(o => o.Name).ToList();
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
                    if (LB.SelectedIndex == -1) { DB.Categories.Add(new Category() { Name = Category_TB.Text, Type = Type }); }
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
                if (Message.Show("هل تريد حذف هذه الفئة", MessageBoxButton.YesNoCancel, 10) == MessageBoxResult.Yes)
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
