using System;

namespace UDP
{
    class Program
    {
        static void Main(string[] args)
        {

            //UDPSocket s = new UDPSocket();
            //s.Server("127.0.0.1", 9100);

            UDPSocket c = new UDPSocket();
            c.Client("127.0.0.1", 9100);
            while (true)
            {
                Console.ReadKey();
                c.Send("AhliWasahli!");
                c.Receive();
            }

            //c.Send("TEST!");


        }

        //static void Main(string[] args)
        //{
        //    while(true)
        //    {
        //        UDPSocket s = new UDPSocket();
        //        s.Server("127.0.0.1", 9100);

        //        UDPSocket c = new UDPSocket();
        //        c.Client("127.0.0.1", 9100);
        //        c.Send("TEST!!");
        //    }

        //}
    }
}