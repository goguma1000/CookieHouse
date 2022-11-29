using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNamePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputBox;
    [SerializeField] private GameObject playerlistPanel;
    [SerializeField] private GameObject[] buttons;
    NetworkManager manager;
    private void Awake()
    {
        manager = NetworkManager.FindInstance();
    }
    private void Update()
    {
        Player ply = manager.GetPlayer();
        if (inputBox.text == "" && (ply != null)) setDefaultName();
    }

    private void setDefaultName()
    {
        manager = NetworkManager.FindInstance();
        inputBox.text = "Player" + manager.GetPlayer().GetInstanceID();
        OnChangedName();
        this.transform.parent.gameObject.SetActive(false);
        playerlistPanel.SetActive(true);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(true);
           
        }
    }
    public void OnChangedName( )
    {
        if (inputBox.text != "")
        {
            Player ply = manager.GetPlayer();
            ply.RPC_SetPlayerName(inputBox.text);
        }
    }
}
