using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class produce : MonoBehaviour
{
    public GameObject objectPrefab;
    // Start is called before the first frame update
    void Start()
    {
        // �b�T�w����m�ͦ����󪺰ƥ�
        GameObject spawnedObject = Instantiate(objectPrefab, new Vector3(-1, 1, 0), Quaternion.Euler(0, 0, 0));

        // �i�H�b�o�̹�ͦ�������i���B�~�ާ@
        // �Ҧp�ק����B�W�[�}������
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
