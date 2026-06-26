using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helper.Interfaces
{
    public interface IGlobalSettings
    {
        public void LoadSettings();
        public void SaveSettings();
        public void CreateBackup();
        public void LoadBackup();
        public void ApplyTheme(bool isDark);
    }
}
