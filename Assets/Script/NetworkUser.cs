/*using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkUser : NetworkBehaviour
{
    public NetworkPrefabRef PrefabRef;

    private NetworkRunner runner;
    private Produce LocalController;
     


    // Start is called before the first frame update
    private async void Awake()
    {
        runner = GetComponent<NetworkRunner>();
        LocalController = FindObjectOfType<Produce>();

    }

    public void SpawnBuilding()
    {
        GameObject gameObject;
        if (LocalController.GetClickObject(out gameObject))
        {
            Debug.Log($"{runner}");

            SpawnObject(PrefabRef, gameObject.transform.position, Quaternion.identity);
        }


    }

    public override void FixedUpdateNetwork()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.Left))
        {
            Debug.Log("QQ");
            SpawnBuilding();
        }
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
*/