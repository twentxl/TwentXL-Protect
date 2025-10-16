using System;
using System.Windows.Media;
using PasswordManager.Components;

namespace PasswordManager.Services
{
    public class ToastService
    {
        private static Toast _toast;

        public static void Initialize(Toast toast)
        {
            _toast = toast;
        }

        public static void Show(string message, Color? color = null)
        {
            _toast.ShowToast(message, color);
        }
    }
}
