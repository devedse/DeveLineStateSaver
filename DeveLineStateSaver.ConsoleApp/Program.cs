using System;

namespace DeveLineStateSaver.ConsoleApp
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var proc = new AProcessor();
            proc.Go();

            Console.WriteLine("Program completed, press any key to continue...");
            Console.ReadKey();
        }
    }
}