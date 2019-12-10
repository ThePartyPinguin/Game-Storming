using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrame.UnityHelpers.Networking.Shared
{
    [CreateAssetMenu(menuName = "Networking/new networkedGo collection")]
    public class NetworkedGameObjectCollection : ScriptableObject
    {
        public List<NetworkedGameObjectSO> NetworkedGameObjects;
    }
}
