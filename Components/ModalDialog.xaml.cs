using System;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.Components
{
    public partial class ModalDialog : UserControl
    {
        public static ModalDialog? Instance;
        public ModalDialog(UIElement element)
        {
            InitializeComponent();
            Instance = this;
            ModalContent.Children.Add(element);
        }
    }
}
