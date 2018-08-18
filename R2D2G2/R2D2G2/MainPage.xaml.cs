using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Gpio;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace R2D2G2
{

    public sealed partial class MainPage : Page
    {
        GpioController gpioController;
        DispatcherTimer timer;
        readonly List<Gamepad> gamepads;
        double gamepadDeadZone = 0.15;
        Head head;
        RightLeg rLeg;
        LeftLeg lLeg;

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;

            InitGPIO();
            InitMotors();

            gamepads = new List<Gamepad>();
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            timer.Start();
        }
        
        private void InitMotors()
        {
            head = new Head(gpioController);
            rLeg = new RightLeg(gpioController);
            lLeg = new LeftLeg(gpioController);
        }

        private void InitGPIO()
        {
            //Show an error if there is no GPIO controller
            if (GpioController.GetDefault() == null)
            {
                txbDebug.Text = "There is no GPIO controller on this device";
                return;
            }

            gpioController = GpioController.GetDefault();

            //Initialize pins
            txbDebug.Text = $"GPIO pins ";

            /*for (int i = 0; i < Pins.Length; i++)
            {
                Pins[i] = gpio.OpenPin(pinNums[i]);
                Pins[i].SetDriveMode(GpioPinDriveMode.Output);
                txbDebug.Text += $"i ";
            }*/

            txbDebug.Text += "no longer initialize on startup (Is this gonna be too slow?)";
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            gamepads.Add(e);
            lLeg.Gamepads = gamepads;
            rLeg.Gamepads = gamepads;
            head.Gamepads = gamepads;
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            gamepads.Remove(e);
            lLeg.Gamepads = gamepads;
            rLeg.Gamepads = gamepads;
            head.Gamepads = gamepads;
        }

        private void Timer_Tick(object sender, object e)
        {
            //txbDebug.Text = gamepads.Count.ToString();

            if (gamepads.Count > 0)
            {
                var reading = gamepads[0].GetCurrentReading();

                //make left leg go forward if left stick is pressed forwards
                if (reading.LeftThumbstickY > gamepadDeadZone)
                {
                    lLeg.SetState(LeftLeg.States.Forwards);
                }
                else if (reading.LeftThumbstickY < -gamepadDeadZone)
                {
                    lLeg.SetState(LeftLeg.States.Backwards);
                }
                else
                {
                    lLeg.SetState(LeftLeg.States.Off);
                }

                //make right leg go forward if right stick is pressed forwards
                if (reading.RightThumbstickY > gamepadDeadZone)
                {
                    rLeg.SetState(RightLeg.States.Forwards);
                }
                else if (reading.RightThumbstickY < -gamepadDeadZone)
                {
                    rLeg.SetState(RightLeg.States.Backwards);
                }
                else
                {
                    rLeg.SetState(RightLeg.States.Off);
                }
            }
        }
    }
}
