using PasswordManager.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.Helper
{
    public class KeysSettings : IKeysSettings
    {
        public void SaveKeys(string keysFile)
        {
            List<byte[]> keysList = new List<byte[]>()
            {
                Crypto.key, Crypto.iv
            };

            string json = JsonSerializer.Serialize(keysList);
            File.WriteAllText(keysFile, json);
            File.SetAttributes(keysFile, File.GetAttributes(keysFile) | FileAttributes.Hidden);
        }

        public void LoadKeys(string keysFile)
        {
            try
            {
                string keysJson = File.ReadAllText(keysFile);
               if(!string.IsNullOrWhiteSpace(keysJson) || !string.IsNullOrEmpty(keysJson))
                {
                    List<byte[]> keysList = JsonSerializer.Deserialize<List<byte[]>>(keysJson);
                    if (keysList != null)
                    {
                        Crypto.key = keysList[0];
                        Crypto.iv = keysList[1];
                    }
                }
            }
            catch
            {
                MessageBox.Show("Launch error: The keys were not found", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
