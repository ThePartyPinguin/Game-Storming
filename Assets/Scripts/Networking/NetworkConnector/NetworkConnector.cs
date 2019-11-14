using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public abstract class NetworkConnector
{
    private TcpNetworkReceiver _receiver;

    public NetworkConnector()
    {
        
    }
}
