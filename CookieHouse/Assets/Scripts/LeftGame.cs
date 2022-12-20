using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftGame : MonoBehaviour
{
    public void DisConnectGame()
    {
        NetworkManager manager = NetworkManager.FindInstance();
        manager.Disconnect();
    }
}
