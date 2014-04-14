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
    public partial class Components : Page
    {
        FRContext DBContext;
        public Components()
        {
            InitializeComponent();
            DBContext = new FRContext();
            FillLB();
            InitializeLookups();
        }

        private void InitializeLookups()
        {
            try
            {
                Category_CB.ItemsSource = DBContext.Categories.OrderBy(c => c.Name).ToList();
            }
            catch
            {

            }
        }
        private void FillLB()
        {

            try
            {               
                  var query =  DBContext.Components.Where(c => c.Name.StartsWith(Component_TB.Text));
                  if (Category_CB.SelectedIndex > 0) { query = query.Where(c => c.CategoryId == (int)Category_CB.SelectedValue); }
                  ComponentsDG.ItemsSource = query.OrderBy(c => c.Name).ToList();

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
                    pop.IsOpen = true;
                }
                else
                {
                  
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
                if (ComponentsDG.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا الصنف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        DBContext.Components.Remove((Component)ComponentsDG.SelectedItem);
                        DBContext.SaveChanges();
                        FillLB();
                    }
                }
            }
            catch
            {

            }
        }


        private void FillDG(object sender, EventArgs e)
        {
            FillLB();
        }


    }
}
