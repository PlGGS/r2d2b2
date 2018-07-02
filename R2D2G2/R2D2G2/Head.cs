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
            Off = 0,
            Left = 6,
            Right = 12
        }
        GpioController gpioController;

        public Head(GpioController pGpioController, States pState = States.Off) : base((int)pState)
        {
            gpioController = pGpioController;
        }

        public void SetState(States state)
        {
            State = (int)state;

            for (int i = 0; i < Enum.GetNames(typeof(States)).Length; i++)
            {                                                           //Why did I need to cast this and not the other part?
                if (!(Enum.GetNames(typeof(States))[i] == "Off") && !(((int[])Enum.GetValues(typeof(States)))[i] == State))
                {
                    TurnOffPin(gpioController, i);
                }
            }

            TurnOnPin(gpioController, State);
        }
    }
}
