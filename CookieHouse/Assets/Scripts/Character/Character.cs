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
       
    }

    
}
