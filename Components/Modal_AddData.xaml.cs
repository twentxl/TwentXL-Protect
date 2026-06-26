using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using PasswordManager.Helper;
using System.Windows.Media;
using PasswordManager.Pages;
using PasswordManager.Services;
using PasswordManager.Helper.Interfaces;

namespace PasswordManager.Components
{
    public partial class Modal_AddData : UserControl
    {
        private IDataSettings _dataSettings;

        public Modal_AddData(IDataSettings dataSettings)
        {
            InitializeComponent();

            _dataSettings = dataSettings;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ModalService.HideModal();
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
                if (FieldValidation(Title) == false || FieldValidation(Password) == false || FieldValidation(Login) == false) return;

                DateTime createdDate = DateTime.Now;
                DataBlock dataBlock = new DataBlock(Title.Text, Login.Text, Password.Text, Additional.Text, createdDate.ToString());
                MainPage.MainPageInstance?.DataBlockStackPanel.Children.Add(dataBlock);
                ModalService.HideModal();
                ToastService.Show("Password data was added", Colors.Green);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Add password error: " + ex.Message);
                MessageBox.Show("Add password error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _dataSettings.SaveJson();
            }
        }

        private bool FieldValidation(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                ToastService.Show($"Field '{textBox.Name.ToLower()}' is required", Colors.Red);
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
                return false;
            }
            else
            {
                textBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#757575"));
                return true;
            }
        }
    }
}
