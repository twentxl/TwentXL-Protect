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
using PasswordManager.Helper.Interfaces;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace PasswordManager
{
    public partial class AuthenticationWindow : Window
    {
        private IDataSettings _dataSettings; 

        public AuthenticationWindow(IDataSettings dataSettings)
        {
            InitializeComponent();
            ErrorMessage.Visibility = Visibility.Collapsed;

            _dataSettings = dataSettings;
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
                string code = File.ReadAllText(ASettings.public_filePathAuth);

                using (Aes aes = Aes.Create())
                {
                    _dataSettings.LoadKeys();
                    aes.Key = Crypto.key;
                    aes.IV = Crypto.iv;

                    string codeDecrypt = Crypto.Decrypt(code, Crypto.key, Crypto.iv);

                    if (Code.Text == codeDecrypt)
                    {
                        var mainWindow = App.Services?.GetRequiredService<MainWindow>();
                        mainWindow?.Show();
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
                Debug.Write("Decrypt error: the decryption keys are missing: " + ex.Message);
            }
        }

        private void DestroyClick(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("\"Destroy all\" will lead to a complete cleanup of your data, including your passwords and authorization code. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(message == MessageBoxResult.Yes)
            {
                File.Delete(ASettings.public_filePathAuth);
                _dataSettings.DestroyAll();
                this.Close();
            }
        }
    }
}
