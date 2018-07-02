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
        IReadOnlyList<Gamepad> gamepads;
        Gamepad gamepad;
        Motor lLeg;
        Motor rLeg;
        Motor head;
        public int[] pinNums = { 6, 12, 19, 16, 26, 20 };
        public GpioPin[] Pins { get; set; } = new GpioPin[6];

        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;

            InitGPIO();
            InitMotors();

            gamepads = Gamepad.Gamepads;

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
            //Show an error if there is no GPIO controller
            if (GpioController.GetDefault() == null)
            {
                txbDebug.Text = "There is no GPIO controller on this device";
                return;
            }

            var gpio = GpioController.GetDefault();

            //Initialize pins
            txbDebug.Text = $"GPIO pins ";

            for (int i = 0; i < Pins.Length; i++)
            {
                Pins[i] = gpio.OpenPin(pinNums[i]);
                Pins[i].SetDriveMode(GpioPinDriveMode.Output);
                txbDebug.Text += $"i ";
            }

            txbDebug.Text += "initialized properly";
        }
        
        private void Timer_Tick(object sender, object e)
        {
            if (gamepads.Count > 0)
            {
                if (gamepads[0] != null)
                {
                    gamepad = Gamepad.Gamepads[0];
                }
                var reading = gamepad.GetCurrentReading();
                //txbDebug.Text = $"{reading} pressed";

                //pbLeftThumbstickX.Value = reading.LeftThumbstickX;
                //pbLeftThumbstickY.Value = reading.LeftThumbstickY;

                //https://msdn.microsoft.com/en-us/library/windows/apps/windows.gaming.input.gamepadbuttons.aspx
                //ChangeVisibility(reading.Buttons.HasFlag(GamepadButtons.A), lblA);

                /*if (Pins[0].Read() == GpioPinValue.Low)
                {
                    Pins[0].Write(GpioPinValue.High);
                }
                else
                {
                    Pins[0].Write(GpioPinValue.Low);
                }*/
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Pins[Convert.ToInt32(Name)].Write(GpioPinValue.High);
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
#if RELEASE
            this.Visibility = Visibility.Collapsed;
#endif
        }
    }
}
