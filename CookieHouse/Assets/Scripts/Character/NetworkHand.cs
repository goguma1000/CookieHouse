using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkTransform))]
[OrderAfter(typeof(NetworkRig), typeof(NetworkTransform), typeof(NetworkRigidbody))]
public class NetworkHand : NetworkBehaviour
{
    public NetworkTransform networkTransform;
    public RigPart side;
    NetworkRig rig;

    public bool isLocalNetworking => rig.isLocalNetworkRig;

    private void Awake()
    {
        rig = GetComponent<NetworkRig>();
        networkTransform = GetComponent<NetworkTransform>();
    }
}
