using System;
namespace RedLionClockReader
{
    public interface IClockReader : IDisposable
    {
        decimal Read();
    }
}
