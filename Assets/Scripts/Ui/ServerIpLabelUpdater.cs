using TMPro;
using UnityEngine;

public class ServerIpLabelUpdater : MonoBehaviour
{
    public UnityServerNetworkManager NetworkManager;
    public TMP_Text Label;

    private bool _set;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_set)
        {
            if(NetworkManager.GameServer != null && NetworkManager.GameServer.ServerListener != null && NetworkManager.GameServer.ServerListener.LocalIpAddress != null)
            {
                _set = true;
                Label.text = NetworkManager.GameServer.ServerListener.LocalIpAddress.ToString() + ':' +
                             NetworkManager.GameServer.ServerListener.ListenPort;
            }
        }
    }
}
