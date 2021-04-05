using System;
using System.Threading.Tasks;

namespace RedLionClockReader
{
    public interface IValueSender : IDisposable
    {
        Task SendValue(decimal value);
    }
}