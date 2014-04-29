using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using FishRestaurant.Model.Services;
using FishRestaurant.Model.Entities;
using Source;


namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Stock.xaml
    /// </summary>
    public partial class Stock : Page
    {
        FrContext DB;
        public Stock()
        {
            InitializeComponent();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DB = new FrContext();
                FillDG();
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
                var categories = DB.Categories.Where(c => c.Type == CategoryTypes.Compontent).OrderBy(c => c.Name).ToList();
                categories.Insert(0, new Category() { Id = 0, Name = "الكل" });
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


                ComponentsDG.ItemsSource = query.OrderBy(c => c.Name).ToList().Select(c => new
                {
                    Category = c.Category.Name,
                    Component = c.Name,
                    AmountLimit = c.AmountLimit,
                    ShopStock = ComponentsService.GetShopStock(c),
                    StoreStock = ComponentsService.GetStock(c),
                    Status = c.StoreStock > c.AmountLimit ? 1 : c.StoreStock == c.AmountLimit ? 0 : -1
                }
                ).ToList();
            }
            catch
            {

            }

        }
        private void FillDG(object sender, EventArgs e)
        {
            FillDG();
        }
    }
}
