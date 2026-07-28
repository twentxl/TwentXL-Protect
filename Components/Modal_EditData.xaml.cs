using PasswordManager.Helper;
using PasswordManager.Helper.Interfaces;
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
        private DataBlockContent? dataBlockContent = DataBlockContent.Instance;
        private DataBlock _dataBlock;
        private IDataSettings _dataSettings;

        public Modal_EditData(DataBlock dataBlock, IDataSettings dataSettings, string title, string login, string password, string additional)
        {
            InitializeComponent();

            _dataBlock = dataBlock;
            _dataSettings = dataSettings;

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
            => ModalService.HideModal();

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FieldValidation(Title) == false || FieldValidation(Password) == false || FieldValidation(Login) == false) return;
                string title = Title.Text;
                string login = Login.Text;
                string password = Password.Text;
                string additional = Additional.Text;

                FillAndCheckValue(title, login, password, additional);

                ModalService.HideModal();
                ToastService.Show("Password data was edited", Colors.Green);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Edit Data error: " + ex.Message);
                MessageBox.Show("Edit Data error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _dataSettings.SaveJson();
            }
        }

        private void FillAndCheckValue(string title, string login, string password, string additional)
        {
            void Fill(dynamic x, Action<dynamic, string> setLogin)
            {
                x.Title_Content.Content = title;
                setLogin(x, login);
                x.Password_Content.Text = password;
                x.Additional_Content.Text = additional;
            }

            Fill(_dataBlock, (x, v) => x.Login_Content.Content = v);

            if(dataBlockContent != null)
            {
                Fill(dataBlockContent, (x, v) => x.Login_Content.Text = v);

                if (string.IsNullOrEmpty(title)) dataBlockContent.Title_Content.Visibility = Visibility.Collapsed;
                else dataBlockContent.Title_Content.Visibility = Visibility.Visible;

                if (string.IsNullOrEmpty(login)) dataBlockContent.Login_Block.Visibility = Visibility.Collapsed;
                else dataBlockContent.Login_Block.Visibility = Visibility.Visible;

                if (string.IsNullOrEmpty(password)) dataBlockContent.Password_Block.Visibility = Visibility.Collapsed;
                else dataBlockContent.Password_Block.Visibility = Visibility.Visible;

                if (string.IsNullOrEmpty(additional)) dataBlockContent.Additional_Block.Visibility = Visibility.Collapsed;
                else dataBlockContent.Additional_Block.Visibility = Visibility.Visible;
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
                textBox.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#757575")); ;
                return true;
            }
        }
    }
}
