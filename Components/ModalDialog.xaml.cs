using System;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager.Components
{
    public partial class ModalDialog : UserControl
    {
        public ModalDialog(UIElement element)
        {
            InitializeComponent();
            ModalContent.Children.Add(element);
        }
    }
}
