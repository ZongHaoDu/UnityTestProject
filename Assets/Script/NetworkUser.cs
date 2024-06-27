using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUser : NetworkBehaviour
{
    private NetworkRunner runner;

    // Start is called before the first frame update
    private void Awake()
    {
        runner = GetComponent<NetworkRunner>();
        Debug.Log("NetworkBehavior");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public NetworkObject SpawnObject(NetworkPrefabRef prefabRef, Vector3 position, Quaternion quaternion)
    {
        NetworkObject spawnedObject =  runner.Spawn(prefabRef, position, quaternion);

        return spawnedObject;
    }



}
