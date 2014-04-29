using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FishRestaurant.Model.Entities;
using Source;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Damage_Page.xaml
    /// </summary>
    public partial class ProductsDamage : Page
    {
        FrContext DB;
        public ProductsDamage()
        {
            InitializeComponent();
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
                //Products
                Product.ItemsSource = DB.Products.OrderBy(c => c.Name).ToList();
                var components = DB.Products.OrderBy(c => c.Name).ToList();
                components.Insert(0, new Product() { Id = 0, Name = "الكل" });
                ProductSearch.ItemsSource = components;
                //Types
                var categories = DB.Categories.Where(c => c.Type == CategoryTypes.Product).OrderBy(c => c.Name).ToList();
                categories.Insert(0, new Category() { Id = 0, Name = "الكل" });
                Category.ItemsSource = categories;
                CategorySearch.ItemsSource = categories;

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
                var query = DB.ProductsDamage.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(SearchFromDateDTP.Value.Value) &&
                   DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(SearchToDateDTP.Value.Value));
                if (CategorySearch.SelectedIndex > 0) { query = query.Where(c => c.Product.CategoryId == ((Category)CategorySearch.SelectedItem).Id); }
                if (ProductSearch.SelectedIndex > 0) { query = query.Where(c => c.Product.Id == ((Product)ProductSearch.SelectedItem).Id); }
                ProductsDamageDG.ItemsSource = query.OrderBy(c => c.Date).ToList();
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
                    pop.DataContext = new ProductDamage() { Date = DateTime.Now };
                }
                else
                {
                    pop.DataContext = ProductsDamageDG.SelectedItem as ProductDamage;
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
                if (ProductsDamageDG.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا التالف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {                      
                        DB.ProductsDamage.Remove((ProductDamage)ProductsDamageDG.SelectedItem);
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
            try
            {
                if (sender == CategorySearch)
                {
                    var query = DB.Products.AsQueryable();
                    if (CategorySearch.SelectedIndex > 0)
                    { query = query.Where(c => c.CategoryId == ((Category)CategorySearch.SelectedItem).Id); }
                    var components = query.OrderBy(c => c.Name).ToList();
                    components.Insert(0, new Product() { Id = 0, Name = "الكل" });
                    ProductSearch.ItemsSource = components;
                }
                FillDG();
            }
            catch
            {

            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var ProductDamage = pop.DataContext as ProductDamage;
                if (ProductDamage.Id == 0)
                {
                    DB.ProductsDamage.Add(ProductDamage);                    
                }              
                DB.SaveChanges();
                if ((bool)New.IsChecked)
                {
                    pop.DataContext = new Product();
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
                Initialize();
            }
            catch
            {

            }
        }
        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Category.SelectedIndex > 0)
                    Product.ItemsSource = DB.Products.Where(c => c.CategoryId == ((Category)Category.SelectedItem).Id).OrderBy(c => c.Name).ToList();
                else
                    Product.ItemsSource = DB.Products.OrderBy(c => c.Name).ToList();
            }
            catch
            {

            }
        }

    }
}
