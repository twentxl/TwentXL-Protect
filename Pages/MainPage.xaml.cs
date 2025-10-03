using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PasswordManager.Components;
using PasswordManager;

namespace PasswordManager.Pages
{
    public partial class MainPage : UserControl
    {
        public static MainPage MainPageInstance { get; private set; }

        public MainPage()
        {
            InitializeComponent();
            MainPageInstance = this;
        }

        private void AddPassword_Click(object sender, RoutedEventArgs e)
        {
            Modal_AddData modal_addData = new Modal_AddData();
            MainWindow.Instance?.ShowModal(modal_addData);
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
                string query = SearchBox.Text?.ToLower().Trim();
                bool isSearched = false;

                if(string.IsNullOrEmpty(query))
                {
                    MainWindow.Instance?.ShowToast("This field is empty", Colors.Yellow);
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
                            isSearched = true;
                        }
                    }
                }

                if (isSearched)
                {
                    SearchButton.Visibility = Visibility.Collapsed;
                    SearchCancelButton.Visibility = Visibility.Visible;
                }
                else
                {
                    SearchButton.Visibility = Visibility.Visible;
                    SearchCancelButton.Visibility = Visibility.Collapsed;
                    MainWindow.Instance?.ShowToast($"'{query}' is not found", Colors.Red);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Search error: " + ex.Message);
                MessageBox.Show("Search Error. Try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
