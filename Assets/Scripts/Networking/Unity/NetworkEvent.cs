using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetworkEvent
{
    //Events send by client
    CLIENT_TO_SERVER_HANDSHAKE,
    CLIENT_DISCONNECT,
    //Events send by server
    SERVER_TO_CLIENT_HANDSHAKE,
    SERVER_DISCONNECT
}
