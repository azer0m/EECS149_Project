using System;

namespace UDP
{
    class Program
    {
        static void Main(string[] args)
        {
            //UDPSocket s = new UDPSocket();
            //s.Server("127.0.0.1", 27000);

            UDPSocket c = new UDPSocket();
            c.Client("localhost", 9100);
            c.Send("TEST!");

            Console.ReadKey();
        }
    }
}