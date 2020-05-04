using FlightSimulatorApp.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.ComponentModel;


namespace FlightSimulatorApp.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(MainWindowModel model)
        {
            this.model = model;
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        //Joystick
        public string Rudder
        {
            get
            {
                return this.model.Rudder;
            }
            set
            {
                this.model.Rudder = value;
            }
        }
        public string Aileron
        {
            get
            {
                return this.model.Aileron;
            }
            set
            {
                this.model.Aileron = value;
            }
        }
        public string Elevator
        {
            get
            {
                return this.model.Elevator;
            }

            set
            {
                this.model.Elevator = value;
            }
        }
        public string Throttle
        {
            get
            {
                return this.model.Throttle;
            }
            set
            {
                this.model.Throttle = value;
            }
        }
        //Map
        public string Latitude
        {
            get { return this.model.Latitude; }
        }
        public string Longitude
        {
            get { return this.model.Longitude; }
        }
        //Dashboard
        public string Airspeed
        {
            get { return this.model.Airspeed; }
        }
        public string Altitude
        {
            get { return this.model.Altitude; }
        }
        public string Roll
        {
            get { return this.model.Roll; }
        }
        public string Pitch
        {
            get { return this.model.Pitch; }
        }
        public string Altimeter
        {
            get { return this.model.Altimeter; }
        }
        public string Heading
        {
            get { return this.model.Heading; }
        }
        public string GroundSpeed
        {
            get { return this.model.GroundSpeed; }
        }
        public string VerticalSpeed
        {
            get { return this.model.VerticalSpeed; }
        }
        //Connections
        public Location Location
        {
            get { return model.Location; }
        }
        public bool Connected
        {
            get { return model.Connected; }
        }
        public string IsConnected
        {
            get { return model.IsConnected; }
        }
        

    }
}
