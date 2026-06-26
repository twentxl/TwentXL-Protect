using PasswordManager.Components;
using PasswordManager.Helper.Interfaces;
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
    public class GlobalSettings : ASettings, IGlobalSettings
    {
        public static SettingsModel settingsModel = new SettingsModel();
        public static bool isAuth = false;

        public IBackupSettings _backupSettings;

        public GlobalSettings(IBackupSettings backupSettings)
        {
            _backupSettings = backupSettings;

            settingsModel = new SettingsModel();
        }

        public void LoadSettings()
        {
            string directorySettings = Path.GetDirectoryName(filePathSettings)!;
            Directory.CreateDirectory(directorySettings);

            if (!File.Exists(filePathSettings))
                SaveSettings();

            if (File.Exists(filePathAuth))
                isAuth = true;
            else 
                isAuth = false;

            string json = File.ReadAllText(filePathSettings);
            if(json != null)
            {
                SettingsModel? settingsList = JsonSerializer.Deserialize<SettingsModel>(json);
                if(settingsList != null)
                {
                    settingsModel.DarkTheme = settingsList.DarkTheme;
                    settingsModel.BackupPath = settingsList.BackupPath;
                }
            }

            ApplyTheme(settingsModel.DarkTheme);
        }

        public void SaveSettings()
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

        public void CreateBackup()
        {
            _backupSettings.CreateBackup(settingsModel);
        }

        public void LoadBackup()
        {
            _backupSettings.LoadBackup();
        }

        public void ApplyTheme(bool isDark)
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

            SaveSettings();
        }
    }
}
