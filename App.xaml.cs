using PasswordManager.Helper;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Models;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PasswordManager
{
    public partial class App : Application
    {
        private IJsonSettings? jsonSettings;
        private IKeysSettings? keysSettings;
        private IBackupSettings? backupSettings;
        internal static IDataSettings? dataSettings;
        internal static IGlobalSettings? globalSettings;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                var uri = new Uri("pack://application:,,,/Styles.xaml");
                var resourceDictionary = new ResourceDictionary { Source = uri };
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

                keysSettings = new KeysSettings();
                jsonSettings = new JsonSettings(keysSettings);
                dataSettings = new DataSettings(jsonSettings, keysSettings);
                backupSettings = new BackupSettings(dataSettings);
                globalSettings = new GlobalSettings(backupSettings);

                globalSettings.LoadSettings();

                Window window;
                if (GlobalSettings.isAuth) window = new AuthenticationWindow();
                else window = new MainWindow();
                window.Show();
            }
            catch(Exception ex)
            {
                string getLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if(!string.IsNullOrWhiteSpace(getLocation) || !string.IsNullOrEmpty(getLocation))
                {
                    string logPath = System.IO.Path.Combine(getLocation, "startup_error.log");
                    System.IO.File.WriteAllText(logPath, ex.ToString());
                }
            }
        }
    }

}
