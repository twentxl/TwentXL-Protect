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
        GlobalSettings settings = new GlobalSettings();
        SettingsModel settingsModel = new SettingsModel();

        public SettingsPage()
        {
            InitializeComponent();
            DarkThemeValidate();
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            DarkThemeValidate();
            settings.ApplyTheme(settingsModel.DarkTheme);
        }

        private void DarkThemeValidate()
        {
            if (settingsModel.DarkTheme)
            {
                settingsModel.DarkTheme = false;
                DarkThemeButton.Content = "On";
            }
            else
            {
                settingsModel.DarkTheme = true;
                DarkThemeButton.Content = "Off";
            }
        }
    }
}
