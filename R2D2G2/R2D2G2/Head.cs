using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Gaming.Input;

namespace R2D2G2
{
    public class Head : Motor
    {
        public enum States
        {
            Off = 0,
            Left = 6, //GPIO6
            Right = 12 //GPIO12
        }
        public List<Gamepad> Gamepads = new List<Gamepad>();

        public Head(GpioController pGpioController, States pState = States.Off) : base(pGpioController, (int)pState)
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
            if (Gamepads.Count > 0)
            {
                State = (int)state;
                SetPinState((States)State);
            }
            else
            {
                SetPinState(States.Off);
            }
        }

        private void SetPinState(States state)
        {
            switch (state)
            {
                case States.Off:
                    Pins[(int)States.Left].Write(GpioPinValue.High);
                    Pins[(int)States.Right].Write(GpioPinValue.High);
                    break;
                case States.Left:
                    Pins[(int)States.Left].Write(GpioPinValue.High);
                    Pins[(int)States.Right].Write(GpioPinValue.Low);
                    break;
                case States.Right:
                    Pins[(int)States.Right].Write(GpioPinValue.High);
                    Pins[(int)States.Left].Write(GpioPinValue.Low);
                    break;
                default:
                    break;
            }
        }
    }
}
