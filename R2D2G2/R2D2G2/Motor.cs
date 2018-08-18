﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace R2D2G2
{
    public class Motor
    {
        public GpioController gpioController;
        protected int State { get; set; }
        public List<GpioPin> Pins { get; set; }

        public Motor(GpioController pGpioController, int pState = -1) //All Motors default to state -1/Off
        {
            gpioController = pGpioController;
            State = pState;
            Pins = new List<GpioPin>();
        }

        protected void TurnOnPin(int pinNum)
        {
            if (pinNum != -1)
            {
                Pins[pinNum].Write(GpioPinValue.High);
            }
        }

        protected void TurnOffPin(int pinNum)
        {
            if (pinNum != -1)
            {
                Pins[pinNum].Write(GpioPinValue.Low); //TODO make pinNum the index of the pinNum for that motor
            }
        }
    }
}
