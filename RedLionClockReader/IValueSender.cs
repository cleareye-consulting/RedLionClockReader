using System;
using System.Threading.Tasks;

namespace ClearEye.RedLionClockReader
{
    public interface IValueSender : IDisposable
    {
        Task SendValue(decimal value);
    }
}