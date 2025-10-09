using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Windows;
using System.Text.Json;

namespace PasswordManager.Helper
{
    public class GlobalSettings
    {
        private const string LightThemePath = "pack://application:,,,/Styles.xaml";
        private const string DarkThemePath = "pack://application:,,,/Styles_Dark.xaml";
        private string filePath;

        public static SettingsModel settingsModel = new SettingsModel();

        public GlobalSettings()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filePath = Path.Combine(localAppData, "TwentXL Protect", "settings.json");
            LoadSettings();
            ApplyTheme(settingsModel.DarkTheme);
        }

        private void LoadSettings()
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
            }
        }

        public void SaveSettings()
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
    }
}
