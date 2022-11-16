using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
public class NetworkButton : NetworkBehaviour
{
    [Networked(OnChanged = nameof(OnSelected))]
    public bool isSelected { get; set; }

    [Networked]
    public string playerName { get; set; }

    [Networked(OnChanged = nameof(OnSelected))]
    public int Owner { get; set; }

    public TextMeshProUGUI text;
    public bool isTakingAuthority = false;

    public void setString(string name)
    {
        text.text = playerName;
    }

    private static void OnSelected(Changed<NetworkButton>changed)
    {
        changed.LoadNew();
        changed.Behaviour.setString(changed.Behaviour.playerName);
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
                playerName = "blank";
                Owner = 0;
                player.RPC_SetCharacterSelected(0);
                player.RPC_SetIsReady(false);
            }
        }
    }
}
