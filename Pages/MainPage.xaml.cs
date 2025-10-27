using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PasswordManager.Components;
using PasswordManager.Helper;
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
            DataSettings.LoadJson();
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
                string query = SearchBox.Text?.Trim();

                if(string.IsNullOrEmpty(query))
                {
                    ToastService.Show("This field is empty", Colors.Orange);
                    return;
                }

                foreach (var child in DataBlockStackPanel.Children)
                {
                    if (child is DataBlock dataBlock)
                    {
                        var titleLabel = FindChild<Label>(dataBlock, "Title_Content");
                        if (titleLabel?.Content?.ToString() != query)
                        {
                            dataBlock.Visibility = Visibility.Collapsed;
                        }
                    }
                }

                SearchButton.Visibility = Visibility.Collapsed;
                SearchCancelButton.Visibility = Visibility.Visible;
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
