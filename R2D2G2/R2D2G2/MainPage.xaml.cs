using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using System.Diagnostics;

namespace R2D2G2
{

    public sealed partial class MainPage : Page
    {
        GpioPin pin0;
        GpioPinValue pinValue;
        static int relay0 = 31;
        static int relay1 = 32;
        static int relay2 = 33;
        static int relay3 = 35;
        static int relay4 = 36;
        static int relay5 = 37;
        static int relay6 = 38;
        static int relay7 = 40;
        DispatcherTimer timer;

        public MainPage()
        {
            this.InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            InitGPIO();
            if (pin0 != null)
            {
                timer.Start();
            }
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                pin0 = null;
                Debug.WriteLine("There is no GPIO controller on this device");
                return;
            }

            pin0 = gpio.OpenPin(relay0);
            pinValue = GpioPinValue.High;
            pin0.Write(pinValue);
            pin0.SetDriveMode(GpioPinDriveMode.Output);

            Debug.WriteLine("GPIO pin initialized correctly");
        }

        private void Timer_Tick(object sender, object e)
        {
            if (pinValue == GpioPinValue.High)
            {
                pinValue = GpioPinValue.Low;
                pin0.Write(pinValue);
                Debug.WriteLine("Low");
            }
            else
            {
                pinValue = GpioPinValue.High;
                pin0.Write(pinValue);
                Debug.WriteLine("High");
            }
        }
    }
}
