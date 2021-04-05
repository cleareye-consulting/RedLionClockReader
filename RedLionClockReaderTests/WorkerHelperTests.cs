using System;
using System.Threading.Tasks;
using Xunit;
using ClearEye.RedLionClockReader;
using Moq;

namespace ClearEye.RedLionClockReaderTests
{
    public class WorkerHelperTests
    {
        [Fact]
        public async Task Main()
        {
            var clockReader = new Mock<IClockReader>();
            clockReader.Setup(cr => cr.Read()).Returns(12);
            var valueSender = new Mock<IValueSender>();
            valueSender.Setup(vs => vs.SendValue(It.IsAny<decimal>()));
            var helper = new WorkerHelper(clockReader.Object, valueSender.Object);
            var value = helper.GetValue();
            Assert.Equal(12, value);
            await helper.PostValue(value);
        }
    }
}
