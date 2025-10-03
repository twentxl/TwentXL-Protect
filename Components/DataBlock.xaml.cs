using PasswordManager.Pages;
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
            MainWindow.Instance?.ShowModal(modal_editData);
        }

        private void DeleteBlock_Click(object sender, RoutedEventArgs e)
        {
            MainPage.MainPageInstance?.DataBlockStackPanel.Children.Remove(this);
        }
    }
}
