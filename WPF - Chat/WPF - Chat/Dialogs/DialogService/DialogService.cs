using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPF___Chat.Dialogs.DialogService.DialogResultEnum;

namespace WPF___Chat.Dialogs.DialogService
{
    public static class DialogService
    {
        public static DialogResult OpenDialog(DialogViewModelBase vm)
        {
            DialogWindow win = new DialogWindow();
            win.DataContext = vm;
            win.ShowDialog();
            return (win.DataContext as DialogViewModelBase).UserDialogResult;
        }
    }
}
