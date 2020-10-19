using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF___Chat.Models;

namespace WPF___Chat.Dialogs.Chat
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat : Window
    {
        public Chat()
        {
            InitializeComponent();
            StartConnection();
        }

        private string status = "Offline";
        private Connection connection = new Connection();

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        private void OnNoClicked(object parameter)
        {
            //this.CloseDialogWithResult(parameter as Window, DialogResult.No);
            Receive();

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
            set
            {
                historicoChat = value;
                OnPropertyChanged(nameof(chatBox));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string msgSend;

        public string sendBox
        {
            get { return msgSend; }
            set { msgSend = value; }
        }

        public void StartConnection()
        {
            try
            {
                connection.Start();
                status = "Online";
            }
            catch
            {
                status = "Offline";
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

            UpdateChat("[Stranger]: " + msgReceived);
        }

        public void UpdateChat(string message)
        {
            if (historicoChat == "")
            {
                historicoChat = message;
            }
            else
            {
                historicoChat = historicoChat + "\n\n" + message;
            }
        }
    }
}
