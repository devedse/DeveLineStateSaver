using System;
using Xunit;

namespace DeveLineStateSaver.Tests
{
    public class LineStateSaverTester
    {
        [Fact]
        public void OnlyCallsMethodOnceForSimpleObject()
        {
            var testCounter = new TestCounter();

            var lss = new LineStateSaver();
            lss.Save(() => testCounter.SimpleCall(15));
            lss.Save(() => testCounter.SimpleCall(15));
            lss.Save(() => testCounter.SimpleCall(15));

            Assert.Equal(1, testCounter.CallCount);
        }

        [Fact]
        public void OnlyCallsMethodOnceForComplexObject()
        {
            var testCounter = new TestCounter();

            var complexObject = new ComplexObjectTest()
            {
                Age = new DateTime(2018, 5, 2),
                Name = "Devedse",
                Timer = 600
            };

            var lss = new LineStateSaver();
            lss.Save(() => testCounter.ComplexCall(complexObject));
            lss.Save(() => testCounter.ComplexCall(complexObject));
            lss.Save(() => testCounter.ComplexCall(complexObject));

            Assert.Equal(1, testCounter.CallCount);
        }
    }
}
