using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.Helper.Interfaces
{
    public interface IKeysSettings
    {
        public void SaveKeys(string keysFile);
        public void LoadKeys(string keysFile);
    }
}
