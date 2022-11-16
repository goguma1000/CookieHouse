using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkTransform))]
[OrderAfter(typeof(NetworkRig), typeof(NetworkTransform), typeof(NetworkRigidbody))]
public class NetworkHeadset : NetworkBehaviour
{
    public NetworkTransform networkTransform;

    private void Awake()
    {
        if (networkTransform == null) networkTransform = GetComponent<NetworkTransform>();
    }
}
