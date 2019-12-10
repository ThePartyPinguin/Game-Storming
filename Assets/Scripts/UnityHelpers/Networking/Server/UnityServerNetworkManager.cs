using System;
using System.Collections;
using GameFrame.UnityHelpers.Networking;
using GameFrame.UnityHelpers.Networking.Message;
using GameFrame.Networking.Serialization;
using GameFrame.Networking.Server;
using GameFrame.UnityHelpers.Networking.Client;
using GameFrame.UnityHelpers.Networking.Shared;
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

    public GameServer<NetworkEvent> GameServer { get; private set; }

    public ClientIdCallback OnClientConnect;

    private ClientIdCallback _callbackToCallInCoRoutine;
    private bool _startCoRoutine;

    public NetworkedGameObjectCollection NetworkedGameObjects;
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

        foreach (var o in NetworkedGameObjects.NetworkedGameObjects)
        {
            Debug.Log(o.GetHashCode());
        }

        GameServer = new GameServer<NetworkEvent>(settings, (client) =>
        {
            ASyncToSynchronousCallbackHandler.Instance.QueueCallbackToHandle(() => OnClientConnect?.Invoke(client.ClientId));
        });

        GameServer.StartServer();

        Debug.Log("Server started");
    }

    public void Stop()
    {
        GameServer.StopServer();
    }

    void OnApplicationQuit()
    {
        GameServer.StopServer();
        Debug.Log("Server stopped");
    }

    public void SendMessageToPlayer(Guid playerId, BaseNetworkMessage message)
    {
        GameServer.SecureSendMessageToSpecificPlayer(playerId, message);
    }

    public void BroadcastMessage(BaseNetworkMessage message)
    {
        GameServer.SecureBroadcastMessage(message);
    }
    
    public void BroadcastMessage(BaseNetworkMessage message, Guid excludePlayerId)
    {
        GameServer.SecureBroadcastMessage(message, excludePlayerId);
    }

    [Serializable]
    public class ClientIdCallback : UnityEvent<Guid>
    {

    }
}
