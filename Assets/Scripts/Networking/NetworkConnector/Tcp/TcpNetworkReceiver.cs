using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TcpNetworkReceiver : NetworkReceiver
{
    private readonly TcpClient _tcpClient;

    private readonly NetworkStream _networkStream;

    public TcpNetworkReceiver(ISerializer serializer, TcpClient tcpClient) : base(serializer)
    {
        _tcpClient = tcpClient;
        _networkStream = _tcpClient.GetStream();
    }

    protected override byte[] ReceiveData()
    {
        try
        {
            int dataAvailable = _tcpClient.Available;
            
            if (dataAvailable > 0)
            {
                byte[] buffer = new byte[dataAvailable];
                _networkStream.Read(buffer, 0, buffer.Length);
                return buffer;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
