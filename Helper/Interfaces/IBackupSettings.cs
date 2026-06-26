using PasswordManager.Models;

namespace PasswordManager.Helper.Interfaces
{
    public interface IBackupSettings
    {
        public void CreateBackup(SettingsModel settingsModel);
        public void LoadBackup();
    }
}
