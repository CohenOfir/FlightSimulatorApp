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
using System.Windows.Shapes;

namespace FlightSimulatorApp.Views
{
    public partial class ConnectView : Window
    {
        private string ip;
        private int port;

        public ConnectView()
        {
            InitializeComponent();
        }

        private void Button_Click_Connect(object sender, RoutedEventArgs e)
        {
            //Read text boxes and store values in data members
            this.ip = ServerIP.Text;
            this.port = int.Parse(ServerPort.Text);
            this.Close();
        }

        public string Ip
        {
            get { return this.ip;}
        }

        public int Port
        {
            get { return this.port; }
        }
    }
}
