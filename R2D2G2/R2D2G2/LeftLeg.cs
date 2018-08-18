using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace R2D2G2
{
    public class LeftLeg : Motor
    {
        public enum States
        {
            Off = 0,
            Forwards = 26, //GPIO26
            Backwards = 20 //GPIO20
        }

        public LeftLeg(GpioController pGpioController, States pState = States.Off) : base(pGpioController, (int)pState)
        {
            for (int i = 1; i < Enum.GetNames(typeof(States)).Length; i++)
            {
                var values = ((int[])Enum.GetValues(typeof(States)));
                Pins[values[i]] = gpioController.OpenPin(values[i]);
                Pins[values[i]].SetDriveMode(GpioPinDriveMode.Output);
            }
        }

        public void SetState(States state)
        {
            State = (int)state;

            for (int i = 1; i < Enum.GetNames(typeof(States)).Length; i++)
            {
                if (!(((int[])Enum.GetValues(typeof(States)))[i] == State))
                {
                    TurnOffPin(((int[])Enum.GetValues(typeof(States)))[i]);
                }
            }

            TurnOnPin(State);
        }
    }
}
