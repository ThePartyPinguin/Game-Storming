﻿using System;
using GameFrame.Networking.Serialization;

public class ServerSettings<TEnum> where TEnum : Enum
{
    public TEnum ClientToServerHandshakeEvent { get; set; }

    public TEnum ServerToClientHandshakeEvent { get; set; }

    public TEnum ClientDisconnectEvent { get; set; }
    public TEnum ServerDisconnectEvent { get; set; }
    public int TcpPort { get; set; } = 5555;

    public int UdpReceivePort { get; set; } = 12000;
    public int UdpRemoteSendPort { get; set; } = 11000;

    public int MaxConnectedClients { get; set; } = 8;

    public SerializationType SerializationType { get; set; }
}
