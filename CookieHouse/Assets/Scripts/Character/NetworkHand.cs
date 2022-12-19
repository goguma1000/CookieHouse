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
    public Animator handAnim;
    [Networked(OnChanged = nameof(UpdateAnim))]
    public float gripValue { get; set; }
    public bool IsLocalNetworkRig => rig.isLocalNetworkRig;

    private void Awake()
    {
        rig = GetComponent<NetworkRig>();
        networkTransform = GetComponent<NetworkTransform>();
    }

    private static void UpdateAnim(Changed<NetworkHand>changed)
    {
        changed.Behaviour.handAnim.SetFloat("Grip", changed.Behaviour.gripValue);
    }
}
