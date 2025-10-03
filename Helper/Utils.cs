using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
