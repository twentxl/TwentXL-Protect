using PasswordManager.Components;
using PasswordManager.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PasswordManager.Models;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using System.Security.Cryptography;

namespace PasswordManager.Helper
{
    public class DataSettings
    {
        private static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static string filePath = Path.Combine(localAppData, "TwentXL Protect", "user_credentials.dat");
        private static string keysFile = Path.Combine(localAppData, "TwentXL Protect", "keys.json");

        public static void LoadJson()
        {
            try
            {
                MainPage.MainPageInstance?.DataBlockStackPanel.Children.Clear();
                string directory = Path.GetDirectoryName(filePath)!;
                Directory.CreateDirectory(directory);

                if (!File.Exists(filePath))
                    SaveJson();

                string file = File.ReadAllText(filePath);
                if (file != null)
                {
                    LoadKeys();
                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = Crypto.key;
                        aes.IV = Crypto.iv;

                        file = Crypto.Decrypt(file, Crypto.key, Crypto.iv);
                    }

                    List<PasswordModel> passwordList = JsonSerializer.Deserialize<List<PasswordModel>>(file);

                    foreach (var item in passwordList)
                    {
                        DataBlock dataBlock = new DataBlock(item.Title, item.Login, item.Password, item.Additional);
                        MainPage.MainPageInstance?.DataBlockStackPanel.Children.Add(dataBlock);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("JSON credentials load error: " + ex.Message);
                MessageBox.Show("JSON credentials load error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void SaveJson()
        {
            try
            {
                List<PasswordModel> passwordList = new List<PasswordModel>();
                var dataBlockPanel = MainPage.MainPageInstance?.DataBlockStackPanel.Children;

                foreach (var item in dataBlockPanel)
                {
                    if (item is DataBlock dataBlock)
                    {
                        passwordList.Add(new PasswordModel { Title = dataBlock.Title_Content.Content.ToString(), Login = dataBlock.Login_Content.Text, Password = dataBlock.Password_Content.Text, Additional = dataBlock.Additional_Content.Text });
                    }
                }

                string json = JsonSerializer.Serialize(passwordList);

                using(Aes aes = Aes.Create())
                {
                    if((Crypto.key == null || Crypto.key.Length == 0) && (Crypto.iv == null || Crypto.iv.Length == 0))
                    {
                        aes.KeySize = 256;
                        aes.GenerateKey();
                        aes.GenerateIV();

                        Crypto.key = aes.Key;
                        Crypto.iv = aes.IV;

                        SaveKeys();
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

        private static void SaveKeys()
        {
            List<byte[]> keysList = new List<byte[]>()
            {
                Crypto.key, Crypto.iv
            };
            string json = JsonSerializer.Serialize(keysList);
            File.WriteAllText(keysFile, json);
        }

        public static void LoadKeys()
        {
            try 
            { 
                string keysJson = File.ReadAllText(keysFile);
                List<byte[]> keysList = JsonSerializer.Deserialize<List<byte[]>>(keysJson);
                Crypto.key = keysList[0];
                Crypto.iv = keysList[1];
            }
            catch
            {
                MessageBox.Show("Launch error: The keys were not found", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
        }
    }
}
