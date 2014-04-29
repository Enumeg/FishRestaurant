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
    public partial class ComponentsDamage : Page
    {
        decimal amount;
        Units Unit;
        FrContext DB;
        public ComponentsDamage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Initialize();
                Units.ItemsSource = Enum.GetValues(typeof(Units));
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
                //components
                Component.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
                var components = DB.Components.OrderBy(c => c.Name).ToList();
                components.Insert(0, new Component() { Id = 0, Name = "الكل" });
                ComponentSearch.ItemsSource = components;
                //Types
                var categories = DB.Categories.Where(c => c.Type == CategoryTypes.Compontent).OrderBy(c => c.Name).ToList();
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
                var query = DB.ComponentsDamage.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(SearchFromDateDTP.Value.Value) &&
                   DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(SearchToDateDTP.Value.Value));
                if (CategorySearch.SelectedIndex > 0) { query = query.Where(c => c.Component.CategoryId == ((Category)CategorySearch.SelectedItem).Id); }
                if (ComponentSearch.SelectedIndex > 0) { query = query.Where(c => c.Component.Id == ((Component)ComponentSearch.SelectedItem).Id); }
                ComponentsDamageDG.ItemsSource = query.OrderBy(c => c.Date).ToList();
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
                    pop.DataContext = new ComponentDamage() { Date = DateTime.Now };
                }
                else
                {
                    var ComponentDamage = ComponentsDamageDG.SelectedItem as ComponentDamage;
                    pop.DataContext = ComponentDamage;
                    amount = ComponentDamage.Amonut;
                    Unit = ComponentDamage.Unit;
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
                if (ComponentsDamageDG.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا التالف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {
                        var ComponentDamage = (ComponentDamage)ComponentsDamageDG.SelectedItem;
                        var Amount = ComponentDamage.Unit == Model.Entities.Units.جرام ? ComponentDamage.Amonut * 0.001m : ComponentDamage.Amonut;
                        //DB.Components.Find(Outcome.Component.Id).Stock += Amount;
                        DB.ComponentsDamage.Remove(ComponentDamage);
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
                    var query = DB.Components.AsQueryable();
                    if (CategorySearch.SelectedIndex > 0)
                    { query = query.Where(c => c.CategoryId == ((Category)CategorySearch.SelectedItem).Id); }
                    var components = query.OrderBy(c => c.Name).ToList();
                    components.Insert(0, new Component() { Id = 0, Name = "الكل" });
                    ComponentSearch.ItemsSource = components;
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
                var ComponentDamage = pop.DataContext as ComponentDamage;
                if (ComponentDamage.Id == 0)
                {
                    DB.ComponentsDamage.Add(ComponentDamage);
                    amount = ComponentDamage.Unit == Model.Entities.Units.جرام ? ComponentDamage.Amonut * 0.001m : ComponentDamage.Amonut;
                }
                else
                {
                    if (Unit == Model.Entities.Units.جرام) { amount *= 0.001m; }
                    amount = (ComponentDamage.Unit == Model.Entities.Units.جرام ? ComponentDamage.Amonut * 0.001m : ComponentDamage.Amonut) - amount;
                }
                //DB.Components.Find(Outcome.Component.Id).Stock -= amount;
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
                    Component.ItemsSource = DB.Components.Where(c => c.CategoryId == ((Category)Category.SelectedItem).Id).OrderBy(c => c.Name).ToList();
                else
                    Component.ItemsSource = DB.Components.OrderBy(c => c.Name).ToList();
            }
            catch
            {

            }
        }

    }
}
