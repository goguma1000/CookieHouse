using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;


public enum RigPart
{
    None,
    Headset,
    LeftController,
    RightController,
    Undefined
}
public struct RigInput : INetworkInput {
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public Vector3 leftHandPosition;
    public Quaternion leftHandRotation;
    public Vector3 rightHandPosition;
    public Quaternion rightHandRotation;
    public Vector3 headsetPosition;
    public Quaternion headsetRotation;
    
}

public class HardwareRig : MonoBehaviour, INetworkRunnerCallbacks
{
    public HardwareHand leftHand;
    public HardwareHand rightHand;
    public HardwareHeadset headset;
    public NetworkRig transformBridge;
    public NetworkRunner runner;

    private void Start()
    {
        if(runner == null)
        {
            NetworkManager manager = NetworkManager.FindInstance();
            runner = manager.GetMangerRunner();
            Debug.LogError("Runner has to be set in the inspector to forward the input");
        }
        Debug.Log(runner);
        runner.AddCallbacks(this);
       
    }
    public void OnInput(NetworkRunner runner, NetworkInput input) {
  
        RigInput rigInput = new RigInput();
        rigInput.playerPosition = transform.position;
        rigInput.playerRotation = transform.rotation;
        rigInput.leftHandPosition = leftHand.transform.position;
        rigInput.leftHandRotation = leftHand.transform.rotation;
        rigInput.rightHandPosition = rightHand.transform.position;
        rigInput.rightHandRotation = rightHand.transform.rotation;
        rigInput.headsetPosition = headset.transform.position;
        rigInput.headsetRotation = headset.transform.rotation;
        input.Set(rigInput);
    }

    #region Unused Callback
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    #endregion
}
