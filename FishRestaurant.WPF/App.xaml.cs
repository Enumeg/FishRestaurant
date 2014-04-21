
using System.Data.Entity;
using System.Windows;
using FishRestaurant.Model.Entities;
using FishRestaurant.Model.Migrations;
using Source;

namespace FishRestaurant.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {      
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FRContext, Configuration>());
            var m = new Main();
            m.ShowDialog();
        }        
    }
}
