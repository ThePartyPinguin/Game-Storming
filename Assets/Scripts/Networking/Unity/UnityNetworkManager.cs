using System;
using System.Collections;
using System.Collections.Generic;
using GameFrame.Networking.Messaging.MessageHandling;
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

    private NetworkMessageDeserializer<MessageEventType> _messageDeserializer;
    private NetworkConnector<MessageEventType> _networkConnector;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
