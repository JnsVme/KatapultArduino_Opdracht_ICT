using System;
using System.IO.Ports;
using System.Windows;

namespace Arduino
{
    public class ArduinoController  //klasse om arduino te lezen op COMPOORT
    {
        private SerialPort arduinoPort;

        public ArduinoController(string portName, int baudRate)
        {
            try
            {
                arduinoPort = new SerialPort(portName, baudRate);    // try and catch vond ik het best om toe te passen, ook al was dit niet nodig volgens de opdrachtbeschrijving.
                arduinoPort.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het openen van de poort. Is de poort correct aangesloten?: {ex.Message}", "Fout");
                Environment.Exit(0);
            }
        }

        public bool IsOpen => arduinoPort?.IsOpen == true;     // ? betekent dat de waarde van arduinoPort gecheckt wordt, en als deze niet null is, wordt de waarde van IsOpen gecheckt.

        public void SendCommand(string command) // methode om commando's te sturen naar de arduino
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
