using PasswordManager.Services;
using System;
using System.Collections.Generic;
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

namespace PasswordManager.Components
{
    public partial class DataBlockContent : UserControl
    {
        public static DataBlockContent? Instance;

        public DataBlockContent(string title, string login, string password, string additional, string createdDate)
        {
            InitializeComponent();
            Instance = this;

            this.Title_Content.Content = title;
            this.Login_Content.Text = login;
            this.Password_Content.Text = password;
            this.Additional_Content.Text = additional;
            this.CreatedDate_Content.Content = createdDate;

            if (string.IsNullOrEmpty(title)) Title_Content.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(login)) Login_Block.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(password)) Password_Block.Visibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(additional)) Additional_Block.Visibility = Visibility.Collapsed;
        }

        private void Actions_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.IsOpen = true;
        }

        private void CopyText_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            byte number = Byte.Parse(button?.Tag.ToString());
            switch (number)
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
