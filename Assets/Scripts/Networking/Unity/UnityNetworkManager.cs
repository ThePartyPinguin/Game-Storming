using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using GameFrame.Networking.Exception;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Connection events")]
    [SerializeField]
    private UnityEvent _onConnected;

    [SerializeField]
    private UnityEvent _onConnectFailed;
    
    [SerializeField]
    private UnityEvent _onConnectionInterrupted;

    private UnityNetworkMessageHandler _messageHandler;
    private NetworkConnector<NetworkEvent> _networkConnector;

    // Start is called before the first frame update
    void Start()
    {
        _messageHandler = UnityNetworkMessageHandler.Instance;

        var ipAddress = ParseIpAddress();

        _networkConnector = new NetworkConnector<NetworkEvent>(ipAddress, _port);
        _networkConnector.Setup(SerializationType.JSON);  
        _networkConnector.SetupCallbacks(_messageHandler.MessageHandled, CallOnConnected, CallOnConnectFailed, CallOnConnectionInterrupted);
        _networkConnector.Connect();
        _networkConnector.Start();
    }

    private IPAddress ParseIpAddress()
    {
        string ipAddress = _ipAddress;
        if (_ipAddress.Equals("localhost", StringComparison.OrdinalIgnoreCase))
            ipAddress = "127.0.0.1";

        if (IPAddress.TryParse(ipAddress, out var address))
        {
            return address;
        }
        throw new InvalidIPAdressException("The given ipAddress: " + _ipAddress + " is not valid");
    }

    private void CallOnConnected()
    {
        _onConnected?.Invoke();
    }

    private void CallOnConnectFailed()
    {
        _onConnectFailed?.Invoke();
    }

    private void CallOnConnectionInterrupted(NetworkConnector<NetworkEvent> connector)
    {
        _onConnectionInterrupted?.Invoke();
    }

    void OnApplicationQuit()
    {
        _networkConnector.Stop();
        Debug.Log("quit");
    }
}
