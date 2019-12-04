using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Assets.Scripts.Networking.Client;
using GameFrame.Networking.Exception;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using UnityEngine;
using UnityEngine.Events;
public class UnityNetworkManager : MonoSingleton<UnityNetworkManager>
{
    //public NetworkConnector<NetworkEvent> NetworkConnector => _networkConnector;

    [SerializeField]
    private string _ipAddress;

    [SerializeField]
    private int _tcpPort;

    [SerializeField]
    private int _udpReceivePort;

    [SerializeField]
    private int _udpRemoteSendPort;

    [SerializeField]
    private SerializationType _serializationType;

    [SerializeField] 
    private bool _useUdp;

    [Header("Connection events")]
    [SerializeField]
    private OnConnectCallback _onConnected;

    [SerializeField]
    private UnityEvent _onConnectFailed;
    
    [SerializeField]
    private UnityEvent _onConnectionInterrupted;
    
    private GameClient<NetworkEvent> _gameClient;

    void Start()
    {
        Connect();
        _onConnected.AddListener((guid) =>
        {
            Debug.Log(guid);
        });
    }

    public void Connect()
    {
        StartCoroutine(ConnectCoRoutine());
    }


    private IEnumerator ConnectCoRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        Debug.Log("Setting up connection to server");

        var ipAddress = ParseIpAddress();

        ClientConnectionSettings<NetworkEvent> settings = new ClientConnectionSettings<NetworkEvent>();

        settings.ClientToServerHandshakeEvent = NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE;
        settings.ServerToClientHandshakeEvent = NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE;
        settings.ClientDisconnectEvent = NetworkEvent.CLIENT_DISCONNECT;
        settings.ServerDisconnectEvent = NetworkEvent.SERVER_DISCONNECT;

        settings.SerializationType = SerializationType.JSON;
        settings.ServerIpAddress = ipAddress;
        settings.TcpPort = _tcpPort;

        if (_useUdp)
        {
            settings.UseUdp = _useUdp;

            settings.UdpRemoteSendPort = _udpRemoteSendPort;
            settings.UdpReceivePort = _udpReceivePort;

        }
        SetupHandshakeEvent();

        _gameClient = new GameClient<NetworkEvent>(settings);

        _gameClient.OnConnectionSuccess = (guid) => { Debug.Log(guid); };
        _gameClient.OnConnectionFailed += () => Debug.Log("Could not connect to server");
        _gameClient.OnConnectionLost += () => Debug.Log("Connection to server lost");

        _gameClient.Connect();
    }

    private void SetupHandshakeEvent()
    {
        
    }

    public void Test(EventOnlyNetworkMessage message)
    {
        Debug.Log("Receive response");
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

    private void CallOnConnected(Guid clientId)
    {
        Debug.Log("Connected: " + clientId);

       _onConnected?.Invoke(clientId);
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
        _gameClient.Stop();
        Debug.Log("quit");
    }

    [Serializable]
    private class OnConnectCallback : UnityEvent<Guid>
    {
        
    }
}
