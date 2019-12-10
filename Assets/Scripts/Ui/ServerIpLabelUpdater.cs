using System.Net;
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
        if (!_set && !NetworkManager.GameServer.LocalIpAddress.Equals(IPAddress.Loopback))
        {
            _set = true;
            Label.text = NetworkManager.GameServer.LocalIpAddress.ToString() + ':' +
                         NetworkManager.GameServer.ListenPort;
        }
    }
}
