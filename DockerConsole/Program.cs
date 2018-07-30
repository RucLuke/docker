using System;
using System.Threading;
namespace DockerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World From Docker!");
            System.Threading.Thread.Sleep(Timeout.Infinite);
        }
    }
}
