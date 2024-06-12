using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produce : MonoBehaviour
{
    public GameObject objectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // 在固定的位置生成物件的副本
        GameObject spawnedObject = Instantiate(objectPrefab, new Vector3(-1, 1, 0), Quaternion.Euler(0, 0, 0));

        // 可以在這裡對生成的物件進行額外操作
        // 例如修改材質、增加腳本等等
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
