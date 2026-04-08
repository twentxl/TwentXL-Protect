using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helper.Interfaces
{
    internal interface IDataSettings
    {
        public void LoadJson();

        public void SaveJson();

        public void SaveKeys();

        public void LoadKeys();

        public void DestroyAll();
    }
}
