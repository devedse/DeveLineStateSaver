using System;

namespace DeveLineStateSaver.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = new AProcessor();
            proc.Go();

            Console.WriteLine("Program completed, press any key to continue...");
            Console.ReadKey();
        }
    }
}
