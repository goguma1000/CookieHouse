using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using System.Threading.Tasks;

public class NetworkButton : NetworkBehaviour
{
    [Networked]
    public string playerName { get; set; }

    [Networked(OnChanged = nameof(OnSelected))]
    public int Owner { get; set; }

    public TextMeshProUGUI text;
    public GameObject SelectImg;
    private bool isTakingAuthority = false;
    public void setString(string name)
    {
        text.text = playerName;
    }

    private static void OnSelected(Changed<NetworkButton>changed)
    {
        changed.LoadNew();
        changed.Behaviour.setString(changed.Behaviour.playerName);
        if (changed.Behaviour.Owner != 0)
        {
            changed.Behaviour.SelectImg.SetActive(true);
        }
        else
        {
            changed.Behaviour.SelectImg.SetActive(false);
        }
        Debug.Log("update" + changed.Behaviour.GetInstanceID());
    }

    public async void OnClickButton(int btnNum)
    {
        NetworkManager manager = NetworkManager.FindInstance();
        Player player = manager.GetPlayer();

        isTakingAuthority = true;
        bool auth = await Object.WaitForStateAuthority();
        isTakingAuthority = false;
        if (auth)
        {
            if (Owner == 0 && player.selectedCharacterNum == 0)
            {
                playerName = player.playerName;
                Owner = player.GetInstanceID();
                player.RPC_SetCharacterSelected(btnNum);
                player.RPC_SetIsReady(true);
            }
            else if (Owner == player.GetInstanceID())
            {
                playerName = "";
                Owner = 0;
                player.RPC_SetCharacterSelected(0);
                player.RPC_SetIsReady(false);
            }
        }
    }
    
    public async void ResetButton(Player player)
    {
        isTakingAuthority = true;
        bool auth = await Object.WaitForStateAuthority();
        isTakingAuthority = false;
        if (true)
        {
            if (Owner != player.GetInstanceID())
            {           
                playerName = "";
                Owner = 0;
            }
            Debug.Log($"Owner:{Owner}  playerName:{playerName}");
        }
    }

    public async void ForceReset(Player ply)
    {
        isTakingAuthority = true;
        bool auth = await Object.WaitForStateAuthority();
        isTakingAuthority = false;
        playerName = "";
        Owner = 0;
        ply.RPC_SetCharacterSelected(0);
        ply.RPC_SetIsReady(false);
    }
}
