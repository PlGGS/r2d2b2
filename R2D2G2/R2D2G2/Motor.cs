using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Gaming.Input;

namespace R2D2G2
{
    public class Motor
    {
        public GpioController gpioController;
        public List<Gamepad> Gamepads = new List<Gamepad>();
        public int State { get; set; }
        public GpioPin[] Pins { get; set; }

        public Motor(GpioController pGpioController, int pState = 0) //All Motors default to state -1/Off
        {
            gpioController = pGpioController;
            State = pState;
            Pins = new GpioPin[40];
        }
    }
}
