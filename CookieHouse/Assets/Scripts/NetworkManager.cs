using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Linq;

public enum ConnectionStatus
{
    Disconnected,
    Connecting,
    Connected,
    Failed,
    EnteringLobby,
    InLobby,
    Starting,
    Started
}

public enum MapIndex 
{
    Intro,
    RoomList,
    Lobby,
    GameMap
}

[RequireComponent(typeof(NetworkSceneManagerBase))]
public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
{
    
    [SerializeField] private Session sessionPrefab;
    [SerializeField] private Player playerPrefab;
    public GameObject loadingPanel;

    private GameObject mainPanel;
    private NetworkRunner _runner;
    private NetworkSceneManagerBase loader;
    private Action<List<SessionInfo>> onSessionListUpdated;
    private Session session;
    private string lobbyid;
    public ConnectionStatus ConnectionStatus { get; private set; }
    public bool IsSessionOwner => _runner != null && (_runner.IsServer || _runner.IsSharedModeMasterClient);
    
    

    private void Awake()
    {
        NetworkManager[] manager = FindObjectsOfType<NetworkManager>();
        if(manager != null && manager.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        if(loader == null)
        {
            loader = GetComponent<NetworkSceneManagerBase>();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public NetworkRunner GetMangerRunner() { return _runner; }
    public static NetworkManager FindInstance()
    {
        return FindObjectOfType<NetworkManager>();
    }
    public void CreateSessoin(SessionProps props)
    {
        StartSession(GameMode.Shared, props, false);
    }

    private async void StartSession(GameMode mode, SessionProps props, bool disableClientSessionCreation = false)
    {
        loadingPanel.SetActive(true);
        mainPanel = GameObject.FindGameObjectWithTag("MainPanel");
        mainPanel.SetActive(false);
        Connect();

        SetConnectionStatus(ConnectionStatus.Starting);
        Debug.Log($"Starting game with session {props.RoomName}, player limit {props.PlayerLimit}");
        _runner.ProvideInput = mode != GameMode.Server;
        StartGameResult result = await _runner.StartGame(new StartGameArgs
        {
            GameMode = mode,
            CustomLobbyName = lobbyid,
            SceneManager = loader,
            SessionName = props.RoomName,
            PlayerCount = props.PlayerLimit,
            SessionProperties = props.properties,
            DisableClientSessionCreation = disableClientSessionCreation

        });
        if (!result.Ok)
            SetConnectionStatus(ConnectionStatus.Failed, result.ShutdownReason.ToString());
    }
    public async Task EnterLobby(string ID , Action<List<SessionInfo>> onSessionListUpdate)
    {
        Connect();
        mainPanel = GameObject.FindGameObjectWithTag("MainPanel");
        mainPanel.SetActive(false);
        loadingPanel.SetActive(true);
        lobbyid = ID;
        onSessionListUpdated = onSessionListUpdate;
        SetConnectionStatus(ConnectionStatus.EnteringLobby);
        var result = await _runner.JoinSessionLobby(SessionLobby.Custom,lobbyid);
        if (!result.Ok)
        {
            onSessionListUpdated = null;
            SetConnectionStatus(ConnectionStatus.Failed);
            onSessionListUpdate(null);
        }
    }
    private void Connect()
    {
        if(_runner == null)
        {
            SetConnectionStatus(ConnectionStatus.Connecting);
            GameObject go = new GameObject("Session");
            go.transform.SetParent(transform);

            _runner = go.AddComponent<NetworkRunner>();
            _runner.AddCallbacks(this);
        }
    }

    private void SetConnectionStatus(ConnectionStatus status, string reason = "")
    {
        if (ConnectionStatus == status) return;
        ConnectionStatus = status;
        if(!string.IsNullOrWhiteSpace(reason) && reason != "Ok")
        {
            Debug.Log("error: " + reason);
        }

        Debug.Log($"ConnectionStatus = {status} {reason}");
    }
    public void Disconnect()
    {
        if(_runner != null)
        {
            SetConnectionStatus(ConnectionStatus.Disconnected);
            _runner.Shutdown();
        }
    }
    public void JoinSession(SessionInfo info)
    {
        SessionProps props = new SessionProps(info.Properties);
        props.PlayerLimit = info.MaxPlayers;
        props.RoomName = info.Name;
        StartSession(GameMode.Shared, props);
    }

    public Session Session
    {
        get => session;
        set { session = value; session.transform.SetParent(_runner.transform); }
    }

    public Player GetPlayer()
    {
        return _runner?.GetPlayerObject(_runner.LocalPlayer)?.GetComponent<Player>();
    }

    public bool ForEachPlayer(int playerCount,Action<Player> action)
    {
        Player[] temp = _runner.gameObject.GetComponentsInChildren<Player>();
        if (temp.Length != playerCount) return false;
        else
        {
            foreach (PlayerRef plyRef in _runner.ActivePlayers)
            {
                NetworkObject plyObj = _runner.GetPlayerObject(plyRef);
                if (plyObj)
                {
                    Player ply = plyObj.GetComponent<Player>();
                    if (ply.playerName == "") return false;
                    action(ply);
                }
            }
            return true;
        }
    }

    private void Update()
    {

    }

    #region Fusion interface use
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"player {player} Joined!");
        if (session == null && IsSessionOwner)
        {
            Debug.Log("Spawning World");
            session = runner.Spawn(sessionPrefab, Vector3.zero, Quaternion.identity);
        }

        if (runner.IsServer || runner.Topology == SimulationConfig.Topologies.Shared && player == runner.LocalPlayer)
        {
            Debug.Log("Spawning player");
            runner.Spawn(playerPrefab, Vector3.zero, Quaternion.identity, player, (runner, obj) => runner.SetPlayerObject(player, obj));

        }

        SetConnectionStatus(ConnectionStatus.Started);
    }

    public async void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"{player.PlayerId} disconnected");
        if (runner.Topology == SimulationConfig.Topologies.Shared && player != runner.LocalPlayer)
        {
            if(SceneManager.GetActiveScene().buildIndex == (int)MapIndex.GameMap)
            {
                Disconnect();
                return;
            }
            NetworkObject playerObj = runner.GetPlayerObject(player);
            if (playerObj)
            {
                await playerObj.WaitForStateAuthority();
                if (playerObj != null && playerObj.HasStateAuthority)
                {
                    Debug.Log("Despawning Player");
                    playerObj.GetComponent<Player>().Despawn();
                }
                else { Debug.Log($"plyObj{playerObj}, auth: {playerObj.HasStateAuthority}"); }
            }
            else Debug.Log("playerObj null");

        }
        else Debug.Log("IsServer false");
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        SetConnectionStatus(ConnectionStatus.InLobby);
        onSessionListUpdated?.Invoke(sessionList);
        if (loadingPanel.activeSelf)
        {
            loadingPanel.SetActive(false);
            mainPanel = GameObject.FindGameObjectWithTag("MainPanel");
            mainPanel.SetActive(true);
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        Debug.Log($"On Shutdown {shutdownReason}");
        SetConnectionStatus(ConnectionStatus.Disconnected, shutdownReason.ToString());
        if (_runner != null && _runner.gameObject)
            Destroy(_runner.gameObject);
        _runner = null;
        session = null;

        if (Application.isPlaying)
        {
            if (SceneManager.GetActiveScene().buildIndex == (int)MapIndex.RoomList)
            {
                SceneManager.LoadSceneAsync((int)MapIndex.Intro);
            }
            else
            {
                SceneManager.LoadSceneAsync((int)MapIndex.RoomList);
            }
        }

    }

    
    #endregion


    #region Fusion Interface not use
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input) { }
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)   { }
    #endregion
}
