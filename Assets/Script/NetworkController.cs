using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : NetworkBehaviour
{
    [SerializeField]
    private NetworkPrefabRef prefabRef;


    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            if(data.state == NetworkInputState.Spawn)
            {
                Runner.Spawn(
                    prefabRef,
                    data.selectDirection + new Vector3(0,2,0),
                    Quaternion.identity,
                    Object.StateAuthority
                );
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
