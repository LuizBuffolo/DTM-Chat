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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPF___Chat.Dialogs.DialogYesNo
{
    class DialogYesNoViewModel : DialogViewModelBase,  INotifyPropertyChanged
    {
        private Connection connection = new Connection();
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        //===============[BINDING'S]
        private string historicoChat;
        public string chatBox
        {
            get { return historicoChat; }
            set
            {
                historicoChat = value;
                OnPropertyChanged();
            }
        }

        private bool btn_SendControl;
        public bool Btn_SendControl
        {
            get { return btn_SendControl; }
            set
            {
                btn_SendControl = value;
                OnPropertyChanged();
            }
        }

        private string stat;
        public string status
        {
            get { return stat; }
            set
            {
                stat = value;
                OnPropertyChanged();
            }
        }

        private string msgSend;
        public string sendBox
        {
            get { return ""; }
            set { msgSend = value; }
        }

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
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
        //[BINDING'S]===============

        public DialogYesNoViewModel()
        {
            StartConnection();

            status = "Online";
            Btn_SendControl = connection.isConnected;
            Thread th = new Thread(() =>
            {
                while (connection.isConnected)
                {
                    Receive();
                    status = "Online";
                }
                status = "Offline";
                Btn_SendControl = connection.isConnected;
            });

            th.Start();
                        
            this.noCommand = new RelayCommand(OnNoClicked);            
        }

        private void OnNoClicked(object parameter)
        {
            StopConnection();
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }

        public void StartConnection()
        {
            try {
                connection.StartUdp();
                Console.WriteLine(connection.tipo);
                if (connection.tipo == "Listener")
                {
                    connection.Listener();
                }
                else
                {
                    connection.Client();
                }
            }
            catch
            {

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

            sendBox = "";
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
                chatBox = message;
            }
            else {
                chatBox = historicoChat + "\n" + message;
            }
        }
    }
}
