using FlightSimulatorApp.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulatorApp.Views
{
    public partial class Joystick : UserControl, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private Point MousePoint;
        private string elev ="0", rudd="0";
        public Joystick()
        {
            InitializeComponent();
        }
        
        public void NotifyPropertyChanged(string propName, string value)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new JoyStickPropertyChanged(propName, value));
        }
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                MousePoint = e.GetPosition(this);
            }
        }
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = e.GetPosition(this).X - MousePoint.X;
                double y = e.GetPosition(this).Y - MousePoint.Y;
                if (Math.Sqrt(x * x + y * y) < Base.Width / 6)
                {
                    Mouse.Capture(this.KnobBase);
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }
                
                else
                {
                    double delta = 1 - (Math.Sqrt(x * x + y * y) - Base.Width / 6) / (Math.Sqrt(x * x + y * y));
                    x = x * delta;
                    y = y * delta;
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }

                //Normalize values
                Elevetor = (x / Math.Sqrt(x * x + y * y)).ToString();
                Rudder = (y / Math.Sqrt(x * x + y * y)).ToString();
            }
        }
        private void center_knob(object sender, MouseButtonEventArgs e)
        {
            knobPosition.X = 0;
            knobPosition.Y = 0;
            Elevetor = "0";
            Rudder = "0";
            Mouse.Capture(null);
        }
        public string Elevetor
        {
            get { return this.elev; }
            set
            {
                this.elev = value;
                NotifyPropertyChanged("Elevator", value);
            }
        }
        public string Rudder
        {
            get { return this.rudd; }
            set
            {
                this.rudd = value;
                NotifyPropertyChanged("Rudder", value);
            }
        }

    }
}
