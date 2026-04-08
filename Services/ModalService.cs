using System;
using System.Windows;
using PasswordManager.Components;

namespace PasswordManager.Services
{
    class ModalService
    {
        private static ModalDialog? currentModal;

        public static void ShowModal(UIElement element)
        {
            currentModal = new ModalDialog(element);
            MainWindow.Instance?.ModalDialog_Area.Children.Add(currentModal);
        }

        public static void HideModal()
        {
            if (currentModal != null)
            {
                currentModal.ModalContent.Children.Clear();
                MainWindow.Instance?.ModalDialog_Area.Children.Remove(currentModal);
                currentModal = null;
            }
        }
    }
}
