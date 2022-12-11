using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(NetworkTransform))]
[OrderAfter(typeof(NetworkTransform), typeof(NetworkRigidbody))]
public class NetworkRig : NetworkBehaviour
{
    public HardwareRig hardwareRig;
    public NetworkHand lefthand;
    public NetworkHand righthand;
    public NetworkHeadset headset;
    public GameObject spawnPosition;
    public NetworkTransform networkTransform;

    private void Awake()
    {
        networkTransform = GetComponent<NetworkTransform>();
    }

    public bool isLocalNetworkRig => Object.HasStateAuthority;

    public override void Spawned()
    {
        base.Spawned();
        if (isLocalNetworkRig)
        {
            hardwareRig = FindObjectOfType<HardwareRig>();
            if(hardwareRig.transformBridge == null) 
            { 
                hardwareRig.transformBridge = this; 
            }
            if (hardwareRig == null) Debug.LogError("Missing HardwareRig in the scene");
            hardwareRig.gameObject.transform.position = spawnPosition.transform.position;
            hardwareRig.gameObject.transform.rotation = spawnPosition.transform.rotation;
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if(GetInput<RigInput>(out var input))
        {
            transform.position = input.playerPosition;
            transform.rotation = input.playerRotation;
            lefthand.transform.position = input.leftHandPosition;
            lefthand.transform.rotation = input.leftHandRotation;
            righthand.transform.position = input.rightHandPosition;
            righthand.transform.rotation = input.rightHandRotation;
            headset.transform.position = input.headsetPosition;
            headset.transform.rotation = input.headsetRotation;
        }
    }

    public override void Render()
    {
        base.Render();
        if (isLocalNetworkRig)
        {
            networkTransform.InterpolationTarget.position = hardwareRig.transform.position;
            networkTransform.InterpolationTarget.rotation = hardwareRig.transform.rotation;
            lefthand.networkTransform.InterpolationTarget.position = hardwareRig.leftHand.transform.position;
            lefthand.networkTransform.InterpolationTarget.rotation = hardwareRig.leftHand.transform.rotation;
            righthand.networkTransform.InterpolationTarget.position = hardwareRig.rightHand.transform.position;
            righthand.networkTransform.InterpolationTarget.rotation = hardwareRig.rightHand.transform.rotation;
            headset.networkTransform.InterpolationTarget.position = hardwareRig.headset.transform.position;
            headset.networkTransform.InterpolationTarget.rotation = hardwareRig.headset.transform.rotation;
        }

    }
}
