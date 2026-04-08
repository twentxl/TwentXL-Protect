using PasswordManager.Pages;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Forms;

namespace PasswordManager.Helper
{
    public class Utils
    {
        public static string GenerateRandomText(int min, int max)
        {
            Random rand = new Random();
            int length = rand.Next(min, max);
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_";
            StringBuilder res = new StringBuilder();

            for (int i = 0; i < length; i++)
                res.Append(valid[rand.Next(valid.Length)]);

            return res.ToString();
        }

        public static string GetPathDir()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Select Folder";
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }
            return null;
        }

        public static string SelectFile()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*",
                DefaultExt = ".dat",
            };

            bool? result = dialog.ShowDialog();
            if (result == true) return dialog.FileName;
            else return null;
        }

        public static void PasswordsListCheck()
        {
            if (MainPage.MainPageInstance?.DataBlockStackPanel.Children.Count > 0)
                MainPage.MainPageInstance.EmptyPasswords_Message.Visibility = Visibility.Hidden;
            else
                MainPage.MainPageInstance.EmptyPasswords_Message.Visibility = Visibility.Visible;
        }

        public static bool ValidateDatFile(string content)
        {
            try
            {
                using var doc = JsonDocument.Parse(content);
                if (doc.RootElement.ValueKind != JsonValueKind.Array)
                    return false;

                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    if (item.ValueKind != JsonValueKind.Object)
                        return false;

                    var foundFields = new HashSet<string>();
                    foreach (var prop in item.EnumerateObject())
                    {
                        if (prop.Value.ValueKind != JsonValueKind.String)
                            return false;
                        foundFields.Add(prop.Name);
                    }

                    if (foundFields.Count != 5 || !foundFields.SetEquals(new[] { "Title", "Login", "Password", "Additional", "CreatedDate" }))
                        return false;
                }

                return true;
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("Incorrect format: " + ex.Message);
                return false;
            }
        }
    }
}
