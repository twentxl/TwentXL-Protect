using PasswordManager.Components;
using PasswordManager.Models;
using PasswordManager.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using PasswordManager.Helper.Interfaces;
using System.Windows;
using System.Security.Cryptography;

namespace PasswordManager.Helper
{
    public class JsonSettings : ASettings, IJsonSettings
    {
        private IKeysSettings _keysSettings;

        public JsonSettings(IKeysSettings keysSettings)
        {
            _keysSettings = keysSettings;
        }

        public void LoadJson(string filePath)
        {
            try
            {
                MainPage.MainPageInstance?.DataBlockStackPanel.Children.Clear();
                string directory = Path.GetDirectoryName(filePath)!;
                Directory.CreateDirectory(directory);

                if (!File.Exists(filePath))
                    SaveJson(filePath);

                string file = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(file) || !string.IsNullOrWhiteSpace(file))
                {
                    _keysSettings.LoadKeys(keysFile);
                    file = Crypto.Decrypt(file, Crypto.key, Crypto.iv);

                    List<PasswordModel> passwordList = JsonSerializer.Deserialize<List<PasswordModel>>(file);

                    if(passwordList != null && passwordList.Count > 0)
                    {
                        foreach (var item in passwordList)
                        {
                            DataBlock dataBlock = new DataBlock(item.Title, item.Login, item.Password, item.Additional, item.CreatedDate);
                            MainPage.MainPageInstance?.DataBlockStackPanel.Children.Add(dataBlock);
                        }

                        Utils.PasswordsListCheck();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JSON credentials load error: " + ex.Message);
                MessageBox.Show("JSON credentials load error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveJson(string filePath)
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

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true
                };
                string json = JsonSerializer.Serialize(passwordList, options);

                using (Aes aes = Aes.Create())
                {
                    if ((Crypto.key == null || Crypto.key.Length == 0) || (Crypto.iv == null || Crypto.iv.Length == 0))
                    {
                        aes.KeySize = 256;
                        aes.GenerateKey();
                        aes.GenerateIV();

                        Crypto.key = aes.Key;
                        Crypto.iv = aes.IV;

                        _keysSettings.SaveKeys(keysFile);
                    }
                    else
                    {
                        aes.Key = Crypto.key;
                        aes.IV = Crypto.iv;
                    }

                    string file = Crypto.Encrypt(json, Crypto.key, Crypto.iv);
                    File.WriteAllText(filePath, file);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JSON credentials save error: " + ex.Message);
                MessageBox.Show("JSON credentials save error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
