using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NetworkInputState
{
    Default,
    SpawnBuilding
}


public struct NetworkInputData : INetworkInput
{
    public NetworkId selectObjectId;
    public NetworkButtons buttons;
    public Vector3 selectDirection;

}
