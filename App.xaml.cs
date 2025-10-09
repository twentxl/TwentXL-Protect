using System.Configuration;
using System.Data;
using System.Windows;

namespace PasswordManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var uri = new Uri("pack://application:,,,/Styles.xaml");
            var resourceDictionary = new ResourceDictionary { Source = uri };
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }

}
