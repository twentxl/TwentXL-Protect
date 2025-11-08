using PasswordManager.Components;
using PasswordManager.Models;
using PasswordManager.Pages;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace PasswordManager.Helper
{
    public class GlobalSettings
    {
        private const string LightThemePath = "pack://application:,,,/Styles.xaml";
        private const string DarkThemePath = "pack://application:,,,/Styles_Dark.xaml";
        private static string filePathSettings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TwentXL Protect", "settings.json");
        public static string filePathAuth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TwentXL Protect", "authcode.dat");

        public static SettingsModel settingsModel = new SettingsModel();
        public static bool isAuth = false;

        public static void LoadSettings()
        {
            string directorySettings = Path.GetDirectoryName(filePathSettings)!;
            Directory.CreateDirectory(directorySettings);

            if (!File.Exists(filePathSettings))
                SaveSettings();

            if (File.Exists(filePathAuth))
                isAuth = true;
            else isAuth = false;

                string json = File.ReadAllText(filePathSettings);
            if(json != null)
            {
                SettingsModel settingsList = JsonSerializer.Deserialize<SettingsModel>(json);
                settingsModel.DarkTheme = settingsList.DarkTheme;
                settingsModel.BackupPath = settingsList.BackupPath;
            }

            ApplyTheme(settingsModel.DarkTheme);
        }

        public static void SaveSettings()
        {
            try
            {
                string json = JsonSerializer.Serialize(settingsModel);
                File.WriteAllText(filePathSettings, json);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Save settings error: " + ex.Message);
                MessageBox.Show("Save settings error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ApplyTheme(bool isDark)
        {
            string themePath;

            if (isDark) themePath = DarkThemePath;
            else themePath = LightThemePath;

            var app = Application.Current;
            var dict = new ResourceDictionary { Source = new Uri(themePath) };

            var oldTheme = app.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source?.OriginalString.Contains("Styles_") == true);
            if (oldTheme != null)
                app.Resources.MergedDictionaries.Remove(oldTheme);

            app.Resources.MergedDictionaries.Add(dict);
        }

        public static void CreateBackup()
        {
            try
            {
                List<PasswordModel> passwordList = new List<PasswordModel>();
                var dataBlockPanel = MainPage.MainPageInstance?.DataBlockStackPanel.Children;

                foreach (var item in dataBlockPanel)
                {
                    if (item is DataBlock dataBlock)
                    {
                        passwordList.Add(new PasswordModel { Title = dataBlock.Title_Content.Content.ToString(), Login = dataBlock.Login_Content.Text, Password = dataBlock.Password_Content.Text, Additional = dataBlock.Additional_Content.Text });
                    }
                }

                string json = JsonSerializer.Serialize(passwordList);

                File.WriteAllText(Path.Combine(settingsModel.BackupPath, "credentialds_backup.dat"), json);
                ToastService.Show($"Backup created '{settingsModel.BackupPath}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Credentials backup error: " + ex.Message);
                ToastService.Show("Credentials backup error", Colors.Red);
            }
        }

        public static void LoadBackup()
        {
            try
            {
                string filename = Utils.SelectFile();
                if (filename != null)
                {
                    string file = File.ReadAllText(filename);
                    if (file != null)
                    {
                        using(Aes aes = Aes.Create())
                        {
                            aes.Key = Crypto.key;
                            aes.IV = Crypto.iv;

                            file = Crypto.Encrypt(file, Crypto.key, Crypto.iv);
                            File.WriteAllText(DataSettings.filePath, file);
                        }
                        DataSettings.LoadJson();
                        ToastService.Show("Backup was loaded!", Colors.Green);
                    }
                }
                else ToastService.Show("File is not selected", Colors.Red);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Load backup error: " + ex.Message);
                ToastService.Show("Load backup error", Colors.Red);
            }
        }
    }
}
