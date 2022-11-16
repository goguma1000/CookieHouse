using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class NewBehaviourScript : NetworkBehaviour
{
    [SerializeField]GameObject prefab;
    bool triger = false;
    private void Start()
    {
        Runner.Spawn(prefab, Vector3.zero, Quaternion.identity, Object.InputAuthority, (runner, o) => { });
    }

    private void Update()
    {
        NetworkManager manager = NetworkManager.FindInstance();
        if (!triger && manager.GetPlayer())
        {
            Runner.Spawn(prefab, Vector3.zero, Quaternion.identity, Object.InputAuthority, (runner, o) => { });
            triger = true;       
        }
    }
}
