using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpNetworkSender : NetworkSender
{
    private readonly TcpClient _tcpClient;

    private readonly NetworkStream _networkStream;
    public TcpNetworkSender(ISerializer serializer, TcpClient tcpClient) : base(serializer)
    {
        _tcpClient = tcpClient;
        _networkStream = tcpClient.GetStream();
    }

    protected override void SendMessage(byte[] data)
    {
        if(data == null || data.Length <= 0)
            return;
        
        try
        {
            _networkStream.Write(data, 0, data.Length);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
