using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkPrefabFix : MonoBehaviour
{
    [SerializeField] private NetworkObject[] objects;
#if UNITY_EDITOR
    private void Start()
    {
        foreach (NetworkObject obj in objects)
        {
            obj.AlwaysReplicateAsRoot = !obj.AlwaysReplicateAsRoot;
            obj.AlwaysReplicateAsRoot = !obj.AlwaysReplicateAsRoot;
        }
    }
#endif
}
