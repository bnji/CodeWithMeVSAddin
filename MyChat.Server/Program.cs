using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Spike;

namespace RtChat
{
    //class Program
    public class Chat
    {
        //static void Main(string[] args)
        public static void Listen(int port)
        {

            // Start listening on the port 8002
            Service.Listen(
                new TcpBinding(IPAddress.Any, port)//8002)
                );
        }
    }
}
