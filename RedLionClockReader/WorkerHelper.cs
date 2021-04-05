using System;
using System.Threading.Tasks;

namespace ClearEye.RedLionClockReader
{
    public class WorkerHelper
    {

        private readonly IClockReader clockReader;
        private readonly IValueSender valueSender;

        public WorkerHelper(IClockReader clockReader, IValueSender valueSender)
        {
            this.clockReader = clockReader;
            this.valueSender = valueSender;
        }

        public decimal GetValue()
        {
            return clockReader.Read();
        }

        public async Task PostValue(decimal value)
        {
            await valueSender.SendValue(value);
        }

    }
}