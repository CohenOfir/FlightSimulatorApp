using FlightSimulatorApp.Tools;
using FlightSimulatorApp.ViewModel;
using FlightSimulatorApp.Views;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;
using System.Windows;

namespace FlightSimulatorApp
{
    public partial class MainWindow : Window
    {
        MainWindowViewModel vm;
        ConnectView connectWin;
        LocationRect bounds;
        private bool stopped;

        public MainWindow()
        {
            InitializeComponent();
            this.stopped = false;
            map.SetView(new Location(31.997, 34.886), 15);
            this.vm = new MainWindowViewModel(new Model.MainWindowModel());
            //Property Changed Assignment 
            this.vm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) { };
            ControlUnit.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                var args = e as JoyStickPropertyChanged;
                if (args != null)
                {
                    string property = args.PropertyName;
                    if (property.Equals("Elevator"))
                    {
                        this.vm.Elevator = args.Propertyvalue;
                    }

                    if (property.Equals("Rudder"))
                    {
                        {
                            this.vm.Rudder = args.Propertyvalue;
                        }
                    }
                }
            };
            DataContext = this.vm;
        }
        //Server Connect
        private void Button_Click_Connect(object sender, RoutedEventArgs e)
        {
            this.connectWin = new ConnectView();
            this.stopped = false;
            connectWin.ShowDialog();
            try
            {
                this.vm.model.connectToServer(connectWin.Ip, connectWin.Port);
            }
            catch
            {
                AutoClosingMessageBox.Show("Connection Failed\nPlease try again\nClosing Window Please wait", "Error", 2500);
            }
        }
        //Disconnected from Server
        private void Button_Click_Disconnect(object sender, RoutedEventArgs e)
        {
            this.vm.model.disconnectServer();
            this.vm.model.Connected = false;
            pin.Location.Latitude = 31.997;
            pin.Location.Longitude = 34.886;
            map.SetView(new Location(pin.Location.Latitude, pin.Location.Longitude), 15);
        }
        //Plane Location on Map
        private void pinUpdated(object sender, EventArgs e)
        {
            if (pin.Location != null && stopped == false)
            {
                this.bounds = map.BoundingRectangle;
                double mapCenterLat = bounds.Center.Latitude,
                       mapCenterLon = bounds.Center.Longitude,
                       pinLat = pin.Location.Latitude,
                       pinLon = pin.Location.Longitude;

                //Current Frame Violation - plane is outside Map Bounds
                if ((pinLat > bounds.North || pinLat < bounds.South || pinLon > bounds.East || pinLon < bounds.West) && !stopped)
                {
                    map.SetView(new Location(pinLat, pinLon), 12);
                }

                //Map Bounds Violation (Nort,Sout Poles)
                if ((pinLat >= 86 || pinLat <= -86))
                {
                    AutoClosingMessageBox.Show("Map Bounds Violation\nDisconnecting Server\nClosing Window Please wait", "Error", 2500);
                    this.vm.model.disconnectServer();
                    this.stopped = true;
                    pin.Location.Latitude = 32;
                    pin.Location.Longitude = 34.88;
                    map.SetView(new Location(pin.Location.Latitude, pin.Location.Longitude), 8);
                    this.vm.model.Latitude = "32";
                    this.vm.model.Longitude = "34.88";
                    this.vm.model.Connected = false;
                }
            }
        }
        //Window Closed- X button
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (this.vm.model.Connected)
            {
                this.vm.model.disconnectServer();
            }
        }
    }
}
