﻿using System;
using System.Net.Sockets;
using GameFrame.Networking.NetworkConnector;
using UnityEngine;

sealed class TcpNetworkSender<TEnum> : NetworkSender<TEnum> where TEnum : Enum
{
    private readonly TcpClient _tcpClient;

    private NetworkStream _networkStream;
    private Action _onConnectionLost;
    public TcpNetworkSender(INetworkMessageSerializer<TEnum> networkMessageSerializer, TcpClient tcpClient, Action onConnectionLost) : base(networkMessageSerializer)
    {
        _tcpClient = tcpClient;
        _onConnectionLost = onConnectionLost;
    }

    protected override void Setup()
    {
        _networkStream = _tcpClient.GetStream();
        base.Setup();
    }

    protected override void SendMessage(byte[] data)
    {

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
