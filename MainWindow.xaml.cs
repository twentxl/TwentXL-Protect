using PasswordManager.Components;
using PasswordManager.Helper;
using PasswordManager.Models;
using PasswordManager.Pages;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PasswordManager.Services;

namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            ToastService.Initialize(MyToast);
            Instance = this;
            MainPageShow();
        }

        protected override void OnClosed(EventArgs e)
        {
            DataSettings.SaveJson();
            GlobalSettings.SaveSettings();
        }

        private void MainPageShow()
        {
            MainPage mainPage = new MainPage();
            AddPage(mainPage);
        }

        private void SettingsPageShow()
        {
            SettingsPage settingsPage = new SettingsPage();
            AddPage(settingsPage);
        }

        private void FAQPageShow()
        {
            FAQPage faqpage = new FAQPage();
            AddPage(faqpage);
        }

        private void AddPage(UIElement element)
        {
            MainControl.Children.Clear();
            MainControl.Children.Add(element);
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
            Button button = sender as Button;
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
            GlobalSettings.CreateBackup();
        }

        private void LoadBackup_Click(object sender, RoutedEventArgs e)
        {
            GlobalSettings.LoadBackup();
        }
    }
}