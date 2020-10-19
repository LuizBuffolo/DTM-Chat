using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Input;
using WPF___Chat.Dialogs.DialogService;
using WPF___Chat.Models;
using System.Threading;
using static WPF___Chat.Dialogs.DialogService.DialogResultEnum;
using System.Windows.Data;

namespace WPF___Chat.Dialogs.DialogYesNo
{
    class DialogYesNoViewModel : DialogViewModelBase
    {
        private string status;
        private Connection connection = new Connection();

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        public DialogYesNoViewModel()
        {
            StartConnection();
            
            Thread th = new Thread(() =>
            {
                while (connection.isConnected)
                {
                    Receive();
                    status = "Online";
                }
                status = "Offline";
            });

            th.Start();

            
            this.noCommand = new RelayCommand(OnNoClicked);
            
        }

        private void OnNoClicked(object parameter)
        {
            StopConnection();
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }

        private ICommand _btn_Send;

        public ICommand Btn_Send
        {
            get
            {
                if (_btn_Send == null)
                {
                    _btn_Send = new RelayCommand(param => this.Send());
                }
                return _btn_Send;
            }
        }

        private string historicoChat;

        public string chatBox
        {
            get { return historicoChat; }
            set { historicoChat = value; }
        }

        private string msgSend;

        public string sendBox
        {
            get { return msgSend; }
            set { msgSend = value; }
        }

        public void StartConnection()
        {
            try {
                connection.Start();
                status = "Online";
            }
            catch
            {
                status = "Offline";
            }
        }

        public void StopOtherside()
        {
            connection.StopOtherside();
        }

        public void StopConnection()
        {
            try
            {
                connection.Stop();
                status = "Offline";
            }
            catch
            {
                status = "Online";
            }
        }
        public void Send()
        {
            connection.SendMessage(msgSend);

            UpdateChat("[You]: " + msgSend);

            
        }

        public void Receive()
        {
            string msgReceived;
            msgReceived = connection.ReceiveMessage();

            if(msgReceived != "")
                UpdateChat("[Stranger]: " + msgReceived);
        }

        public void UpdateChat(string message)
        {
            if (historicoChat == "")
            {
                historicoChat = message;
            }
            else {
                historicoChat = historicoChat + "\n\n" + message;
            }

            //descobrir como fazer binding para enviar para o chatBox
            Console.WriteLine(historicoChat);

        }
    }
}
