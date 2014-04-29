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
    /// Interaction logic for Outcomes.xaml
    /// </summary>
    public partial class Outcomes : Page
    {

        FrContext DB;
        OutcomeTypes outcome_types;
        public Outcomes()
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
                var Types = DB.OutcomeTypes.OrderBy(c => c.Name).ToList();
                Types.Insert(0, new OutcomeType() { Id = 0, Name = "الكل" });                
                OutcomeTypeSearch.ItemsSource = Types;
                Type.ItemsSource = DB.OutcomeTypes.OrderBy(c => c.Name).ToList();
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
                var query = DB.Outcomes.Where(c => DbFunctions.TruncateTime(c.Date) >= DbFunctions.TruncateTime(SearchFromDateDTP.Value.Value) &&
                   DbFunctions.TruncateTime(c.Date) <= DbFunctions.TruncateTime(SearchToDateDTP.Value.Value));
                if (OutcomeTypeSearch.SelectedIndex > 0) { query = query.Where(c => c.OutcomeTypeId == ((OutcomeType)OutcomeTypeSearch.SelectedItem).Id); }
                OutcomeDG.ItemsSource = query.OrderBy(c => c.Date).ToList();
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
                    pop.DataContext = new Outcome() { Date = DateTime.Now };
                }
                else
                {                   
                    pop.DataContext = OutcomeDG.SelectedItem as Outcome;                    
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
                if (OutcomeDG.SelectedIndex != -1)
                {

                    if (Message.Show("هل تريد حذف هذا المصروف", MessageBoxButton.YesNoCancel, 5) == MessageBoxResult.Yes)
                    {                      
                        DB.Outcomes.Remove((Outcome)OutcomeDG.SelectedItem);
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
                var Outcome = pop.DataContext as Outcome;
                if (Outcome.Id == 0)
                {
                    DB.Outcomes.Add(Outcome);                   
                }               
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
                OutcomeTypes c = new OutcomeTypes();
                c.ShowDialog();
                Initialize();
            }
            catch
            {

            }
        }





    }
}
