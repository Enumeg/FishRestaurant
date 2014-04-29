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
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Window
    {


        FrContext DB;
        

        
        public Users()
        {

            InitializeComponent();
            
            DB = new FrContext();

            Group_CB.ItemsSource = Enum.GetValues(typeof(Groups));
       

            FillLB();



        }

        private void FillLB()
        {
            try
            {
                if (LB.IsEnabled)
                {
                  

                   var query = DB.Users.Where(c => c.Name.StartsWith(Name_TB.Text));
                   if (Group_CB.SelectedIndex > -1) { query = query.Where(c => c.Group == (Groups)Group_CB.SelectedItem); }
                   LB.ItemsSource = query.OrderBy(c => c.Group).ToList();
                }
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
                    pop.DataContext = new User();
                }
                else
                {
                    pop.DataContext = LB.SelectedItem as User;
                }

                pop.IsOpen = true;

            }
            catch
            {

            }
        }

     
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {

              if(Password_TB.Text.Equals(Re_Password_TB.Text))
              {

                    var User = pop.DataContext as User;
                    User.Password = Password_TB.Text.GetHashCode();
                    if (User.Id == 0) 
                    {
                        DB.Users.Add(User); 
                    }
                
                    DB.SaveChanges();

                    if ((bool)New.IsChecked)
                    {
                        pop.DataContext = new User();
                    }
                    else
                    {
                        pop.IsOpen = false;
                    }
                
                    Confirm.Check(true);
                  
                    FillLB();

            }
              
            }
            catch
            {
                Confirm.Check(false);
            }
        }

        private void EditPanel_Delete(object sender, EventArgs e)
        {

            try
            {
                if (Message.Show("هل تريد حذف هذا المستخدم", MessageBoxButton.YesNoCancel, 10) == MessageBoxResult.Yes)
                {
                    DB.Users.Remove((User)LB.SelectedItem);
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GroupCB.ItemsSource = Enum.GetValues(typeof(Groups));
                FillLB();
            }
            catch 
            {
                
                
            }
        }



        //////////////////////////////////////////////

    }
}
