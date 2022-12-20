using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkManager manager;
    [SerializeField] private Character[] characterPrefabs;
    [Networked] public int selectedCharacterNum { get; set; }
    [Networked] public string playerName { get; set; }
    
    [Networked] public NetworkBool Ready { get; set; }

    private Character character;
    public override void Spawned()
    {
        selectedCharacterNum= 0;
        manager = NetworkManager.FindInstance();
        transform.SetParent(Runner.gameObject.transform);
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority && character == null && SceneManager.GetActiveScene().buildIndex == (int)MapIndex.GameMap)
        {
            Debug.Log($"Spawning avatar for player {name} with input auth {Object.InputAuthority}");
            character = Runner.Spawn(characterPrefabs[selectedCharacterNum - 1],Vector3.zero, Quaternion.identity, Object.InputAuthority, (runner, o) => {
                Character temp = o.GetComponent<Character>();
                Debug.Log($"Created Character for Player {playerName}");
                temp.Player = this;
            });
        }
    }

    public void ForceReset(Player ply)
    {
        ply.RPC_SetCharacterSelected(0);
        ply.RPC_SetIsReady(false);
    }

    public void Despawn()
    {
        if (HasStateAuthority)
        {
            Runner.Despawn(Object);
        }
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetCharacterSelected(int btnNum)
    {
        selectedCharacterNum = btnNum;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetPlayerName(string name)
    {
        playerName = name;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
    public void RPC_SetIsReady(NetworkBool ready)
    {
        Ready = ready;
    }
}
