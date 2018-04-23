using System;
using System.Threading.Tasks;

namespace DeveLineStateSaver.Tests
{
    public class TestCounter
    {
        public int CallCount { get; private set; }
        public int UnrelatedCallCount { get; private set; }
        public int DoubleParametersCallCount { get; private set; }

        public int SimpleCall(int input)
        {
            CallCount++;
            return input * 2;
        }

        public async Task<int> SimpleCallAsync(int input)
        {
            await Task.Delay(1);
            CallCount++;
            return input * 2;
        }

        public int SimpleCall(int input, int input2)
        {
            DoubleParametersCallCount++;
            return input + input2;
        }

        public async Task<int> SimpleCallAsync(int input, int input2)
        {
            await Task.Delay(1);
            DoubleParametersCallCount++;
            return input + input2;
        }

        public int SimpleUnrelatedCall(int input)
        {
            UnrelatedCallCount++;
            return input;
        }

        public async Task<int> SimpleUnrelatedCallAsync(int input)
        {
            await Task.Delay(1);
            UnrelatedCallCount++;
            return input;
        }

        public ComplexObjectTest ComplexCall(ComplexObjectTest input)
        {
            CallCount++;

            var retval = new ComplexObjectTest()
            {
                Age = input.Age.AddDays(5),
                Name = input.Name + "blah",
                Timer = input.Timer + 100
            };

            return retval;
        }

        public async Task<ComplexObjectTest> ComplexCallAsync(ComplexObjectTest input)
        {
            await Task.Delay(1);
            CallCount++;

            var retval = new ComplexObjectTest()
            {
                Age = input.Age.AddDays(5),
                Name = input.Name + "blah",
                Timer = input.Timer + 100
            };

            return retval;
        }

        public ComplexObjectTest ComplexCall(ComplexObjectTest input, ComplexObjectTest input2)
        {
            DoubleParametersCallCount++;

            var retval = new ComplexObjectTest()
            {
                Age = input.Age.AddDays(5).AddDays(input2.Age.Day),
                Name = input.Name + "blah" + input2.Name,
                Timer = input.Timer + 100 + input2.Timer
            };

            return retval;
        }

        public async Task<ComplexObjectTest> ComplexCallAsync(ComplexObjectTest input, ComplexObjectTest input2)
        {
            await Task.Delay(1);
            DoubleParametersCallCount++;

            var retval = new ComplexObjectTest()
            {
                Age = input.Age.AddDays(5).AddDays(input2.Age.Day),
                Name = input.Name + "blah" + input2.Name,
                Timer = input.Timer + 100 + input2.Timer
            };

            return retval;
        }

        public ComplexObjectTest ComplexUnrelatedCall(ComplexObjectTest input)
        {
            UnrelatedCallCount++;
            return input;
        }

        public async Task<ComplexObjectTest> ComplexUnrelatedCallAsync(ComplexObjectTest input)
        {
            await Task.Delay(1);
            UnrelatedCallCount++;
            return input;
        }

        public object ObjectCall(Action input)
        {
            return input;
        }
    }
}
