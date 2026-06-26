using PasswordManager.Helper;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Models;
using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Components;
using PasswordManager.Pages;

namespace PasswordManager
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        public static IServiceProvider Services => ((App)Current)._serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            RegisterServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        private void RegisterServices(ServiceCollection services)
        {
            services.AddSingleton<IJsonSettings, JsonSettings>();
            services.AddSingleton<IKeysSettings, KeysSettings>();
            services.AddSingleton<IBackupSettings, BackupSettings>();
            services.AddSingleton<IDataSettings, DataSettings>();
            services.AddSingleton<IGlobalSettings, GlobalSettings>();

            services.AddTransient<MainWindow>();
            services.AddTransient<AuthenticationWindow>();

            services.AddTransient<MainPage>();
            services.AddTransient<SettingsPage>();

            services.AddTransient<Func<DataBlock, DeleteDialog>>(sp => dataBlock =>
                new DeleteDialog(dataBlock, sp.GetRequiredService<IDataSettings>())
            );
            services.AddTransient<Modal_AddAuthCode>();
            services.AddTransient<Modal_AddData>();
            services.AddTransient<Func<DataBlock, string, string, string, string, Modal_EditData>>(sp =>
                (dataBlock, title, login, password, additional) =>
                    new Modal_EditData(dataBlock, sp.GetRequiredService<IDataSettings>(), title, login, password, additional)
            );
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                var uri = new Uri("pack://application:,,,/Styles.xaml");
                var resourceDictionary = new ResourceDictionary { Source = uri };
                Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);

                var globalSettings = _serviceProvider!.GetRequiredService<IGlobalSettings>();
                globalSettings.LoadSettings();

                if (_serviceProvider != null)
                {
                    Window window = GlobalSettings.isAuth ? _serviceProvider.GetRequiredService<AuthenticationWindow>() : _serviceProvider.GetRequiredService<MainWindow>();
                    window.Show();
                }
                else
                    throw new Exception("Set provider error");
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
