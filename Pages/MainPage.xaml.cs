using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordManager.Components;
using PasswordManager.Helper;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Services;

namespace PasswordManager.Pages
{
    public partial class MainPage : UserControl
    {
        public static MainPage MainPageInstance { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            MainPageInstance = this;

            App.dataSettings.LoadJson();
        }

        private void AddPassword_Click(object sender, RoutedEventArgs e)
        {
            Modal_AddData modal_addData = new Modal_AddData();
            ModalService.ShowModal(modal_addData);
        }

        private void SearchCancelButton_Click(object sender, RoutedEventArgs e)
        {
            SearchButton.Visibility = Visibility.Visible;
            SearchCancelButton.Visibility = Visibility.Collapsed;

            foreach (var child in DataBlockStackPanel.Children)
            {
                if (child is DataBlock dataBlock)
                {
                    dataBlock.Visibility = Visibility.Visible;
                }
            }

            SearchBox.Clear();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var query = SearchBox.Text?.Trim();

                if (string.IsNullOrEmpty(query))
                {
                    SearchButton.Visibility = Visibility.Visible;
                    SearchCancelButton.Visibility = Visibility.Collapsed;

                    foreach (var child in DataBlockStackPanel.Children)
                    {
                        if (child is DataBlock dataBlock)
                        {
                            dataBlock.Visibility = Visibility.Visible;
                        }
                    }

                    return;
                }

                SearchButton.Visibility = Visibility.Collapsed;
                SearchCancelButton.Visibility = Visibility.Visible;

                foreach (var child in DataBlockStackPanel.Children)
                {
                    if (child is DataBlock dataBlock)
                    {
                        var text = dataBlock.Title_Content.Content.ToString() ?? string.Empty;
                        bool isMatch = text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0;

                        dataBlock.Visibility = isMatch ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Search error: " + ex.Message);
                ToastService.Show("Search Error. Try again", Colors.Red);
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T tChild && (string.IsNullOrEmpty(childName) || (child is FrameworkElement fe && fe.Name == childName)))
                {
                    return tChild;
                }

                var found = FindChild<T>(child, childName);
                if (found != null) return found;
            }
            return null;
        }
    }
}
