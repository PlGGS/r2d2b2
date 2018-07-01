using System;
using Windows.Devices.Gpio;

namespace R2D2G2
{
    public class State
    {
        string name;
        public string Name { get => name; set => name = value; }
        GpioPin pin;
        public GpioPin Pin { get => pin; set => pin = value; }

        public State(string pName, GpioPin pPin)
        {
            Name = pName;
            Pin = pPin;
        }
    }
}