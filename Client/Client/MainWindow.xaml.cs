using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpClient tcpClient = new TcpClient();

        Thread thread;

        public MainWindow()
        {
            InitializeComponent();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] RecBuf = new byte[4 * 1024];
            int port;
            int.TryParse(textBoxPort.Text, out port);
            tcpClient.Connect(textBoxIp.Text, port);
            tcpClient.Client.Send(Encoding.UTF8.GetBytes(textBoxName.Text));
            int recSize = tcpClient.Client.Receive(RecBuf);
            string text = Encoding.UTF8.GetString(RecBuf, 0, recSize);

            textBlockMessages.Text += text + "\n";

            thread = new Thread(Cucle);
            thread.Start();
        }

        private void Cucle(object obj)
        {
            while (true)
            {
                byte[] RecBuf = new byte[4 * 1024];
                int recSize = tcpClient.Client.Receive(RecBuf);
                string json = Encoding.UTF8.GetString(RecBuf, 0, recSize);

                Messages messages = JsonConvert.DeserializeObject<Messages>(json);

                Dispatcher.Invoke(() => {
                    comboBoxName.Items.Clear();

                    foreach (var i in messages.ListName)
                    {
                        comboBoxName.Items.Add(i);
                    }
                });
                

                Dispatcher.Invoke(() => {
                    textBlockMessages.Text += messages.Text + "\n";
                });
                
                Thread.Sleep(1);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            byte[] RecBuf = new byte[4 * 1024];

            Messages messages = new Messages
            {
                Text = textBoxSend.Text,
            };
            if(comboBoxName.SelectedIndex >= 0)
            messages.NamePerson = (string)comboBoxName.Items[comboBoxName.SelectedIndex];

            string json = JsonConvert.SerializeObject(messages);            

            RecBuf = Encoding.UTF8.GetBytes(json);
            

            tcpClient.Client.Send(RecBuf);
        }
    }
}
