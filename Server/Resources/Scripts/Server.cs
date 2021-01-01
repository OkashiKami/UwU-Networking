using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UServer
{
    public class Server
    {
        private int port;

        public void Start(params KVP[] args)
        {
            var list = args.ToList();

            port = int.Parse(list.Find(x => x.Key == "port").Value);


        }
        public void Stop()
        {

        }

    }
}
