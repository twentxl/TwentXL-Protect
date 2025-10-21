using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Models
{
    public class SettingsModel
    {
        public bool DarkTheme { get; set; } = false;
        public string BackupPath { get; set; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    }
}
