
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
        public static User User { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            User = new Model.Entities.User() { Group = Groups.Admin };
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FrContext, Configuration>());            
            var m = new Management();
            
            m.ShowDialog();
        }        
    }
}
