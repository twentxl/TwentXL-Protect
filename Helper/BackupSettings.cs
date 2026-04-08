using PasswordManager.Components;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Models;
using PasswordManager.Pages;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PasswordManager.Helper
{
    internal class BackupSettings : ASettings, IBackupSettings
    {
        private IDataSettings _dataSettings;

        public BackupSettings(IDataSettings dataSettings)
        {
            _dataSettings = dataSettings;
        }

        public void CreateBackup(SettingsModel settingsModel)
        {
            try
            {
                List<PasswordModel> passwordList = new List<PasswordModel>();
                var dataBlockPanel = MainPage.MainPageInstance?.DataBlockStackPanel.Children;

                if(dataBlockPanel != null)
                {
                    foreach (var item in dataBlockPanel)
                    {
                        if (item is DataBlock dataBlock)
                        {
                            passwordList.Add(new PasswordModel
                            {
                                Title = dataBlock.Title_Content.Content.ToString(),
                                Login = dataBlock.Login_Content.Content.ToString(),
                                Password = dataBlock.Password_Content.Text,
                                Additional = dataBlock.Additional_Content.Text,
                                CreatedDate = dataBlock.CreatedDate_Content.Text
                            });
                        }
                    }
                }

                string json = JsonSerializer.Serialize(passwordList);

                File.WriteAllText(Path.Combine(settingsModel.BackupPath, "credentialds_backup.dat"), json);
                ToastService.Show($"Backup created '{settingsModel.BackupPath}'");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Credentials backup error: " + ex.Message);
                ToastService.Show("Credentials backup error", Colors.Red);
            }
        }

        public void LoadBackup()
        {
            try
            {
                string filename = Utils.SelectFile();
                if (filename != null)
                {
                    string file = File.ReadAllText(filename);
                    bool isValidatedFile = Utils.ValidateDatFile(file);

                    if (!isValidatedFile)
                    {
                        ToastService.Show("Incorrect format", Colors.Red);
                        return;
                    }

                    if (file != null)
                    {
                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = Crypto.key;
                            aes.IV = Crypto.iv;

                            file = Crypto.Encrypt(file, Crypto.key, Crypto.iv);
                            File.WriteAllText(DataSettings.filePath, file);
                        }
                        _dataSettings.LoadJson();
                        ToastService.Show("Backup was loaded!", Colors.Green);
                    }
                }
                else ToastService.Show("File is not selected", Colors.Red);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Load backup error: " + ex.Message);
                ToastService.Show("Load backup error", Colors.Red);
            }
        }
    }
}
