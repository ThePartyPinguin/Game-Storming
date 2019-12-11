using System.Net;
using TMPro;
using UnityEngine;

public class ServerIpLabelUpdater : MonoBehaviour
{
    private UnityServerNetworkManager _networkManager;
    public TMP_Text Label;

    private bool _set;

    // Start is called before the first frame update
    void Start()
    {
        _networkManager = UnityServerNetworkManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_set && _networkManager.GameServer != null && !_networkManager.GameServer.LocalIpAddress.Equals(IPAddress.Loopback))
        {
            _set = true;
            Label.text = _networkManager.GameServer.LocalIpAddress.ToString();
        }
    }
}
