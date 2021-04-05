using System;
namespace ClearEye.RedLionClockReader
{
    public static class ClockReaderFactory
    {
        public static IClockReader GetClockReader(string clockModel, int clockAddress)
        {
            if (clockModel == "GEMINI 2000")
            {
                return new Gemini2000ClockReader(clockAddress);
            }
            throw new NotSupportedException($"Clock '{clockModel}' is not currently supported");
        }
    }
}