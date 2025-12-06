using System;
using System.Windows;
using PasswordManager.Components;

namespace PasswordManager.Services
{
    class ModalService
    {
        public static void ShowModal(UIElement element)
        {
            ModalDialog modalDialog = new ModalDialog(element);
            MainWindow.Instance?.ModalDialog_Area.Children.Add(modalDialog);
        }

        public static void HideModal()
        {
            ModalDialog.Instance?.ModalContent.Children.Clear();
            MainWindow.Instance?.ModalDialog_Area.Children.Clear();
        }
    }
}
