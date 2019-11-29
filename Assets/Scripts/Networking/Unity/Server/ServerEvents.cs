using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEvents : MonoBehaviour
{
    public void TestString(StringNetworkMessage message)
    {
        Debug.Log("Received string message: " + message.Value);
    }
}
