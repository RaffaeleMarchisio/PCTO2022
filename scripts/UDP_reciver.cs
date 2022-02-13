using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System;

public class UDP_reciver : MonoBehaviour
{
    const int PORT = 4124;
    const int NUM_FLOAT_IN_DATAGRAMS = 6;
    static UdpClient udp;
    Thread thread;
    [HideInInspector] public bool isGameOver;
    float[] floatData = new float[NUM_FLOAT_IN_DATAGRAMS];
    float xPolsoSx, yPolsoSx, xPolsoDx, yPolsoDx;


    void Start()
    {
        isGameOver = false;
        thread = new Thread(new ThreadStart(ThreadMethod));
        //start the thread
        thread.Start();
    }
    private void ThreadMethod()
    {
        IPEndPoint ip_porta = new IPEndPoint(IPAddress.Any, PORT);
        udp = new UdpClient(ip_porta);
        while (!isGameOver)    //for verify the game over
        {
            //Represents a network endpoint as an IP address and port number
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, PORT);

            //returns a UDP datagram that was sent from a remote host
            byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);

            for (int j = 0; j < NUM_FLOAT_IN_DATAGRAMS; j++)
                floatData[j] = BitConverter.ToSingle(receiveBytes, sizeof(Single) * j);
            xPolsoDx = floatData[2];
            yPolsoDx = -floatData[3];
            xPolsoSx = floatData[4];
            yPolsoSx = -floatData[5];
        }
        udp.Close();        //close the socket
        udp.Dispose();
    }

    //methods for get the coordinates of the wrists

    public float getxPolsoDx()
    {
        return xPolsoDx;
    }

    public float getyPolsoDx()
    {
        return yPolsoDx;
    }

    public float getxPolsoSx()
    {
        return xPolsoSx;
    }

    public float getyPolsoSx()
    {
        return yPolsoSx;
    }

}