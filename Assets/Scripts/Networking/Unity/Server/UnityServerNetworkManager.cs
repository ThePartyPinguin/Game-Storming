using System;
using System.Collections;
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
    public bool UseUdp;

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
        settings.ClientDisconnectEvent = NetworkEvent.CLIENT_DISCONNECT;
        settings.ServerDisconnectEvent = NetworkEvent.SERVER_DISCONNECT;

        settings.MaxConnectedClients = MaxConnectedPlayers;
        settings.SerializationType = SerializationType;
        settings.TcpPort = TcpPort;

        if (UseUdp)
        {
            settings.UseUdp = UseUdp;

            settings.UdpReceivePort = UdpReceivePort;
            settings.UdpRemoteSendPort = UdpRemoteSendPort;
        }

        _gameServer = new GameServer<NetworkEvent>(settings, (guid) => _onClientConnect?.Invoke(guid));

        _gameServer.StartServer();

        Debug.Log("Server started");
    }

    public void Stop()
    {

    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Screenmanager Resolution Width", 800);
        PlayerPrefs.SetInt("Screenmanager Resolution Height", 600);
        PlayerPrefs.SetInt("Screenmanager Is Fullscreen mode", 0);
        _gameServer.StopServer();
        Debug.Log("Server stopped");
    }

    public void SendMessageToPlayer(Guid playerId, BaseNetworkMessage message)
    {
        _gameServer.SecureSendMessageToSpecificPlayer(playerId, message);
    }

    public void BroadcastMessage(BaseNetworkMessage message)
    {
        _gameServer.SecureBroadcastMessage(message);
    }
    
    public void BroadcastMessage(BaseNetworkMessage message, Guid excludePlayerId)
    {
        _gameServer.SecureBroadcastMessage(message, excludePlayerId);
    }

    [Serializable]
    private class OnConnectCallback : UnityEvent<Guid>
    {

    }
}
