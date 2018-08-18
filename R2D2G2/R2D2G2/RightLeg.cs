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
            Forwards = 19, //GPIO19
            Backwards = 16 //GPIO16
        }

        public RightLeg(GpioController pGpioController, States pState = States.Off) : base(pGpioController, (int)pState)
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
            SetPinState((States)State);
        }

        private void SetPinState(States state)
        {
            switch (state)
            {
                case States.Off:
                    Pins[(int)States.Forwards].Write(GpioPinValue.High);
                    Pins[(int)States.Backwards].Write(GpioPinValue.High);
                    break;
                case States.Forwards:
                    Pins[(int)States.Backwards].Write(GpioPinValue.High);
                    Pins[(int)States.Forwards].Write(GpioPinValue.Low);
                    break;
                case States.Backwards:
                    Pins[(int)States.Forwards].Write(GpioPinValue.High);
                    Pins[(int)States.Backwards].Write(GpioPinValue.Low);
                    break;
                default:
                    break;
            }
        }
    }
}
