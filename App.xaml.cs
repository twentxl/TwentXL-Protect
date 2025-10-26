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

                // Показываем окно (если возможно)
                MessageBox.Show($"Ошибка запуска:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
