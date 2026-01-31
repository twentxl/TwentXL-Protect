using PasswordManager.Helper;
using PasswordManager.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PasswordManager
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                var uri = new Uri("pack://application:,,,/Styles.xaml");
                var resourceDictionary = new ResourceDictionary { Source = uri };
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
                GlobalSettings.LoadSettings();

                Window window;
                if (GlobalSettings.isAuth) window = new AuthenticationWindow();
                else window = new MainWindow();
                window.Show();
            }
            catch(Exception ex)
            {
                string logPath = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location
                ),
                "startup_error.log"
            );
                System.IO.File.WriteAllText(logPath, ex.ToString());
            }
        }
    }

}
