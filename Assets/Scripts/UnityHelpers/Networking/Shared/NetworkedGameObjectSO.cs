using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFrame.UnityHelpers.Networking.Shared
{
    [CreateAssetMenu(menuName = "Networking/new networkedGo")]
    public class NetworkedGameObjectSO : ScriptableObject
    {
        public int ObjectId => _objectId;
        public NetworkedGameObject Prefab => _prefab;


        [SerializeField]
        private int _objectId;

        [SerializeField]
        private NetworkedGameObject _prefab;
    }
}
