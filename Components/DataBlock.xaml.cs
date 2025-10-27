using PasswordManager.Helper;
using PasswordManager.Pages;
using PasswordManager.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PasswordManager.Components
{
    public partial class DataBlock : UserControl
    {
        public DataBlock(string title, string login, string password, string additional)
        {
            InitializeComponent();

            this.Title_Content.Content = title;
            this.Login_Content.Text = login;
            this.Password_Content.Text = password;
            this.Additional_Content.Text = additional;

            if (string.IsNullOrEmpty(title)) Title_Content.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(login)) Login_Block.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(password)) Password_Block.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(additional)) Additional_Block.Visibility = Visibility.Collapsed;
        }

        private void Actions_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.IsOpen = true;
        }

        private void EditBlock_Click(object sender, RoutedEventArgs e)
        {
            Modal_EditData modal_editData = new Modal_EditData(this, Title_Content.Content.ToString(), Login_Content.Text, Password_Content.Text, Additional_Content.Text);
            ModalService.ShowModal(modal_editData);
        }

        private void DeleteBlock_Click(object sender, RoutedEventArgs e)
        {
            MainPage.MainPageInstance?.DataBlockStackPanel.Children.Remove(this);
            ToastService.Show("Block was deleted", Colors.Orange);
            DataSettings.SaveJson();
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            byte number = Byte.Parse(button?.Tag.ToString());
            switch(number)
            {
                case 1:
                    Clipboard.SetText(Login_Content.Text);
                    ToastService.Show("Copied", Colors.Green);
                    break;
                case 2:
                    Clipboard.SetText(Password_Content.Text);
                    ToastService.Show("Copied", Colors.Green);
                    break;
                case 3:
                    Clipboard.SetText(Additional_Content.Text);
                    ToastService.Show("Copied", Colors.Green);
                    break;
                default:
                    ToastService.Show("Copy error", Colors.Red);
                    break;
            }
        }
    }
}