using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Gpio;
using System.Threading.Tasks;
using Windows.Gaming.Input;
using System.Collections.Generic;
using System.Reflection;

namespace R2D2G2
{

    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer;
        Gamepad controller;
        Motor lLeg;
        Motor rLeg;
        Motor head;
        public GpioPin[] Pins { get; set; } = new GpioPin[6];

        public MainPage()
        {
            this.InitializeComponent();
            
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;

            InitGPIO();
            InitMotors();

            //IReadOnlyList<Gamepad> gamepads = Gamepad.Gamepads;

            timer.Start();
        }

        private void InitMotors()
        {
            lLeg = new Motor(new List<State>() { new State("Forward", Pins[0]),
                                                    new State("Backward", Pins[1]) });
            rLeg = new Motor(new List<State>() { new State("Forward", Pins[2]),
                                                    new State("Backward", Pins[3]) });
            head = new Motor(new List<State>() { new State("Left", Pins[4]),
                                                    new State("Right", Pins[5]) });
        }

        private void InitGPIO()
        {
            
            var gpio = GpioController.GetDefault();

            //Show an error if there is no GPIO controller
            if (gpio == null)
            {
                txbDebug.Text = "There is no GPIO controller on this device";
                return;
            }

            //Initialize pins
            txbDebug.Text = $"GPIO pins ";

            Pins[0] = gpio.OpenPin(13);
            Pins[1] = gpio.OpenPin(19);
            Pins[2] = gpio.OpenPin(20);
            Pins[3] = gpio.OpenPin(21);
            Pins[4] = gpio.OpenPin(24);
            Pins[5] = gpio.OpenPin(26);

            txbDebug.Text += "initialized properly";

            /*  
                    InitPin(tmpPin, pair, item);
                pins[pin].SetDriveMode(GpioPinDriveMode.Output);
                txbDebug.Text += $"{pin} ";
                tmpPin++;
            */
        }
        
        private void Timer_Tick(object sender, object e)
        {
            if (Gamepad.Gamepads.Count > 0)
            {
                if (Gamepad.Gamepads[0] != null)
                {
                    controller = Gamepad.Gamepads[0];
                }
                var reading = controller.GetCurrentReading();
                txbDebug.Text = $"{reading} pressed";

                //pbLeftThumbstickX.Value = reading.LeftThumbstickX;
                //pbLeftThumbstickY.Value = reading.LeftThumbstickY;

                //https://msdn.microsoft.com/en-us/library/windows/apps/windows.gaming.input.gamepadbuttons.aspx
                //ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.A), lblA);
            }
        }
        
        private void ChangeVisibility(bool flag, UIElement elem)
        {
            if (flag)
            {
                elem.Visibility = Visibility.Visible;
            }
            else
            {
                elem.Visibility = Visibility.Collapsed;
            }
        }
    }
}
