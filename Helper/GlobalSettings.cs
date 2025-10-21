using PasswordManager.Components;
using PasswordManager.Models;
using PasswordManager.Pages;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace PasswordManager.Helper
{
    public class GlobalSettings
    {
        private const string LightThemePath = "pack://application:,,,/Styles.xaml";
        private const string DarkThemePath = "pack://application:,,,/Styles_Dark.xaml";
        private static string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TwentXL Protect", "settings.json");

        public static SettingsModel settingsModel = new SettingsModel();

        public static void LoadSettings()
        {
            string directory = Path.GetDirectoryName(filePath)!;
            Directory.CreateDirectory(directory);

            if (!File.Exists(filePath))
                SaveSettings();

            string json = File.ReadAllText(filePath);
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
                File.WriteAllText(filePath, json);
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

                File.WriteAllText(Path.Combine(settingsModel.BackupPath, "credentialds_backup.json"), json);
                ToastService.Show($"Backup created '{settingsModel.BackupPath}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Credentials backup error: " + ex.Message);
                ToastService.Show("Credentials backup error", Colors.Red);
            }
        }
    }
}
