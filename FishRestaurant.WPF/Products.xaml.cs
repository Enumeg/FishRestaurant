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
        FRContext DB;

        public Products()
        {
            InitializeComponent();           
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DB = new FRContext();
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
                ComponentsCB.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
                Units.ItemsSource = Enum.GetValues(typeof(Units));
                
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

                LB.ItemsSource = query.OrderBy(c => c.Name).ToList();
            }
            catch
            {

            }

        }

        #region ProductPanel
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
                    var product = LB.SelectedItem as Product;
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
                if (LB.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا الصنف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        DB.Products.Remove((Product)LB.SelectedItem);
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

        #endregion

        #region ComponentsPanel
        private void EditPanel_Edit(object sender, EventArgs e)
        {
            try
            {
                if (((Button)sender).Name.Split('_')[0] == "Add")
                {
                    ComPopup.DataContext = new ProductComponents() { };
                    SumbitBTN.Tag = "Add";
                }
                else
                {
                    SumbitBTN.Tag = "Edit";
                    ComPopup.DataContext = ComponentsLB.SelectedItem;
                }
                ComPopup.IsOpen = true;

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
                        ((Product)LB.SelectedItem).ProductComponents.Remove((ProductComponents)ComponentsLB.SelectedItem);
                        DB.SaveChanges();
                        FillLB();
                    }
                }
            }
            catch
            {

            }
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var ProductComs = ComPopup.DataContext as ProductComponents;
                if (ProductComs.Id == 0) { ((Product)LB.SelectedItem).ProductComponents.Add(ProductComs); }
                DB.SaveChanges();
                if ((bool)New.IsChecked)
                {
                    ComPopup.DataContext = new Product();
                }
                else
                {
                    ComPopup.IsOpen = false;
                }
                Confirm.Check(true);
                FillLB();
            }
            catch
            {
                Confirm.Check(false);
            }
        }
        private void FillComponentsLB()
        {
            try
            {
                ComponentsLB.ItemsSource = ((Product)LB.SelectedItem).ProductComponents;
            }
            catch
            {

            }
        }

        #endregion
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
        private void LB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                FillComponentsLB();
                var product = LB.SelectedItem as Product;
                if (product.Image != null)
                    ProductImg.Source = ImageByteConverter.byteArrayToImage(product.Image);
                else
                    ProductImg.Source = new BitmapImage(new Uri("/Images/question_mark_icon.jpg", UriKind.Relative));
            }
            catch
            {

            }
        }
    }
}
