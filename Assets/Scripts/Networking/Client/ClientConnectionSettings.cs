using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using GameFrame.Networking.Serialization;
using UnityEngine;

public class ClientConnectionSettings<TEnum> where TEnum : Enum
{
    public TEnum ClientToServerHandshakeEvent { get; set; }

    public TEnum ServerToClientHandshakeEvent { get; set; }

    public TEnum ClientDisconnectEvent { get; set; }
    public TEnum ServerDisconnectEvent { get; set; }

    public int TcpPort { get; set; } = 5555;

    public int UdpReceivePort { get; set; } = 12000;
    public int UdpRemoteSendPort { get; set; } = 11000;

    public IPAddress ServerIpAddress { get; set; } = null;

    public SerializationType SerializationType { get; set; }

    public bool UseUdp { get; set; }
}
