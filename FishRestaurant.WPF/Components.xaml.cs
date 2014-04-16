﻿using System;
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
        FRContext DB;
        public Components()
        {
            InitializeComponent();
            DB = new FRContext();
            FillDG();
            InitializeLookups();
        }

        private void InitializeLookups()
        {
            try
            {
                var categories = DB.Categories.OrderBy(c => c.Name).ToList();
                categories.Insert(0, new Category() { Id = 0, Name = "الكل" });
                CategoryCB.ItemsSource = DB.Categories.OrderBy(c => c.Name).ToList();
                CategorySearchCB.ItemsSource = categories;
            }
            catch
            {

            }
        }
        private void FillDG()
        {

            try
            {
                var query = DB.Components.Where(c => c.Name.StartsWith(ComponentSearchTB.Text));
                if (CategorySearchCB.SelectedIndex > 0) { query = query.Where(c => c.CategoryId == (int)CategorySearchCB.SelectedValue); }
                ComponentsDG.ItemsSource = query.OrderBy(c => c.Name).ToList().Select(c => {c.Status = c.Stock > c.AmountLimit ? 1 : c.Stock == c.AmountLimit ? 0 : -1; return c;});
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
                    pop.DataContext = new Component();
                }
                else
                {
                    pop.DataContext = ComponentsDG.SelectedItem as Component;
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
                if (ComponentsDG.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا الصنف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        DB.Components.Remove((Component)ComponentsDG.SelectedItem);
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
            FillDG();
        }

        private void pop_Closed(object sender, EventArgs e)
        {
            try
            {
                //pop.DataContext = null;
            }
            catch
            {

            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Component = pop.DataContext as Component;
                if (Component.Id == 0) { DB.Components.Add(Component); }
                DB.SaveChanges();
                if ((bool)New.IsChecked)
                {
                    pop.DataContext = new Component();
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
        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Categories c = new Categories(CategoryTypes.Compontent);
                c.ShowDialog();
                InitializeLookups();
            }
            catch
            {

            }
        }

        
    }
}
