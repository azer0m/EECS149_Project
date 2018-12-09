using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class readKinect : MonoBehaviour {
    String posData;
    float X;
    float Y;
    float Z;
    private const int listenPort = 9300;
    //IPAddress comefrom = IPAddress.Parse("127.0.1.1");
    private void StartListener()

    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Loopback, listenPort);

        try
        {
            //    Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);
                posData = $" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");
                string[] posXYZ = posData.Split(' ');
                X = Convert.ToSingle(posXYZ[0]);
                Y = Convert.ToSingle(posXYZ[1]);
                Z = Convert.ToSingle(posXYZ[2]);
                transform.position = new Vector3(X, Y, Z);
            //Console.WriteLine("Received broadcast from {groupEP} :");
            //Console.WriteLine(" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}")
        }
        catch (SocketException e)
        {
            //Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        StartListener();
    }
}
