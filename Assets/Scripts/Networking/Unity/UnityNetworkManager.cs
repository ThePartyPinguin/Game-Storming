using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using UnityEngine;

public class UnityNetworkManager : MonoSingleton<UnityNetworkManager>
{
    public string IpAddress => _ipAddress;
    public int Port => _port;

    [SerializeField]
    private string _ipAddress;

    [SerializeField]
    private int _port;

    [SerializeField]
    private SerializationType _serializationType;

    //private NetworkMessageDeserializer<BaseNetworkMessage, NetworkEvent> _messageDeserializer;
    private NetworkConnector<NetworkEvent> _networkConnector;

    // Start is called before the first frame update
    void Start()
    {
        //_networkConnector.Setup<StringNetworkMessage>(new UnityStringMessageEventHandler().MessageHandled, SerializationType.JSON);   
    }
}
