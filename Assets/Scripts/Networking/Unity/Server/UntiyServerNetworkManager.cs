using GameFrame.Networking.Serialization;
using UnityEngine;

[RequireComponent(typeof(UnityNetworkMessageHandler))]
public class UntiyServerNetworkManager : MonoSingleton<UntiyServerNetworkManager>
{
    public int Port;
    public int MaxConnectedPlayers;
    public SerializationType SerializationType;

    private CustomGameServer _gameServer;
    private UnityNetworkMessageHandler _messageHandler;
    // Start is called before the first frame update
    void Start()
    {
        _messageHandler = GetComponent<UnityNetworkMessageHandler>();
        _gameServer = new CustomGameServer(Port, MaxConnectedPlayers, _messageHandler, SerializationType);
        _gameServer.StartServer();
        Debug.Log("Server has started");
    }
}
