using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DeveLineStateSaver.Tests
{
    public class LineStateSaverAsyncTester
    {
        [Fact]
        public async void OnlyCallsMethodOncePerSimpleObject()
        {
            var testCounter = new TestCounter();

            var lss = new LineStateSaver();
            await lss.Save(() => testCounter.SimpleCallAsync(15));
            await lss.Save(() => testCounter.SimpleCallAsync(15));
            await lss.Save(() => testCounter.SimpleCallAsync(17));
            await lss.Save(() => testCounter.SimpleCallAsync(15));
            await lss.Save(() => testCounter.SimpleCallAsync(17));
            await lss.Save(() => testCounter.SimpleCallAsync(15, 17));
            await lss.Save(() => testCounter.SimpleUnrelatedCallAsync(17));

            Assert.Equal(2, testCounter.CallCount);
            Assert.Equal(1, testCounter.DoubleParametersCallCount);
            Assert.Equal(1, testCounter.UnrelatedCallCount);
        }

        [Fact]
        public async void OnlyCallsMethodOncePerComplexObject()
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
            await lss.Save(() => testCounter.ComplexCallAsync(complexObject1));
            await lss.Save(() => testCounter.ComplexCallAsync(complexObject1));
            await lss.Save(() => testCounter.ComplexCallAsync(complexObject2));
            await lss.Save(() => testCounter.ComplexCallAsync(complexObject1));
            await lss.Save(() => testCounter.ComplexCallAsync(complexObject1, complexObject2));
            await lss.Save(() => testCounter.ComplexUnrelatedCallAsync(complexObject2));

            Assert.Equal(2, testCounter.CallCount);
            Assert.Equal(1, testCounter.DoubleParametersCallCount);
            Assert.Equal(1, testCounter.UnrelatedCallCount);
        }

        [Fact]
        public async void CorrectlySerializesSimpleObject()
        {
            string fileName = $"{nameof(CorrectlySerializesSimpleObject)}.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var testCounter = new TestCounter();

            var lss1 = new LineStateSaver(fileName);
            var result1 = await lss1.Save(() => testCounter.SimpleCallAsync(16));

            var lss2 = new LineStateSaver(fileName);
            var result2 = await lss2.Save(() => testCounter.SimpleCallAsync(16));

            Assert.Equal(result1, result2);
            Assert.Equal(1, testCounter.CallCount);

            File.Delete(fileName);
        }

        [Fact]
        public async void CorrectlySerializesComplexObject()
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
            var result1 = await lss1.Save(() => testCounter.ComplexCallAsync(complexObject));

            var lss2 = new LineStateSaver(fileName);
            var result2 = await lss2.Save(() => testCounter.ComplexCallAsync(complexObject));

            Assert.Equal(result1, result2);
            Assert.Equal(1, testCounter.CallCount);

            File.Delete(fileName);
        }

        [Fact]
        public async void ClearsTheDataWhenCallingClearFunction()
        {
            var testCounter = new TestCounter();

            var lss = new LineStateSaver();
            await lss.Save(() => testCounter.SimpleCallAsync(15));

            lss.Clear();
            await lss.Save(() => testCounter.SimpleCallAsync(15));

            Assert.Equal(2, testCounter.CallCount);
        }

        [Fact]
        public async void ThrowsExceptionWhenNoMethodCallIsProvided()
        {
            var lss = new LineStateSaver();
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => lss.Save(() => new Task<int>(() => 3 + 5)));

            Assert.Contains("Could not cast function.body to type MethodCallExpression", ex.Message);
        }

        [Fact]
        public async void ThrowsExceptionWhenMethodCallIsProvidedWithNestedMethodCalls()
        {
            var testCounter = new TestCounter();

            var lss = new LineStateSaver();
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => lss.Save(() => testCounter.SimpleCallAsync(int.Parse("5"))));

            Assert.Contains("is not of type MemberExpression or ConstantExpression", ex.Message);
        }
    }
}