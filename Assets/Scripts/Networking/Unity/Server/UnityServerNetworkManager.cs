using System;
using System.Collections;
using GameFrame.Networking.Messaging.MessageHandling;
using GameFrame.Networking.NetworkConnector;
using GameFrame.Networking.Serialization;
using GameFrame.Networking.Server;
using UnityEngine;
using UnityEngine.Events;

public class UnityServerNetworkManager : MonoSingleton<UnityServerNetworkManager>
{
    public int TcpPort;
    public int UdpRemoteSendPort;
    public int UdpReceivePort;
    public int MaxConnectedPlayers;

    public SerializationType SerializationType;
    private GameServer<NetworkEvent> _gameServer;
    
    [SerializeField]
    private OnConnectCallback _onClientConnect;

    // Start is called before the first frame update
    void Start()
    {
        Setup();

    }

    public void Setup()
    {
        StartCoroutine(StartServer());
    }

    private IEnumerator StartServer()
    {
        yield return new WaitForSeconds(2f);

        ServerSettings<NetworkEvent> settings = new ServerSettings<NetworkEvent>();

        settings.ClientToServerHandshakeEvent = NetworkEvent.CLIENT_TO_SERVER_HANDSHAKE;
        settings.ServerToClientHandshakeEvent = NetworkEvent.SERVER_TO_CLIENT_HANDSHAKE;
        settings.MaxConnectedClients = MaxConnectedPlayers;
        settings.SerializationType = SerializationType;
        settings.TcpPort = TcpPort;
        settings.UdpReceivePort = UdpReceivePort;
        settings.UdpRemoteSendPort = UdpRemoteSendPort;


        _gameServer = new GameServer<NetworkEvent>(settings, (guid) => _onClientConnect?.Invoke(guid));

        _gameServer.StartServer();

        Debug.Log("Server started");
    }

    public void Stop()
    {

    }

    void OnApplicationQuit()
    {
        _gameServer.StopServer();
        Debug.Log("Server stopped");
    }

    [Serializable]
    private class OnConnectCallback : UnityEvent<Guid>
    {

    }
}
