using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PasswordManager.Helper;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PasswordManager.Pages;

namespace PasswordManager.Components
{
    public partial class Modal_AddData : UserControl
    {
        public Modal_AddData()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance?.HideModal();
        }
        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            string res = Utils.GenerateRandomText(8, 21);
            Password.Text = res.ToString();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FieldValidation(Title) == false || FieldValidation(Password) == false) return;

                DataBlock dataBlock = new DataBlock(Title.Text, Login.Text, Password.Text, Additional.Text);
                MainPage.MainPageInstance?.DataBlockStackPanel.Children.Add(dataBlock);
                MainWindow.Instance?.HideModal();
                MainWindow.Instance?.ShowToast("Succcess!", Colors.Green);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Add password error: " + ex.Message);
                MessageBox.Show("Add password error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool FieldValidation(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                MainWindow.Instance?.ShowToast($"Field '{textBox.Name.ToLower()}' is required", Colors.Red);
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                textBox.BorderBrush = null;
                return true;
            }
        }
    }
}
