using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Arduino;

namespace KatapultArduino
{
    public partial class MainWindow : Window
    {
        private ArduinoController arduinoController;
        private bool isReady = false;
        private DispatcherTimer countdownTimer;
        private int countdownValue;

        public MainWindow()
        {
            InitializeComponent();
            arduinoController = new ArduinoController("COM4", 9600);
            ResetArduino();
            Servoslider.IsEnabled = false;

            countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            countdownTimer.Tick += CountdownTimer_Tick;
        }

        private void Servo1Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (arduinoController.IsOpen && isReady)
            {
                int angle = (int)((Slider)sender).Value;
                arduinoController.SendCommand($"M{angle}");
                int tandwiel = (12 * (angle - 90)) / 56;
                lblslider.Content = $"{tandwiel}°";
            }
            else
            {
                lbl.Content = "Niet gereed";
            }
        }

        private void Vuurknop_Click(object sender, RoutedEventArgs e)
        {
            if (arduinoController.IsOpen && isReady)
            {
                StartCountdown();
                Servoslider.IsEnabled = false;
            }
            else
            {
                lbl.Content = "Niet gereed";
            }
        }

        private void StartCountdown()
        {
            countdownValue = 3;
            lbl.Content = countdownValue.ToString();
            countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            countdownValue--;
            lbl.Content = countdownValue.ToString();

            if (countdownValue == 0)
            {
                countdownTimer.Stop();
                lbl.Content = "FIRE";
                arduinoController.SendCommand("B");

                isReady = false;
                ResetArduino();
            }
        }

        private void ResetArduino()
        {
            if (!isReady && arduinoController.IsOpen)
            {
                arduinoController.SendCommand("A");
                Servoslider.Value = 90;
                lblslider.Content = $"0°";
                DispatcherTimer resetTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(85)
                };
                resetTimer.Tick += (s, e) =>
                {
                    isReady = true;
                    resetTimer.Stop();
                    Servoslider.IsEnabled = true;
                    lbl.Content = "Ready?";
                };
                resetTimer.Start();
            }
        }
    }
}
