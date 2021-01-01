using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace UServer
{
    public class Commands
    {
        private static string commandPath = Path.Combine(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "commands.usettings");

        public static List<Command> commands = new List<Command>();
        public static bool running;
        private static Assembly _assembly;
        private static Thread _thread;

        public static void Start()
        {
            running = true;
            LoadCommands();
            _thread = new Thread(new ThreadStart(Run));            
            _thread.Start();
        }

        private static void LoadCommands()
        {
            if (!File.Exists(commandPath)) return;
            var array = File.ReadAllBytes(commandPath);
            using (var ms = new MemoryStream(array))
            {
               commands =  Serializer.DeSerialize<List<Command>>(ms);
            }
            var files = new List<string>();
            commands.ForEach(x => files.Add(x.pathToCode));
            _assembly = UCompiler.Compile(files);
        }

        public static void Stop()
        {
            running = false;
            _thread.Abort(null);
        }

        private static void SaveCommands()
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(commands, ms);
                var array = ms.ToArray();
                using (var fs = File.Open(commandPath, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(array, 0, array.Length);
                }
            }
            LoadCommands();
        }

        internal static void Register(Command command)
        {
            if (commands.Find(x => x.prefix == command.prefix) != null) return;

            commands.Add(command);
            SaveCommands();
        }

        private static void Run()
        {
            while (running)
            {
                var userinput = Console.ReadLine();
                var userinputarray = userinput.ToCharArray();


                List<string> parts = new List<string>();
                var sb = new StringBuilder();
                for (int a = 0; a < userinputarray.Length; a++)
                {
                    if(userinputarray[a].Equals(' '))
                    {
                        parts.Add(sb.ToString());
                        sb = new StringBuilder();
                        continue;
                    }
                    sb.Append(userinputarray[a]);
                    if (a >= userinputarray.Length - 1)
                    {
                        parts.Add(sb.ToString());
                        sb = new StringBuilder();
                    }
                }

                var platform = parts[0];
                parts.RemoveAt(0);
                var cmd = parts[0];
                var firstletter = cmd.ToCharArray()[0].ToString().ToUpper();
                sb = new StringBuilder();
                for (int i = 1; i < cmd.ToCharArray().Length; i++) sb.Append(cmd.ToCharArray()[i]);
                cmd = $"{firstletter }{sb}";
                parts.RemoveAt(0);

                var keyvalue = parts;


                var command = commands.Find(x => x.prefix.ToLower() == platform.ToLower());
                if (command == null) continue;

                UCompiler.GetMethod(_assembly, command, cmd, command.Args(cmd, keyvalue))?.Invoke();
            }
        }
    }
}