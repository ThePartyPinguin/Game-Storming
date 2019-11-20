using System;
using System.Net.Sockets;
using GameFrame.Networking.NetworkConnector;

sealed class TcpNetworkSender<TEnum> : NetworkSender<TEnum> where TEnum : Enum
{
    private readonly TcpClient _tcpClient;

    private NetworkStream _networkStream;
    public TcpNetworkSender(INetworkMessageSerializer<TEnum> networkMessageSerializer, TcpClient tcpClient) : base(networkMessageSerializer)
    {
        _tcpClient = tcpClient;
    }

    protected override void SendMessage(byte[] data)
    {
        if (_networkStream == null)
            _networkStream = _tcpClient.GetStream();
        if (data == null || data.Length <= 0)
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
