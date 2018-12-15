//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//public class UDPListener
//{
//    private const int listenPort = 9300;
//    IPAddress comefrom = IPAddress.Parse("127.0.1.1");


//    private static void StartListener()
//    {
//        IPEndPoint localpt = new IPEndPoint(IPAddress.Any, listenPort);
//        //UdpClient listener = new UdpClient(localpt);
//        UdpClient listener2 = new UdpClient();
//        listener2.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
//        listener2.Client.Bind(localpt);

//        try
//        {
//            Console.WriteLine("Waiting for broadcast");
//            Console.WriteLine("Made it here");
//            byte[] bytes = listener2.Receive(ref localpt);
//            Console.WriteLine($"Received broadcast from {localpt} :");
//            Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
//            Console.WriteLine("Made it here");
//        }
//        catch (SocketException e)
//        {
//            Console.WriteLine(e);
//        }
//        finally
//        {
//            listener2.Close();
//        }
//    }

//    public static void Main()
//    {
//        StartListener();
//    }
//}



////IPEndPoint localpt = new IPEndPoint(IPAddress.Any, 6000);

//////Failed try
////    try
////    {
////        var u = new UdpClient(5000);
////u.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

////        UdpClient u2 = new UdpClient(5000);//KABOOM
////u2.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
////    }
////    catch (Exception)
////    {
////        Console.WriteLine("ERROR! You must call Bind only after setting SocketOptionName.ReuseAddress. \n And you must not pass any parameter to UdpClient's constructor or it will call Bind.");
////    }


////// ******************************************************************************************
//////This is how you do it (kudos to sipwiz)
////    UdpClient udpServer = new UdpClient(localpt); //This is what the proprietary(see question) sender would do (nothing special) 

//////!!! The following 3 lines is what the poster needs...(and the definition of localpt (of course))
////UdpClient udpServer2 = new UdpClient();
////udpServer2.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
////udpServer2.Client.Bind(localpt);


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class UDPListener
{
    private const int listenPort = 9300;
    IPAddress comefrom = IPAddress.Parse("127.0.1.1");

    private static void StartListener()
    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Loopback, listenPort);

        try
        {
            while (true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                Console.WriteLine($"Received broadcast from {groupEP} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }

    public static void Main()
    {
        StartListener();
    }
}