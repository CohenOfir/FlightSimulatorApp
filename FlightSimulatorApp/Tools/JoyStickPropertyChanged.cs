using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulatorApp.Tools
{
    class JoyStickPropertyChanged : PropertyChangedEventArgs
    {
        public string propertyvalue; 

        public JoyStickPropertyChanged(string propertyName, string value) : base(propertyName)
        {
            propertyvalue = value;
        }

        public string Propertyvalue
        {
            get { return this.propertyvalue; }
            set { this.propertyvalue = value; }
        }
    }
}
