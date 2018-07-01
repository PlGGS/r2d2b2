using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Gpio;
using System.Threading.Tasks;

namespace R2D2G2
{

    public sealed partial class MainPage : Page
    {
        GpioPin[] pins = new GpioPin[6];
        int[] relayValues = new int[6] { 13, 19, 16, 26, 20, 21 };
        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            InitGPIO();
            timer.Start();
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

            for (int i = 0; i < pins.Length; i++)
            {
                pins[i] = gpio.OpenPin(relayValues[i]);
                pins[i].SetDriveMode(GpioPinDriveMode.Output);
                pins[i].Write(GpioPinValue.High);
                txbDebug.Text += $"{i} ";
            }

            txbDebug.Text += "initialized properly";
        }

        private void Timer_Tick(object sender, object e)
        {
            for (int i = 0; i < pins.Length; i++)
            {
                if (pins[i].Read() == GpioPinValue.High)
                {
                    pins[i].Write(GpioPinValue.Low);
                }
                else
                {
                    pins[i].Write(GpioPinValue.High);
                }

                Task.Delay(100).Wait();
            }
        }
    }
}
