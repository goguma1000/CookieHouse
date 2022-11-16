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

    [SerializeField] private GameObject PlayerListItemPrefab;
    [SerializeField] private Transform itemParent;
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI startText;
    private float y = -32;
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
            int count = Runner.ActivePlayers.Count();
            if (prePlayerCount != count && player != null&& player.playerName != "")
            {
                UpdateList(count);
                Debug.Log(count);
            }
            if (prePlayerCount == count)
            {
                CheckReady(count);
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
        Recycle();
        if (manager.ForEachPlayer(nowPlayerCount, ply =>
          {
              AddRow(ply);
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
        if (ready < nowPlayerCount)
            wait = $"Waiting for {nowPlayerCount - ready} of {nowPlayerCount} players";
        else if (!manager.IsSessionOwner)
            wait = "Waiting for session owner to start";
        startButton.enabled = wait == null;
        startText.text = wait ?? "Start";
    }
    private void AddRow(Player ply)
    {
        
        GameObject go = Instantiate(PlayerListItemPrefab, Vector3.zero, Quaternion.identity, itemParent);
        PlayerListItem item = go.GetComponent<PlayerListItem>();
        item.SetUp(ply);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
        rt.anchoredPosition = new Vector2(0, y);
        y -= rt.rect.height;
    }

    private void Recycle()
    {
        if (itemParent != null)
        {
            PlayerListItem[] items = itemParent.GetComponentsInChildren<PlayerListItem>();
            foreach (PlayerListItem temp in items)
            {
                temp.transform.SetParent(null);
                Destroy(temp.gameObject);
            }
        }
        y = -32;
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
        SessionProps props = manager.Session.Props;
        manager.Session.LoadMap(MapIndex.GameMap);
    }
}