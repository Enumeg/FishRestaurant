﻿using System;
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
using System.IO;
namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        List<Page> pages;
        List<Window> windows;
        public Main()
        {
            InitializeComponent();
            getpages();         
        }

        private void Buttons_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Control = ((Button)e.Source).Tag;
                if (Control is Page) { frame.Navigate(Control); }
                else if (Control is Window) { ((Window)Control).ShowDialog(); }
            }
            catch
            {

            }
        }
        private void getpages()
        {
            try
            {

                pages = new List<Page>();
                windows = new List<Window>();
                pages.Add(new Components());
                pages.Add(new People(Model.Entities.PersonTypes.Supplier));
                pages.Add(new Purchases(Model.Entities.TransactionTypes.Buy));
                pages.Add(new Purchases(Model.Entities.TransactionTypes.ReBuy));
                //pages.Add(new Transfers(Model.Entities.Transaction_Types.Out));
                //pages.Add(new Transfers(Model.Entities.Transaction_Types.In));
                pages.Add(new Products());
                pages.Add(new People(Model.Entities.PersonTypes.Customer));
                pages.Add(new Sales());                
                pages.Add(new ComponentsDamage());
                pages.Add(new ProductsDamage());
                pages.Add(new Stock());
                pages.Add(new Installments(Model.Entities.PersonTypes.Supplier));
                pages.Add(new Installments(Model.Entities.PersonTypes.Customer));
                pages.Add(new Outcomes());
                foreach (Page p in pages)
                {
                    Button btn = new Button();
                    btn.Style = this.FindResource("Side") as Style;
                    btn.Content = p.Title;
                    btn.Tag = p;
                    Buttons.Children.Add(btn);
                }
             
            }
            catch
            {
                //var controls = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.GetFiles().Where(f => f.Extension == ".xaml" && f.Name != "App.xaml" && f.Name != "Main.xaml").ToList();
                //foreach (var f in controls)
                //{
                //    var Control = Application.LoadComponent(new Uri("/" + f.Name, UriKind.Relative));
                //    if (Control is Page) { pages.Add((Page)Control); }
                //    else if (Control is Window) { windows.Add((Window)Control); }
                //}
            }
        }
    }
}
