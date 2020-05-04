using FlightSimulatorApp.Tools;
using FlightSimulatorApp.Views;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace FlightSimulatorApp.Model
{
    class MainWindowModel : INotifyPropertyChanged
    {
        private string throttle, aileron, elevator, rudder, latitude, longitude, airspeed, altitude,
                       roll, pitch, altimeter, heading, groundSpeed, verticalSpeed;
        private bool disconnected, connected;
        private TcpClient tcpClient;
        private NetworkStream netStream;
        private Queue<string> queue;


        public MainWindowModel()
        {
            this.latitude = "31.996";
            this.longitude = "34.8865";
            disconnected = false;
            this.queue = new Queue<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public void connectToServer(string ip, int port)
        {
            connected = true;
            try
            {
                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(ip, port);
                this.disconnected = false;
                this.Connected = true;
                this.netStream = tcpClient.GetStream();

            }
            catch 
            {
                throw new Exception("not connected");
            }

            try
            {
                this.fly();
            }
            catch
            {
                throw new Exception("connection interupt");
            }



        }

        public void disconnectServer()
        {
            disconnected = true;
            tcpClient.Close();
            connected = false;
        }
        //Write to Server
        public void write(string text)
        {
            this.netStream = this.tcpClient.GetStream();
            ASCIIEncoding asci = new ASCIIEncoding();
            byte[] msg = asci.GetBytes(text);
            netStream.Write(msg, 0, msg.Length);
        }
        //Read From Server
        public string read()
        {
            byte[] buffer = new byte[100];

            int k = this.netStream.Read(buffer, 0, 100);
            string text = "";
            for (int i = 0; i < k; i++)
                text += (Convert.ToChar(buffer[i]));
            return text;
        }
        //Server Comunication - read and write
        public void fly()
        {
            
            //New Thred for server comunication
                new Thread(delegate ()
                {
                    while (!this.disconnected)
                    {
                        try
                        {
                            //Read from server
                            this.write("get /instrumentation/airspeed-indicator/indicated-speed-kt\n");
                            this.Airspeed = this.read();
                            this.write("get /instrumentation/gps/indicated-altitude-ft\n");
                            this.Altitude = this.read();
                            this.write("get /instrumentation/attitude-indicator/internal-roll-deg\n");
                            this.Roll = this.read();
                            this.write("get /instrumentation/attitude-indicator/internal-pitch-deg\n");
                            this.Pitch = this.read();
                            this.write("get /instrumentation/altimeter/indicated-altitude-ft\n");
                            this.Altimeter = this.read();
                            this.write("get /instrumentation/heading-indicator/indicated-heading-deg\n");
                            this.Heading = this.read();
                            this.write("get /instrumentation/gps/indicated-ground-speed-kt\n");
                            this.GroundSpeed = this.read();
                            this.write("get /instrumentation/gps/indicated-vertical-speed\n");
                            this.VerticalSpeed = this.read();
                            this.write("get /position/latitude-deg\n");
                            this.Latitude = this.read();
                            this.write("get /position/longitude-deg\n");
                            this.Longitude = this.read();
                            //Write to server 
                            while (this.queue.Count != 0)
                            {
                                this.write(queue.Dequeue());
                                this.read();
                            }
                            Thread.Sleep(250);
                        }
                       
                        catch
                        {
                            disconnected = true;
                            connected = false;
                            this.NotifyPropertyChanged("IsConnected");
                            AutoClosingMessageBox.Show("Connection Interupted\nPlease Reconnect to Server\nClosing Window Please wait", "Error", 2500);
                        }
                    }
                }).Start();            
        }
        //Set commands queue - to be write to Server in Thread
        public void addToQueue(string msg)
        {
            this.queue.Enqueue(msg);
        }

        //ControlUnit- Joytick
        public string Throttle
        {
            get
            {
                return this.throttle;
            }
            set
            {
                this.throttle = value;
                string msg = "set /controls/engines/current-engine/throttle ";
                msg += value.ToString();
                msg += "\n";
                this.addToQueue(msg);
                this.NotifyPropertyChanged("Throttle");
            }
        }
        public string Aileron
        {
            get
            {
                return this.aileron;
            }
            set
            {
                this.aileron = value;
                string msg = "set /controls/flight/aileron ";
                msg += value.ToString();
                msg += "\n";
                this.addToQueue(msg);
                this.NotifyPropertyChanged("Aileron");

            }
        }
        public string Elevator
        {
            get
            {
                return this.elevator;
            }

            set
            {
                this.elevator = value;
                string msg = "set /controls/flight/elevator ";
                msg += value.ToString();
                msg += "\n";
                this.addToQueue(msg);
                this.NotifyPropertyChanged("Elevator");


            }
        }
        public string Rudder
        {
            get
            {
                return this.rudder;
            }
            set
            {
                this.rudder = value;
                string msg = "set /controls/flight/rudder ";
                msg += value.ToString();
                msg += "\n";
                this.addToQueue(msg);
                this.NotifyPropertyChanged("Rudder");

            }
        }
        //Map 
        public string Latitude
        {
            get { return this.latitude; }
            set
            {
                this.latitude = value;
                this.NotifyPropertyChanged("Location");
                this.NotifyPropertyChanged("Latitude");
            }
        }
        public string Longitude
        {
            get { return this.longitude; }
            set
            {

                this.longitude = value;
                this.NotifyPropertyChanged("Location");
                this.NotifyPropertyChanged("Longitude");

            }
        }
        public Location Location
        {
            get
            {
                double lat, lon;
                try
                {
                    lat = double.Parse(this.latitude);
                    lon = double.Parse(this.longitude);

                }
                catch
                {
                    return null;
                }

                return new Location(double.Parse(this.latitude), double.Parse(this.longitude));

            }
        }
        //Dashboard 
        public string Airspeed
        {
            get { return this.airspeed; }
            set
            {
                this.airspeed = value;
                this.NotifyPropertyChanged("Airspeed");
            }
        }
        public string Altitude
        {
            get { return this.altitude; }
            set
            {
                this.altitude = value;
                this.NotifyPropertyChanged("Altitude");
            }
        }
        public string Roll
        {
            get { return this.roll; }
            set
            {
                this.roll = value;
                this.NotifyPropertyChanged("Roll");
            }
        }
        public string Pitch
        {
            get { return this.pitch; }
            set
            {
                this.pitch = value;
                this.NotifyPropertyChanged("Pitch");
            }
        }
        public string Altimeter
        {
            get { return this.altimeter; }
            set
            {
                this.altimeter = value;
                this.NotifyPropertyChanged("Altimeter");
            }
        }
        public string Heading
        {
            get { return this.heading; }
            set
            {
                this.heading = value;
                this.NotifyPropertyChanged("Heading");
            }
        }
        public string GroundSpeed
        {
            get { return this.groundSpeed; }
            set
            {
                this.groundSpeed = value;
                this.NotifyPropertyChanged("GroundSpeed");
            }
        }
        public string VerticalSpeed
        {
            get { return this.verticalSpeed; }
            set
            {
                this.verticalSpeed = value;
                this.NotifyPropertyChanged("VerticalSpeed");
            }
        }
        //Connections
        private bool Disconnected
        {
            get { return this.disconnected; }
            set { this.disconnected = value; }
        }
        public bool Connected
        {
            get { return this.connected; }
            set 
            { 
                this.connected = value;
                this.NotifyPropertyChanged("IsConnected");

            }
        }
        public string IsConnected
        {
            get 
            {
                if (Connected)
                    return "Connected";
                else
                    return "Disconnected";

            }
        }

    }
}
