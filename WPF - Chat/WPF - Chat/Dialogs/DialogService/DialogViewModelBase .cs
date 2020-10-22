using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static WPF___Chat.Dialogs.DialogService.DialogResultEnum;

namespace WPF___Chat.Dialogs.DialogService
{
    public abstract class DialogViewModelBase
    {
        public DialogResult UserDialogResult
        {
            get;
            private set;
        }

        public void CloseDialogWithResult(Window dialog, DialogResult result)
        {
            dialog.Close();
            this.UserDialogResult = result;
            if(dialog != null)
            {
                dialog.DialogResult = true;
            }
        }
    }
}
