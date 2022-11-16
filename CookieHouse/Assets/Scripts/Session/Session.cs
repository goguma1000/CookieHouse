using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class Session : NetworkBehaviour
{
    private NetworkManager manager;
    public SessionProps Props => new SessionProps(Runner.SessionInfo.Properties);
    public SessionInfo info => Runner.SessionInfo;
    public override void Spawned()
    {
        manager = NetworkManager.FindInstance();
        manager.Session = this;

        if(Object.HasStateAuthority && (Runner.CurrentScene == 0 || Runner.CurrentScene == SceneRef.None)){
            Runner.SetActiveScene((int)MapIndex.Lobby);
        }
    }

    public void LoadMap(MapIndex mapIndex)
    {
        Runner.SessionInfo.IsOpen = Props.AllowLateJoin;
        Runner.SetActiveScene((int)mapIndex);
    }
}
