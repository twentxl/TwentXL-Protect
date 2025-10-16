using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.Components
{
    /// <summary>
    /// Логика взаимодействия для Toast.xaml
    /// </summary>
    public partial class Toast : UserControl
    {
        public Toast()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
        }

        public async void ShowToast(string message, Color? color = null)
        {
            ToastMessage.Content = message;
            if (color != null) ToastMessage.Foreground = new SolidColorBrush((Color)color);
            this.Visibility = Visibility.Visible;

            DoubleAnimation fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            this.BeginAnimation(UIElement.OpacityProperty, fadeIn);

            await Task.Delay(2000);

            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(300)
            };
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            await Task.Delay(300);
            this.Visibility = Visibility.Collapsed;
        }
    }
}
