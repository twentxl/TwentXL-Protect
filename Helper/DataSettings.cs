using PasswordManager.Components;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Models;
using PasswordManager.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;

namespace PasswordManager.Helper
{
    public class DataSettings : ASettings, IDataSettings
    {
        private IJsonSettings _jsonSettings;
        private IKeysSettings _keysSettings;

        public DataSettings(IJsonSettings jsonSettings, IKeysSettings keysSettings)
        {
            _jsonSettings = jsonSettings;
            _keysSettings = keysSettings;
        }

        public void LoadJson()
        {
            _jsonSettings.LoadJson(filePath);
        }

        public void SaveJson()
        {
            _jsonSettings.SaveJson(filePath);
        }

        public void SaveKeys()
        {
            _keysSettings.SaveKeys(keysFile);
        }

        public void LoadKeys()
        {
            _keysSettings.LoadKeys(keysFile);
        }

        public void DestroyAll()
        {
            File.Delete(filePath);
            File.Delete(keysFile);
        }
    }
}
