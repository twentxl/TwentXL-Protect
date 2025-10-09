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

namespace PasswordManager.Helper
{
    public class DataSettings
    {
        private string filePath;

        public DataSettings()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            filePath = Path.Combine(localAppData, "TwentXL Protect", "user_credentials.json");
        }

        public void LoadJson()
        {
            try
            {
                string directory = Path.GetDirectoryName(filePath)!;
                Directory.CreateDirectory(directory);

                if (!File.Exists(filePath))
                    File.Create(filePath).Dispose();

                string json = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(json))
                {
                    List<PasswordModel> passwordList = JsonSerializer.Deserialize<List<PasswordModel>>(json);

                    foreach (var item in passwordList)
                    {
                        DataBlock dataBlock = new DataBlock(item.Title, item.Login, item.Password, item.Additional);
                        MainPage.MainPageInstance?.DataBlockStackPanel.Children.Add(dataBlock);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine("JSON load error: " + ex.Message);
                MessageBox.Show("JSON load error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveJson()
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
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("JSON save error: " + ex.Message);
                MessageBox.Show("JSON save error", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
