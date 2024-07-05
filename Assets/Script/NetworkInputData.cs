using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NetworkInputState
{
    Default,
    Spawn
}


public struct NetworkInputData : INetworkInput
{

    public NetworkInputState state;
    public Vector3 selectDirection;

}
