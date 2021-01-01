using System.Collections.Generic;

namespace UServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Commands.Start();
            Commands.Register(new Command()
            {
                name = "ExitApplication",
                prefix = "exit",
                pathToCode = "Resources/Scripts/ExitApplication.cs"
            });
            Commands.Register(new Command()
            {
                name = "Server",
                prefix = "server",
                flags = new List<Flag>
                {
                    new Flag(){ name = "start", args = new[] { "port", "loopback" } },
                    new Flag(){ name = "stop" },
                    new Flag(){ name = "restart" },
                },
                pathToCode = "Resources/Scripts/Server.cs"
            });
        }
    }
}
