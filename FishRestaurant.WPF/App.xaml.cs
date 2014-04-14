using System.Configuration;
using System.Windows;
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
            var m = new Main();
            m.ShowDialog();

        }
    }
}
