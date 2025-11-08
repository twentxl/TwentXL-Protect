using PasswordManager.Helper;
using PasswordManager.Pages;
using PasswordManager.Services;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager.Components
{
    public partial class DeleteDialog : UserControl
    {
        private DataBlock _dataBlock;
        public DeleteDialog(DataBlock dataBlock)
        {
            InitializeComponent();
            _dataBlock = dataBlock;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ModalService.HideModal();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MainPage.MainPageInstance?.DataBlockStackPanel.Children.Remove(_dataBlock);
            ModalService.HideModal();
            ToastService.Show("Block was deleted", Colors.Orange);
            DataSettings.SaveJson();
        }
    }
}
