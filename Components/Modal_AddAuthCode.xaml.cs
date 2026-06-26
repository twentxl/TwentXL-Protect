using PasswordManager.Helper;
using PasswordManager.Pages;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Security.Cryptography;
using PasswordManager.Helper.Interfaces;

namespace PasswordManager.Components
{
    public partial class Modal_AddAuthCode : UserControl
    {
        private IGlobalSettings _globalSettings;

        public Modal_AddAuthCode(IGlobalSettings globalSettings)
        {
            InitializeComponent();

            _globalSettings = globalSettings;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ModalService.HideModal();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(Code.Text.Length < 4)
            {
                ToastService.Show("The length must be at least 4 characters.", Colors.Red);
                return;
            }

            using (Aes aes = Aes.Create())
            {
                aes.Key = Crypto.key;
                aes.IV = Crypto.iv;

                string file = Crypto.Encrypt(Code.Text, Crypto.key, Crypto.iv);
                File.WriteAllText(ASettings.public_filePathAuth, file);
            }

            ModalService.HideModal();
            ToastService.Show("Authentication code was added", Colors.Green);
            _globalSettings.SaveSettings();
            _globalSettings.LoadSettings();
            SettingsPage.SettingsPageInstance?.UpdateSettings();
        }
    }
}
