using System;
using System.Threading;
namespace UServer
{
    public class ExitApplication
    {
        public int time = 15;
        public void Execute()
        {
            Console.WriteLine("Application will close in " + time + "s");
            var line = Console.CursorTop;
            new Thread(() =>
            {
                Thread.Sleep(1000);
                while(time > 0)
                {
                    time--;
                    Console.CursorTop = line - 1;
                    Console.CursorLeft = 0;
                    Console.Write(new String(' ', Console.BufferWidth));
                    Console.CursorTop = line - 1;
                    Console.CursorLeft = 0;
                    Console.Write("Application will close in " + time + "s");
                    Thread.Sleep(1000);
                }
               
            }).Start();

            Commands.Stop();

            while(time > 0) {  }
            Environment.Exit(0);
        }
    }
}