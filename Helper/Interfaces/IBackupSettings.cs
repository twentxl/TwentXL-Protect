using PasswordManager.Models;

namespace PasswordManager.Helper.Interfaces
{
    internal interface IBackupSettings
    {
        public void CreateBackup(SettingsModel settingsModel);
        public void LoadBackup();
    }
}
