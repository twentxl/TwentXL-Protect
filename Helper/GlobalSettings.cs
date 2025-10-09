using PasswordManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace PasswordManager.Helper
{
    public class GlobalSettings
    {
        private const string LightThemePath = "pack://application:,,,/Styles.xaml";
        private const string DarkThemePath = "pack://application:,,,/Styles_Dark.xaml";

        public void ApplyTheme(bool isDark)
        {
            string themePath;

            if (isDark) themePath = DarkThemePath;
            else themePath = LightThemePath;

                var app = Application.Current;
            var dict = new ResourceDictionary { Source = new Uri(themePath) };

            var oldTheme = app.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source?.OriginalString.Contains("Styles_") == true);
            if (oldTheme != null)
                app.Resources.MergedDictionaries.Remove(oldTheme);

            app.Resources.MergedDictionaries.Add(dict);
        }
    }
}
