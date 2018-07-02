using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace R2D2G2
{
    public class Motor
    {
        protected int State { get; set; }

        public Motor(int pState = -1) //All Motors default to state -1/Off
        {
            State = pState;
        }

        protected void TurnOnPin(GpioController gpioController, int pinNum)
        {
            GpioPin tmpPin = gpioController.OpenPin(pinNum);
            tmpPin.Write(GpioPinValue.High);
        }

        protected void TurnOffPin(GpioController gpioController, int pinNum)
        {
            GpioPin tmpPin = gpioController.OpenPin(pinNum);
            tmpPin.Write(GpioPinValue.Low);
        }
    }
}
