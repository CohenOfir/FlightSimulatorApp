using FlightSimulatorApp.Tools;
using System;
using System.ComponentModel;
using System.Windows.Controls;


namespace FlightSimulatorApp.Views
{
    public partial class ControlUnit : UserControl, INotifyPropertyChanged
    {
        private string elevator, rudder;
        public event PropertyChangedEventHandler PropertyChanged;

        public ControlUnit()
        {
            InitializeComponent();
            Joystick.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                var args = e as JoyStickPropertyChanged;
                if (args != null)
                {
                    string property = args.PropertyName as string;
                    if (property.Equals("Elevator"))
                    {
                        Elevator = args.Propertyvalue;
                    }
                    else
                    {
                        if (property.Equals("Rudder"))
                        {
                            Rudder = args.Propertyvalue;
                        }
                    }
                }

            };
           
        }

        public void NotifyPropertyChanged(string propertyName, string newValue)
        {
            if (this.PropertyChanged != null)
                PropertyChanged(this, new JoyStickPropertyChanged(propertyName, newValue));
        }
        public string Elevator
        {
            get { return this.elevator; }
            set
            {
                this.elevator = value;
                this.NotifyPropertyChanged("Elevator", value);
            }
        }
        public string Rudder
        {
            get { return this.rudder; }
            set
            {
                this.rudder = value;
                this.NotifyPropertyChanged("Rudder", value);
            }
        }




    }
}
