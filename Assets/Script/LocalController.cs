using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalController : MonoBehaviour, INetworkRunnerCallbacks
{
    private GameObject selectedObject;
    private NetworkInputData inputData;


    public void Update()
    {
        inputData = new NetworkInputData();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHit = Physics.Raycast(ray, out hit);
       
        if (!isHit)
        {
            return;
        }
        selectedObject = hit.collider.gameObject;


        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            inputData.buttons.Set(NetworkInputState.SpawnBuilding, true);
            inputData.selectObjectId = selectedObject.GetComponent<NetworkObject>().Id;
            Debug.Log(inputData.selectObjectId);
        }

    }


    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
        input.Set(inputData);
    }




    #region ¥¼¨Ï¥ÎEVENT
    public void OnConnectedToServer(NetworkRunner runner)
    {
       
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
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, Fusion.NetworkInput input)
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
       
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
       
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
    #endregion

}
