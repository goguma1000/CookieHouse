using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    
    public void SetUp(Player ply)
    {
        playerName.text = ply.playerName;
    }
}
