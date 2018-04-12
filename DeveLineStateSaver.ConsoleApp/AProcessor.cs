using System;
using System.Threading;

namespace DeveLineStateSaver.ConsoleApp
{
    public class AProcessor
    {
        private LineStateSaver _lineStateSaver;

        public AProcessor()
        {
            _lineStateSaver = new LineStateSaver("linestatedata.json");
        }

        public void Go()
        {
            //_lineStateSaver.Clear();

            var res1 = _lineStateSaver.Save(() => LongRunningOperation(12));
            var res2 = _lineStateSaver.Save(() => LongRunningOperation(12));
            var res3 = _lineStateSaver.Save(() => LongRunningOperation(18));
            var res4 = _lineStateSaver.Save(() => LongRunningOperation(20));
            var res5 = _lineStateSaver.Save(() => LongRunningOperation(500));


            var complexObject = new ComplexObject()
            {
                Age = new DateTime(2018, 3, 4),
                Name = "Devedse",
                Timer = 1000
            };

            var complexObject2 = new ComplexObject()
            {
                Age = new DateTime(2018, 3, 4),
                Name = "Devedse2",
                Timer = 1000
            };

            var comp1 = _lineStateSaver.Save(() => LongRunningOperationComplex(complexObject));
            var comp2 = _lineStateSaver.Save(() => LongRunningOperationComplex(complexObject));
            var comp3 = _lineStateSaver.Save(() => LongRunningOperationComplex(complexObject));
            var comp4 = _lineStateSaver.Save(() => LongRunningOperationComplex(complexObject2));
            var comp5 = _lineStateSaver.Save(() => LongRunningOperationComplex(complexObject2));

            Console.WriteLine();

            
        }


        private int LongRunningOperation(int input)
        {
            Console.WriteLine($"Executing long running operation for input: {input}");
            Thread.Sleep(5000);
            var retval = 15 * input;
            Console.WriteLine($"Returning long running operation for input: {input} with retval: {retval}");
            return retval;
        }

        private ComplexObject LongRunningOperationComplex(ComplexObject input)
        {
            Console.WriteLine($"Complex executing long running operation for input: {input}");
            Thread.Sleep(5000);

            var retval = new ComplexObject()
            {
                Age = input.Age + TimeSpan.FromDays(15),
                Name = input.Name + " extended",
                Timer = input.Timer - 5
            };
            Console.WriteLine($"Complex returning long running operation for input: {input} with retval: {retval}");
            return retval;
        }
    }
}
