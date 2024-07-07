using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : NetworkBehaviour
{
    [SerializeField]
    private NetworkPrefabRef prefabRef;
    
    private void SpawnBuilding(NetworkInputData data)
    {
        bool isSelect = Runner.TryFindObject(data.selectObjectId, out NetworkObject selectObject);

        if (!isSelect)
        {
            return;
        }

        NetworkObject spawnObject =  Runner.Spawn(
                    prefabRef,
                    selectObject.transform.position + new Vector3(0 ,(float) 0.5 ,0),
                    Quaternion.identity,
                    Object.StateAuthority
                );

        spawnObject.transform.SetParent(selectObject.transform);

    }

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            if(data.buttons.IsSet(NetworkInputState.SpawnBuilding))
            {
                SpawnBuilding(data);
            }
        }


    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
