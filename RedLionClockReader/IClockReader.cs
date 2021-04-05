using System;
namespace ClearEye.RedLionClockReader
{
    public interface IClockReader : IDisposable
    {
        decimal Read();
    }
}
