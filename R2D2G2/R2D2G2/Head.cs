using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace R2D2G2
{
    public class Head : Motor
    {
        public enum States
        {
            Off = -1,
            Left = 6,
            Right = 12
        }

        public Head(GpioController pGpioController, States pState = States.Off) : base(pGpioController, (int)pState)
        {
            for (int i = 1; i < Enum.GetNames(typeof(States)).Length; i++)
            {
                if (Pins != null)
                {
                    Pins.Add(gpioController.OpenPin(((int[])Enum.GetValues(typeof(States)))[i]));
                }
            }
        }

        public void SetState(States state)
        {
            State = (int)state;

            for (int i = 1; i < Enum.GetNames(typeof(States)).Length; i++)
            {                if (!(((int[])Enum.GetValues(typeof(States)))[i] == State))
                {
                    TurnOffPin(((int[])Enum.GetValues(typeof(States)))[i]);
                }
            }

            TurnOnPin(State);
        }
    }
}
