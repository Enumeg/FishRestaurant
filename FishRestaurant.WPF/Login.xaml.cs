﻿using FishRestaurant.Model.Entities;
using Source;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        FrContext DB;


        public Login()
        {
            InitializeComponent();
            DB = new FrContext();
            var user = DB.Users.AsQueryable().ToList();
        }

        private void Log_In_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Check_Login();

            }
            catch
            {


            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            try
            {



            }
            catch
            {


            }
        }


        private void Check_Login()
        {
            try
            {
                User_name_TB.Text.Trim();
                Password_TB.Password.GetHashCode();
                Window w;
                var user = DB.Users.FirstOrDefault(c => c.Name.Equals(User_name_TB.Text.Trim()));
                if (user != null)
                {
                    // user check Password with Db Password
                    if (user.Password.GetHashCode().Equals(Password_TB.Password.GetHashCode()))
                    {
                        // check Group if Admin or Cashier
                        App.User = user;
                        if (user.Group == 0)
                        {
                            // Cashier Pager
                            w = new Reception();
                        }
                        else
                        {

                            // Admin Page
                            w = new Management();

                        }
                        this.Hide();
                        w.ShowDialog();
                    }
                    else
                    {
                        Message.Show("كلمة المرور غير صحيحة", MessageBoxButton.OK, 10);

                    }

                }
                else
                {
                    // Not user
                    Message.Show("إسم المستخدم غير صحيح", MessageBoxButton.OK, 10);


                }

            }
            catch
            {

            }
        }


        ///////////////////////////////////////////////////
    }
}
