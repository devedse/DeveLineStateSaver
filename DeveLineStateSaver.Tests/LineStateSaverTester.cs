using System;
using System.IO;
using Xunit;

namespace DeveLineStateSaver.Tests
{
    public class LineStateSaverTester
    {
        [Fact]
        public void OnlyCallsMethodOncePerSimpleObject()
        {
            var testCounter = new TestCounter();

            var lss = new LineStateSaver();
            lss.Save(() => testCounter.SimpleCall(15));
            lss.Save(() => testCounter.SimpleCall(15));
            lss.Save(() => testCounter.SimpleCall(17));
            lss.Save(() => testCounter.SimpleCall(15));
            lss.Save(() => testCounter.SimpleCall(17));
            lss.Save(() => testCounter.SimpleUnrelatedCall(17));

            Assert.Equal(2, testCounter.CallCount);
        }

        [Fact]
        public void OnlyCallsMethodOncePerComplexObject()
        {
            var testCounter = new TestCounter();

            var complexObject1 = new ComplexObjectTest()
            {
                Age = new DateTime(2018, 5, 2),
                Name = "Devedse",
                Timer = 600
            };

            var complexObject2 = new ComplexObjectTest()
            {
                Age = new DateTime(2018, 5, 2),
                Name = "Devedse",
                Timer = 601
            };

            var lss = new LineStateSaver();
            lss.Save(() => testCounter.ComplexCall(complexObject1));
            lss.Save(() => testCounter.ComplexCall(complexObject1));
            lss.Save(() => testCounter.ComplexCall(complexObject2));
            lss.Save(() => testCounter.ComplexCall(complexObject1));
            lss.Save(() => testCounter.ComplexUnrelatedCall(complexObject2));

            Assert.Equal(2, testCounter.CallCount);
        }

        [Fact]
        public void CorrectlySerializesSimpleObject()
        {
            string fileName = $"{nameof(CorrectlySerializesSimpleObject)}.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var testCounter = new TestCounter();

            var lss1 = new LineStateSaver(fileName);
            var result1 = lss1.Save(() => testCounter.SimpleCall(16));
            
            var lss2 = new LineStateSaver(fileName);
            var result2 = lss2.Save(() => testCounter.SimpleCall(16));

            Assert.Equal(result1, result2);
            Assert.Equal(1, testCounter.CallCount);

            File.Delete(fileName);
        }

        [Fact]
        public void CorrectlySerializesComplexObject()
        {
            string fileName = $"{nameof(CorrectlySerializesComplexObject)}.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var testCounter = new TestCounter();

            var complexObject = new ComplexObjectTest()
            {
                Age = new DateTime(2014, 1, 6),
                Name = "Devedse123",
                Timer = 400
            };

            var lss1 = new LineStateSaver(fileName);
            var result1 = lss1.Save(() => testCounter.ComplexCall(complexObject));

            var lss2 = new LineStateSaver(fileName);
            var result2 = lss2.Save(() => testCounter.ComplexCall(complexObject));

            Assert.Equal(result1, result2);
            Assert.Equal(1, testCounter.CallCount);

            File.Delete(fileName);
        }
    }
}
