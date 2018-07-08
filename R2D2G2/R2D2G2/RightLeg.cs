using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace R2D2G2
{
    public class RightLeg: Motor
    {
        public enum States
        {
            Off = 0,
            Forwards = 19,
            Backwards = 16
        }

        public RightLeg(GpioController pGpioController, States pState = States.Off) : base(pGpioController, (int)pState)
        {
            for (int i = 1; i < Enum.GetNames(typeof(States)).Length; i++)
            {
                Pins.Add(gpioController.OpenPin(((int[])Enum.GetValues(typeof(States)))[i]));
            }
        }

        public void SetState(States state)
        {
            State = (int)state;

            for (int i = 0; i < Enum.GetNames(typeof(States)).Length; i++)
            {                                                           //Why did I need to cast this and not the other part?
                if (!(Enum.GetNames(typeof(States))[i] == "Off") && !(((int[])Enum.GetValues(typeof(States)))[i] == State))
                {
                    TurnOffPin(((int[])Enum.GetValues(typeof(States)))[i]);
                }
            }

            TurnOnPin(State);
        }
    }
}
