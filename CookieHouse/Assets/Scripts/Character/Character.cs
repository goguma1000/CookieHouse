using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using Unity.XR.CoreUtils;

public class Character : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [Networked] public Player Player {get;set;}
    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            GameObject xrOrrigin = FindObjectOfType<XROrigin>().gameObject;
            xrOrrigin.transform.SetParent(this.gameObject.transform);
            xrOrrigin.transform.localPosition = new Vector3(0, 0, 0);
            xrOrrigin.transform.localRotation = Quaternion.identity;
            Debug.Log("set parent");
        }
    }

    
}
