using PasswordManager.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;
using System.Windows.Shapes;

namespace PasswordManager
{
    public partial class AuthenticationWindow : Window
    {
        public AuthenticationWindow()
        {
            InitializeComponent();
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void Titlebar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string code = File.ReadAllText(GlobalSettings.filePathAuth);

                using (Aes aes = Aes.Create())
                {
                    DataSettings.LoadKeys();
                    aes.Key = Crypto.key;
                    aes.IV = Crypto.iv;

                    string codeDecrypt = Crypto.Decrypt(code, Crypto.key, Crypto.iv);

                    if (Code.Text == codeDecrypt)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        ErrorMessage.Visibility = Visibility.Visible;
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Decrypt error: the decryption keys are missing", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
