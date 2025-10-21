using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using PasswordManager.Helper;
using PasswordManager.Models;

namespace PasswordManager.Pages
{
    public partial class SettingsPage : UserControl
    {
        private SettingsModel settingsModel = GlobalSettings.settingsModel;
        public SettingsPage()
        {
            InitializeComponent();
            UpdateSettings();
        }

        private void BackupPathEdit_Click(object sender, RoutedEventArgs e)
        {
            string path = Utils.GetPathDir();
            settingsModel.BackupPath = path;
            UpdateSettings();
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            if (settingsModel.DarkTheme)
                settingsModel.DarkTheme = false;
            else
                settingsModel.DarkTheme = true;

            UpdateSettings();
            GlobalSettings.ApplyTheme(settingsModel.DarkTheme);
        }

        private void UpdateSettings()
        {
            if (settingsModel.DarkTheme)
                DarkThemeButton.Content = "Off";
            else
                DarkThemeButton.Content = "On";

            BackupPath.Content = settingsModel.BackupPath;
        }
    }
}
