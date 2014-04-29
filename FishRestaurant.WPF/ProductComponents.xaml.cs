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
using System.Windows.Shapes;
using FishRestaurant.Model.Entities;
using Source;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for ProductsComponents.xaml
    /// </summary>
    public partial class ProductsComponents : Window
    {
        FrContext DB;
        Product Product;
        public ProductsComponents(FrContext Db, Product product)
        {
            InitializeComponent();
            DB = Db;
            Product = product;
            InitializeLookups();
        }
        private void InitializeLookups()
        {
            try
            {
                var categories = DB.Categories.Where(c => c.Type == CategoryTypes.Compontent).OrderBy(c => c.Name).ToList();
                categories.Insert(0, new Category() { Id = 0, Name = "الكل" });
                CategoryCB.ItemsSource = categories;
                ComponentsCB.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
                Units.ItemsSource = Enum.GetValues(typeof(Units));
                FillLB();
            }
            catch
            {

            }
        }
        private void FillLB()
        {
            try
            {
                ComponentsLB.ItemsSource = DB.ProductsComponents.Where(p => p.ProductId == Product.Id).OrderBy(c => c.Component.Name).ToList();
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
                    MainGrid.DataContext = new ProductComponents() { };
                    ComponentsLB.SelectedIndex = -1;
                }
                else
                {
                    if (ComponentsLB.SelectedIndex == -1)
                    {
                        Message.Show("من فضلك أختار المكون أولاً", MessageBoxButton.OK);
                    }
                    else
                    {
                        MainGrid.DataContext = ComponentsLB.SelectedItem;
                    }
                }
                MainGrid.RowDefinitions[0].Height = new GridLength(190);
                MainGrid.RowDefinitions[1].Height = MainGrid.RowDefinitions[2].Height = new GridLength(0);
                ComponentsLB.IsEnabled = false;
            }
            catch
            {

            }
        }
        private void EditPanel_Delete(object sender, EventArgs e)
        {
            try
            {
                if (ComponentsLB.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا الصنف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        DB.ProductsComponents.Remove((ProductComponents)ComponentsLB.SelectedItem);
                        DB.SaveChanges();
                        FillLB();
                    }
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
                    var ProductComs = MainGrid.DataContext as ProductComponents;
                    if (ProductComs.Id == 0) { Product.ProductComponents.Add(ProductComs); }
                    DB.SaveChanges();
                    Confirm.Check(true);
                }

                if ((bool)New.IsChecked)
                {
                    MainGrid.DataContext = new Product();
                }
                else
                {
                    MainGrid.RowDefinitions[0].Height = new GridLength(0);
                    MainGrid.RowDefinitions[1].Height = new GridLength(300);
                    MainGrid.RowDefinitions[2].Height = new GridLength(35);
                    ComponentsLB.IsEnabled = true;
                }

                FillLB();
            }
            catch
            {
                Confirm.Check(false);
            }
        }

        private void GetCompontents(object sender, EventArgs e)
        {
            try
            {
                if (CategoryCB.SelectedIndex > 0)
                    ComponentsCB.ItemsSource = DB.Components.Where(c => c.CategoryId == ((Category)CategoryCB.SelectedItem).Id).OrderBy(c => c.Name).ToList();
                else
                    ComponentsCB.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
            }
            catch
            {

            }
        }
    }
}
