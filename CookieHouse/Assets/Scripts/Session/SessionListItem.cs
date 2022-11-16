using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using System;
public class SessionListItem : GridCell
{
    [SerializeField] private TMP_Text sessionNname;
    [SerializeField] private TMP_Text map;
    [SerializeField] private TMP_Text players;

    private Action<SessionInfo> _onJoin;
    private SessionInfo _info;

    public void Setup(SessionInfo info, Action<SessionInfo> onjoin)
    {
        _info = info;
        sessionNname.text = $"{info.Name} ({info.Region})";
        map.text = "CookieHouse";
        players.text = $"{info.PlayerCount}/({info.MaxPlayers})";
        _onJoin = onjoin;
    }

    public void OnJoin()
    {
        if (_info.PlayerCount != _info.MaxPlayers)
            _onJoin(_info);
        else Debug.Log("The Room is Full");
    }
}
