using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helper.Interfaces
{
    public interface IJsonSettings
    {
        public void LoadJson(string filePath);
        public void SaveJson(string filePath);
    }
}
