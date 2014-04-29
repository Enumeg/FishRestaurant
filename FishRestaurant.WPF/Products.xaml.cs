using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Source;
using FishRestaurant.Model.Entities;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using Source.Converters;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Page
    {
        FrContext DB;

        public Products()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DB = new FrContext();
                FillLB();
                InitializeLookups();
            }
            catch
            {

            }
        }
        private void InitializeLookups()
        {
            try
            {
                var categories = DB.Categories.Where(c => c.Type == CategoryTypes.Product).OrderBy(c => c.Name).ToList();
                categories.Insert(0, new Category() { Id = 0, Name = "الكل" });
                CategorySearch.ItemsSource = categories;
                CategoryCB.ItemsSource = DB.Categories.Where(c => c.Type == CategoryTypes.Product).OrderBy(c => c.Name).ToList();
            }
            catch
            {

            }
        }
        private void FillLB()
        {

            try
            {
                var query = DB.Products.Where(c => c.Name.StartsWith(ProductSearch.Text));
                if (CategorySearch.SelectedIndex > 0)
                {
                    query = query.Where(c => c.CategoryId == (int)CategorySearch.SelectedValue);
                }

                ProductsDG.ItemsSource = query.OrderBy(c => c.Name).ToList();
            }
            catch
            {

            }

        }
        private void EP_Edit(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    pop.DataContext = new Product();
                    Img.Source = new BitmapImage(new Uri("/Images/question_mark_icon.jpg", UriKind.Relative));
                }
                else
                {
                    var product = ProductsDG.SelectedItem as Product;
                    pop.DataContext = product;
                    if (product.Image != null)
                        Img.Source = ImageByteConverter.byteArrayToImage(product.Image);
                    else
                        Img.Source = new BitmapImage(new Uri("/Images/question_mark_icon.jpg", UriKind.Relative));
                }
                pop.IsOpen = true;

            }
            catch
            {

            }
        }
        private void EP_Delete(object sender, EventArgs e)
        {
            try
            {
                if (ProductsDG.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا الصنف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        DB.Products.Remove((Product)ProductsDG.SelectedItem);
                        DB.SaveChanges();
                        FillLB();
                    }
                }
            }
            catch
            {

            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Product = pop.DataContext as Product;
                if (!Img.Source.ToString().StartsWith("pack") && !Img.Source.ToString().StartsWith("System"))
                {
                    Product.Image = ImageByteConverter.imageToByteArray(System.Drawing.Image.FromFile(Img.Source.ToString().Remove(0, 8)));
                }
                if (Product.Id == 0) { DB.Products.Add(Product); }
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
                FillLB();
            }
            catch
            {
                Confirm.Check(false);
            }
        }
        private void BTNImg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                if ((bool)dlg.ShowDialog())
                {
                    if (!string.IsNullOrEmpty(dlg.FileName))
                    {
                        Img.SetValue(System.Windows.Controls.Image.SourceProperty, new BitmapImage(new Uri(dlg.FileName)));
                    }
                }
            }
            catch
            {

            }
        }
        private void FillLB(object sender, EventArgs e)
        {
            FillLB();
        }
        private void Categories_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Categories c = new Categories(CategoryTypes.Product);
                c.ShowDialog();
                InitializeLookups();
            }
            catch
            {

            }
        }
        private void Components_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ProductsDG.SelectedIndex > -1)
                {
                    ProductsComponents c = new ProductsComponents(DB, ProductsDG.SelectedItem as Product);
                    c.ShowDialog();
                    InitializeLookups();
                }
                else
                {
                    Message.Show("من فضلك أختار المنتج أولاً", MessageBoxButton.OK);
                }
            }
            catch
            {

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
    }
}
