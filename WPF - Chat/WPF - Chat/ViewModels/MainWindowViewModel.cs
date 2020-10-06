using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF___Chat.ViewModels
{
    public class MainWindowViewModel
    {
        private ICommand openDialogCommand = null;
        public ICommand OpenDialogCommand
        {
            get { return this.openDialogCommand;  }
            set { this.openDialogCommand = value;  }
        }

        public MainWindowViewModel()
        {
            this.openDialogCommand = new RelayCommand(OnOpenDialog);
        }

        private void OnOpenDialog(object parameter)
        {
            Dialogs.DialogService.DialogViewModelBase vm =
            new Dialogs.DialogYesNo.DialogYesNoViewModel();

            Dialogs.DialogService.DialogResultEnum.DialogResult result = 
            Dialogs.DialogService.DialogService.OpenDialog(vm);
        }
    }
}
