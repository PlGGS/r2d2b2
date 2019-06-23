using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Gpio;
using Windows.Gaming.Input;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Search;
using System.Threading.Tasks;

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
        Motor[] motors = new Motor[3];
        List<MediaElement> soundEffects;
        Random rnd = new Random();
        
        public MainPage()
        {
            this.InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;

            InitGPIO();
            InitMotors();
            InitSoundEffects(); //TODO check if R2D2G2 has internet to get the audio files and if not just load one sorta sad file

            gamepads = new List<Gamepad>();
            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            timer.Start();
        }

        private async void InitSoundEffects()
        {
            soundEffects = new List<MediaElement>();
            StorageFolder assetsFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            List<StorageFile> audioFiles = new List<StorageFile>();

            //Get all audio files in assets folder
            for (int f = 0; f < (await assetsFolder.GetFilesAsync(CommonFileQuery.OrderByName)).Count; f++)
            {
                StorageFile file = (await assetsFolder.GetFilesAsync(CommonFileQuery.OrderByName))[f];

                if (file.ContentType == "audio/wav" || file.ContentType == "audio/mp3")
                {
                    audioFiles.Add(file);
                    txbDebug.Text = file.Name;
                }
            }

            //Add audio files to soundEffects List
            for (int e = 0; e < audioFiles.Count; e++)
            {
                soundEffects.Add(new MediaElement());
                IRandomAccessStream stream = await audioFiles[e].OpenAsync(FileAccessMode.Read);
                soundEffects[e].SetSource(stream, audioFiles[e].ContentType);
            }
        }

        private void InitMotors()
        {
            head = new Head(gpioController);
            motors[0] = head;
            rLeg = new RightLeg(gpioController);
            motors[1] = rLeg;
            lLeg = new LeftLeg(gpioController);
            motors[2] = lLeg;
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
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            gamepads.Add(e);

            foreach (Motor motor in motors)
            {
                motor.Gamepads = gamepads;
            }
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            gamepads.Remove(e);

            foreach (Motor motor in motors)
            {
                motor.Gamepads = gamepads;
            }

            lLeg.SetState(LeftLeg.States.Off);
            rLeg.SetState(RightLeg.States.Off);
            head.SetState(Head.States.Off);
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
                
                //make head go left and right depending on which bumber is pressed
                if (reading.Buttons == GamepadButtons.LeftShoulder)
                {
                    head.SetState(Head.States.Left);
                }
                else if (reading.Buttons == GamepadButtons.RightShoulder)
                {
                    head.SetState(Head.States.Right);
                }
                else
                {
                    head.SetState(Head.States.Off);
                }

                //Audio playing section
                if (soundEffects.Count > 0)
                {
                    int r = rnd.Next(0, soundEffects.Count - 1);

                    if (reading.Buttons == GamepadButtons.A)
                    {
                        soundEffects[r].Play();
                        Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(soundEffects[r].NaturalDuration)));
                    }
                    else if (reading.Buttons == GamepadButtons.B)
                    {
                        soundEffects[r].Play();
                        Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(soundEffects[r].NaturalDuration)));
                    }
                    else if (reading.Buttons == GamepadButtons.X)
                    {
                        soundEffects[r].Play();
                        Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(soundEffects[r].NaturalDuration)));
                    }
                    else if (reading.Buttons == GamepadButtons.Y)
                    {
                        soundEffects[r].Play();
                        Task.Delay(TimeSpan.FromSeconds(Convert.ToDouble(soundEffects[r].NaturalDuration)));
                    }
                }
            }
        }
        
        
    }
}
