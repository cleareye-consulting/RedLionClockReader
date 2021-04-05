using System;
using System.IO.Ports;
using RS232;

namespace RedLionClockReader
{
    public class Gemini2000ClockReader : IClockReader
    {

        private const string portName = "COM1";
        private const int baudRate = 2400;
        private const Parity parity = Parity.Odd;
        private const int dataBits = 7;
        private const StopBits stopBits = StopBits.One;
        private const bool isOverlappedRead = false;

        private readonly int clockAddress;
        private readonly ISerialConnection serialConnection;

        public Gemini2000ClockReader(int clockAddress)
        {
            this.clockAddress = clockAddress;
            serialConnection = SerialConnectionFactory.GetSerialConnection(portName, baudRate, parity, dataBits, stopBits, isOverlappedRead);
            serialConnection.Open();
        }

        public decimal Read()
        {
            serialConnection.Write($"N{clockAddress}TC*");
            var response = serialConnection.Read(19);
            var unitAddress = response.Substring(0, 2);
            if (int.Parse(unitAddress) != clockAddress)
            {
                throw new InvalidOperationException("Clock address on response doesn't match request");
            }
            if (!(response[2] == ' ' && response[3] == ' '))
            {
                throw new InvalidOperationException("Found value where blank space expected");
            }
            var mnemonic = response.Substring(4, 3);
            if (mnemonic != "CNT")
            {
                throw new InvalidOperationException($"Expected operation mnemonic 'CNT' but received '${mnemonic}'");
            }
            if (!(response[7] == ' '))
            {
                throw new InvalidOperationException("Found value where blank space expected");
            }
            //Not really clear what should be in 8 according to the docs
            char position9 = response[9];
            bool isNegative = position9 == '-' ? false : position9 == ' ' ? true : throw new InvalidOperationException($"Expected '-' or blank, got '{position9}");
            if (!(response[10] == ' '))
            {
                throw new InvalidOperationException("Found value where blank space expected");
            }
            var value = decimal.Parse(response.Substring(11, 6));
            if (isNegative)
            {
                value = value * -1;
            }
            if (!(response[17] == '\r' && response[18] == '\n'))
            {
                throw new InvalidOperationException("Did not find carriage return and line feed at end of transmission as expected");
            }
            return value;
        }

        public void Dispose()
        {
            serialConnection.Dispose();
        }
    }
}
