using Fusion;
using Fusion.Sockets;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SyncScript : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner networkRunner;


    [SerializeField] 
    private NetworkPrefabRef networkPrefabRef;
    [SerializeField]
    private NetworkPrefabRef userController;

    private Dictionary<PlayerRef, NetworkObject> spawnCharacter = new Dictionary<PlayerRef, NetworkObject>();

    
    public void InitGameRoom(NetworkRunner networkRunner)
    {
        networkRunner.Spawn(networkPrefabRef);
    }


    async void JoinLobby()
    {
        networkRunner.JoinSessionLobby(SessionLobby.Shared);
    }



    async void GameStart(GameMode gameMode, string sessionName)
    {
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        networkRunner.ProvideInput = true;
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }
        await networkRunner.StartGame(new StartGameArgs()
        {
            
            GameMode = gameMode,
            SessionName = sessionName,
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            PlayerCount = 2,
            OnGameStarted = InitGameRoom,
            IsVisible = true,
            IsOpen = true
        });

        


    }

    private void OnGUI()
    {
        if (networkRunner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                GameStart(GameMode.Host, "DefalutRoom");
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                GameStart(GameMode.Client, "DefalutRoom");
            }
            if(GUI.Button(new Rect (0, 80, 200, 40), "List"))
            {
                GameStart(GameMode.Shared, "Shared");
            }

        }
        else
        {
    
        }


    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        //if(runner.IsServer)
        //{
        //    SpawnMap(networkPrefabRef, runner);
        //}
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        NetworkObject controller = runner.Spawn(userController, Vector3.zero, Quaternion.identity, player);

        spawnCharacter.Add(player, controller);

    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if(spawnCharacter.TryGetValue(player, out  NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnCharacter.Remove(player);
        }


    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInput(NetworkRunner runner, Fusion.NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, Fusion.NetworkInput input)
    {
        throw new NotImplementedException();
    }
}
