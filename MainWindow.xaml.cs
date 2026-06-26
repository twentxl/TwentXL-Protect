using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Components;
using PasswordManager.Helper;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Models;
using PasswordManager.Pages;
using PasswordManager.Services;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }

        private IDataSettings _dataSettings;
        private IGlobalSettings _globalSettings;

        private readonly MainPage mainPage = App.Services.GetRequiredService<MainPage>();
        private readonly SettingsPage settingsPage = App.Services.GetRequiredService<SettingsPage>();
        private readonly FAQPage faqpage = new FAQPage();

        public MainWindow(IDataSettings dataSettings, IGlobalSettings globalSettings)
        {
            InitializeComponent();
            ToastService.Initialize(MyToast);
            Instance = this;

            _dataSettings = dataSettings;
            _globalSettings = globalSettings;

            MainPageShow();
        }

        protected override void OnClosed(EventArgs e)
        {
            _dataSettings.SaveJson();
            _globalSettings.SaveSettings();
            base.OnClosed(e);
        }

        private void MainPageShow()
        {
            AddPage(mainPage, "Home");
        }

        private void SettingsPageShow()
        {
            AddPage(settingsPage, "Settings");
        }

        private void FAQPageShow()
        {
            AddPage(faqpage, "FAQ");
        }

        private void AddPage(UIElement element, string pageName)
        {
            MainControl.Children.Clear();
            MainControl.Children.Add(element);
            PageName.Content = pageName;
        }

        private void Titlebar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else
                this.WindowState = WindowState.Maximized;
        }

        private void BackupAction_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.IsOpen = true;
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainPageShow();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPageShow();
        }

        private void FAQ_Click(object sender, RoutedEventArgs e)
        {
            FAQPageShow();
        }

        private void CreateBackup_Click(object sender, RoutedEventArgs e)
        {
            _globalSettings.CreateBackup();
        }

        private void LoadBackup_Click(object sender, RoutedEventArgs e)
        {
            _globalSettings.LoadBackup();
        }
    }
}