using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.IO;
using PasswordManager.Components;
using PasswordManager.Helper;
using PasswordManager.Models;
using PasswordManager.Services;

namespace PasswordManager.Pages
{
    public partial class SettingsPage : UserControl
    {
        private SettingsModel settingsModel = GlobalSettings.settingsModel;
        public static SettingsPage SettingsPageInstance { get; private set; }
        public SettingsPage()
        {
            SettingsPageInstance = this;
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

        private void AddAuthCode_Click(object sender, RoutedEventArgs e)
        {
            Modal_AddAuthCode modalAddAuthCode = new Modal_AddAuthCode();
            ModalService.ShowModal(modalAddAuthCode);
        }

        private void RemoveAuthCode_Click(object sender, RoutedEventArgs e)
        {
            File.Delete(GlobalSettings.filePathAuth);
            GlobalSettings.LoadSettings();
            UpdateSettings();
        }

        public void UpdateSettings()
        {
            if (settingsModel.DarkTheme)
                DarkThemeButton.Content = "Off";
            else
                DarkThemeButton.Content = "On";

            BackupPath.Content = settingsModel.BackupPath;

            if(GlobalSettings.isAuth)
            {
                AddCodeButton.Visibility = Visibility.Collapsed;
                RemoveCodeButton.Visibility = AuthenticationCode.Visibility = Visibility.Visible;

                string code = File.ReadAllText(GlobalSettings.filePathAuth);
                AuthenticationCode.Content = code;
            }
            else
            {
                AddCodeButton.Visibility = Visibility.Visible;
                RemoveCodeButton.Visibility = AuthenticationCode.Visibility = Visibility.Collapsed;
            }
        }
    }
}
