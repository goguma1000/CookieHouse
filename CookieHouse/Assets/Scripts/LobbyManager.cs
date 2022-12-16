using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

public class LobbyManager : NetworkBehaviour
{

    [SerializeField] private TextMeshProUGUI[] PlayerListItems;
    [SerializeField] private NetworkButton[] networkButtons;
    [SerializeField] private Transform itemParent;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startText;
    private int prePlayerCount = 0;
    private Player player;
    private int ready = 0;
    private NetworkManager manager;

    private void Awake()
    {
        manager = NetworkManager.FindInstance();
        manager.GetPlayer()?.RPC_SetIsReady(false);
    }
 
    void Update()
    {
        player = manager.GetPlayer();
        if (manager.ConnectionStatus == ConnectionStatus.Started)
        {
            if (Runner != null)
            {
                int count = Runner.ActivePlayers.Count();
                if (prePlayerCount != count && player != null && player.playerName != "")
                {
                    if(prePlayerCount > count)
                    {
                        for (int i = 0; i < networkButtons.Length; i++)
                        {
                            networkButtons[i].ResetButton(player);
                        }
                    }
                    UpdateList(count);
                }
                if (prePlayerCount == count)
                {
                    CheckReady(count);
                }
            }
        }
    }

    public void DisconnectRoom()
    {   
        manager = NetworkManager.FindInstance();
        manager.Disconnect();
    }
    public void UpdateList(int nowPlayerCount)
    {
        Refresh();
        if (manager.ForEachPlayer(nowPlayerCount, ply =>
          {
              UpdateName(ply, nowPlayerCount);
          })) prePlayerCount = nowPlayerCount;

    }

    private void CheckReady(int nowPlayerCount)
    {
        ready = 0;
        manager.ForEachPlayer(nowPlayerCount, ply =>
        {
            if (ply.Ready) ready++;
        });

        string wait = null;
        if (nowPlayerCount != 2)
        {
            wait = $"Waiting for other players";
        }
        else
        {
            if (ready < nowPlayerCount)
                wait = $"Waiting for {nowPlayerCount - ready} of {nowPlayerCount} players";
            else if (!manager.IsSessionOwner)
                wait = "Waiting for session owner to start";
        }
        startButton.enabled = wait == null;
        startText.text = wait ?? "Start";
        if (wait == null) startText.fontSize = 20;
        else startText.fontSize = 10;
    }
    private void UpdateName(Player ply, int nowPlayerCount)
    {
        for(int i =0; i < nowPlayerCount; i++)
        {
            if (PlayerListItems[i].text != "") continue;
            else
            {
                PlayerListItems[i].text = ply.playerName;
                break;
            }
        }
    }
    private void Refresh()
    {
        for (int i = 0; i < PlayerListItems.Length; i++)
        {
            PlayerListItems[i].text = "";
        }
    }

    public void OnClickReady()
    {
        Player player = manager.GetPlayer();
        if (player.Ready) player.RPC_SetIsReady(false);
        else player.RPC_SetIsReady(true);
    }

    public void OnStartGame()
    {   
        manager = NetworkManager.FindInstance();
        manager.loadingPanel.SetActive(true);
        SessionProps props = manager.Session.Props;
        manager.Session.LoadMap(MapIndex.GameMap);
    }
}