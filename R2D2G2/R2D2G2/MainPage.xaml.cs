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
            //timer.Start();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            pins[4].Write(GpioPinValue.Low);
            pins[2].Write(GpioPinValue.Low);

            if (pins[5].Read() == GpioPinValue.High && pins[3].Read() == GpioPinValue.High)
            {
                pins[5].Write(GpioPinValue.Low);
                pins[3].Write(GpioPinValue.Low);
            }
            else
            {
                pins[5].Write(GpioPinValue.High);
                pins[3].Write(GpioPinValue.High);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            pins[5].Write(GpioPinValue.Low);
            pins[3].Write(GpioPinValue.Low);

            if (pins[4].Read() == GpioPinValue.High && pins[2].Read() == GpioPinValue.High)
            {
                pins[4].Write(GpioPinValue.Low);
                pins[2].Write(GpioPinValue.Low);
            }
            else
            {
                pins[4].Write(GpioPinValue.High);
                pins[2].Write(GpioPinValue.High);
            }
        }
    }
}
