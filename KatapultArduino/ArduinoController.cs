using System;
using System.IO.Ports;
using System.Windows;
// Zorg ervoor dat je deze namespace toevoegt

namespace Arduino
{
    public class ArduinoController
    {
        private SerialPort arduinoPort;

        public ArduinoController(string portName, int baudRate)
        {
            try
            {
                arduinoPort = new SerialPort(portName, baudRate);
                arduinoPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het openen van de poort. Is de poort correct aangesloten?: {ex.Message}", "Fout");
                Environment.Exit(0);
            }
        }

        public bool IsOpen => arduinoPort?.IsOpen == true;

        public void SendCommand(string command)
        {
            if (IsOpen)
            {
                try
                {
                    arduinoPort.WriteLine(command);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fout bij verzenden: {ex.Message}", "Fout");
                    Environment.Exit(0);
                }
            }
            else
            {
                MessageBox.Show("De seriële poort is niet open.", "Fout");
                Environment.Exit(0);
            }
        }
    }
}
