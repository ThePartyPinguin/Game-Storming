using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public interface IHandshakeHandler
{
    void AddClientToAccept(TcpClient client);
}
