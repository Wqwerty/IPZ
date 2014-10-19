using System;
using System.IO.Ports;
using System.Threading;

namespace Komunikacja
{
    class COM
    {
        static bool _continue;
        static SerialPort _serialPort;

        string name;
        string message;

        string dataBits = "8";
        string parity = "none";
        string stopBits = "1";
        string handshake = "none";
        string baudRate = "9600";

        public void initialization()
        {
            _serialPort = new SerialPort();

            // StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            //Thread readThread = new Thread(Read);

            _serialPort.BaudRate = int.Parse(baudRate);
            _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), parity, true);
            _serialPort.DataBits = int.Parse(dataBits.ToUpperInvariant());
            _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
            _serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
            _serialPort.PortName = SetPortName();

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;
            _serialPort.NewLine = "B";
            _serialPort.Open();
        }

        public void close()
        {
            _serialPort.Close();
        }
        public int[] Read()
        {
            int[] tab = new int[0];
            int[] tab1 = new int[200000];
            int temp;
            int i = 1;
            _continue = true;
            Send(tab, 1);
            while (_continue)
            {
                try
                {
                    //temp = _serialPort.ReadByte();
                    //Console.WriteLine(temp);

                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);

                }
                catch (TimeoutException) { }
            }
            return tab;
        }

        // Display Port values and prompt user to enter a port.
        public static string SetPortName()
        {
            string portName = "Nie ma urządzenia";
            int iloscZapytan = 0;
            int odpowiedz = 0;

            foreach (string s in SerialPort.GetPortNames())
            {
                _continue = true;
                _serialPort.PortName = s;
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;

                if (s != "COM4") //do wyrzucenia, uzywane przy termianalu
                {
                    _serialPort.Open();
                    Console.WriteLine(s);
                    _serialPort.WriteLine("$FF$01$AF$00$0A");
                    while (_continue)
                    {
                        try
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                odpowiedz += _serialPort.ReadByte();
                            }

                            if (odpowiedz == 335)
                            {
                                portName = s;
                            }
                        }
                        catch (TimeoutException) { }
                        iloscZapytan++;
                        if (iloscZapytan == 5)
                        {
                            iloscZapytan = 0;
                            _continue = false;
                        }
                        Console.WriteLine(portName);
                    }
                    _serialPort.Close();
                }
            }
            return portName;
        }

        public int Send(int[] set, int nrCommand)
        {
            _serialPort.WriteLine("$FF$01$AF$00$0A");
            _serialPort.WriteLine(Convert.ToString(nrCommand));
            for (int i = 0; i < set.Length; i++)
            {
                _serialPort.WriteLine(Convert.ToString(set[i]));
            }
            return 0;
        }
    }
}