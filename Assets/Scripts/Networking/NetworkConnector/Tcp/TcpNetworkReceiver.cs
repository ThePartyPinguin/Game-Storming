using System;
using System.Net.Sockets;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;

sealed class TcpNetworkReceiver<TEnum> : NetworkReceiver<TEnum> where TEnum : Enum
{
    private readonly TcpClient _tcpClient;

    private NetworkStream _networkStream;

    private Action _onConnectionLost;
    public TcpNetworkReceiver(TcpClient tcpClient, Action onConnectionLost)
    {
        _tcpClient = tcpClient;
        _onConnectionLost = onConnectionLost;
    }

    protected override byte[] ReceiveData()
    {
        if (_networkStream == null)
            _networkStream = _tcpClient.GetStream();
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
            _onConnectionLost?.Invoke();
            throw;
        }
    }
}
