using PasswordManager.Helper;
using PasswordManager.Pages;
using PasswordManager.Services;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PasswordManager.Components
{
    public partial class Modal_EditData : UserControl
    {
        private DataBlock dataBlock;
        public Modal_EditData(DataBlock _dataBlock, string title, string login, string password, string additional)
        {
            InitializeComponent();
            dataBlock = _dataBlock;
            Title.Text = title;
            Login.Text = login;
            Password.Text = password;
            Additional.Text = additional;
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            string res = Utils.GenerateRandomText(8, 21);
            Password.Text = res.ToString();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ModalService.HideModal();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FieldValidation(Title) == false || FieldValidation(Password) == false) return;
                string title = Title.Text;
                string login = Login.Text;
                string password = Password.Text;
                string additional = Additional.Text;

                FillAndCheckValue(title, login, password, additional);

                ModalService.HideModal();
                ToastService.Show("Block was edited", Colors.Green);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Edit Data error: " + ex.Message);
                MessageBox.Show("Edit Data error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                DataSettings.SaveJson();
            }
        }

        private void FillAndCheckValue(string title, string login, string password, string additional)
        {
            dataBlock.Title_Content.Content = title;
            dataBlock.Login_Content.Text = login;
            dataBlock.Password_Content.Text = password;
            dataBlock.Additional_Content.Text = additional;

            if (string.IsNullOrEmpty(title)) dataBlock.Title_Content.Visibility = Visibility.Collapsed;
            else dataBlock.Title_Content.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(login)) dataBlock.Login_Block.Visibility = Visibility.Collapsed;
            else dataBlock.Login_Block.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(password)) dataBlock.Password_Block.Visibility = Visibility.Collapsed;
            else dataBlock.Password_Block.Visibility = Visibility.Visible;

            if (string.IsNullOrEmpty(additional)) dataBlock.Additional_Block.Visibility = Visibility.Collapsed;
            else dataBlock.Additional_Block.Visibility = Visibility.Visible;

            // I understand that the code can be written easier, but I'm too lazy.
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
                textBox.BorderBrush = null;
                return true;
            }
        }
    }
}
