using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(isEven(2));
        }
        public static bool isEven(int num) => num % 2 == 0;
    }
}
