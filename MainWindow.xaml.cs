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
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private GlobalSettings globalSettings = new GlobalSettings();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MainPageShow();
        }

        protected override void OnClosed(EventArgs e)
        {
            DataSettings dataSettings = new DataSettings();
            dataSettings.SaveJson();
            globalSettings.SaveSettings();
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

        private void Action_Click(object sender, RoutedEventArgs e)
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

        public async void ShowToast(string message, Color? color = null)
        {
            ToastMessage.Content = message;
            if (color != null) ToastMessage.Foreground = new SolidColorBrush((Color)color);
            ToastPanel.Visibility = Visibility.Visible;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            ToastPanel.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            await Task.Delay(2000);

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            ToastPanel.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            await Task.Delay(300);
            ToastPanel.Visibility = Visibility.Collapsed;
        }

        public void ShowModal(UIElement element)
        {
            ModalDialog modalDialog = new ModalDialog(element);
            ModalDialog_Area.Children.Add(modalDialog);
        }

        public void HideModal()
        {
            ModalDialog_Area.Children.Clear();
        }
    }
}