using GameFrame.Networking.Serialization;
using UnityEngine;
public class UntiyServerNetworkManager : MonoSingleton<UntiyServerNetworkManager>
{
    public int Port;
    public int MaxConnectedPlayers;
    public SerializationType SerializationType;

    private CustomGameServer _gameServer;
    // Start is called before the first frame update
    void Start()
    {
        _gameServer = new CustomGameServer(Port, MaxConnectedPlayers, SerializationType);
        _gameServer.StartServer();
        Debug.Log("Server has started");
    }
}
