using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class choose : MonoBehaviour
{
    public static bool ischoose = false;  // �лx�O�_�w��ܪ���
    public static string objTag = "-1";   // �O�s��ܪ��󪺹Ϥ�����

    void Update()
    {
        //�ˬd���Х����I���]�Ω�첾�ͦ��^
        if (Input.GetMouseButtonDown(0))
        {
            // �ˬd���ЬO�_�I���b UI �����W
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                // �����Ҧ��I�����G
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                // �ˬd�I�����G�O�_�]�t��e����
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject == gameObject)
                    {
                        ischoose = true;
                        objTag = gameObject.tag;
                        int index = int.Parse(objTag);

                        // �ͦ��s����
                        GameObject spawnedObject = Instantiate(Select.objectPrefabsStatic[index], Input.mousePosition, Quaternion.identity);
                        // �N�ͦ����󪺼��ҳ]�m�� "plant"
                        spawnedObject.tag = "plant";
                        //�K�[�}��
                        spawnedObject.AddComponent<Product>();

                        

                        Debug.Log("�ͦ��F�s����A���Ҭ��G" + objTag);
                        Debug.Log("�O�_��ܡG" + ischoose);
                        break;
                    }
                }
            }
        }
        // �ˬd���Хk���I���]�Ω��I���ͦ��^
        if (Input.GetMouseButtonDown(1))
        {
            Select.scriptUse = true;
            // �ˬd���ЬO�_�I���b UI �����W
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                // �����Ҧ��I�����G
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                // �ˬd�I�����G�O�_�]�t��e����
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject == gameObject)
                    {
                        ischoose = true;
                        objTag = gameObject.tag;
                        int index = int.Parse(objTag);

                        // �ˬd������O�_�w���l����
                        if (Select.parentObject != null && Select.parentObject.transform.childCount > 0)
                        {
                            Debug.Log("������w���l����A���ͦ��s����");
                            return; // ������w���l����A���ͦ��s����
                        }

                        // �ͦ��s����
                        GameObject spawnedObject = Instantiate(Select.objectPrefabsStatic[index], Select.centerPosition, Quaternion.identity);

                        // �K�[ Capsule Collider
                        spawnedObject.AddComponent<CapsuleCollider>();

                        // �]�m�ͦ����󪺤�����
                        if (Select.parentObject != null)
                        {
                            spawnedObject.transform.SetParent(Select.parentObject.transform);
                        }
                        // �N�ͦ����󪺼��ҳ]�m�� "plant"
                        spawnedObject.tag = "plant";
                        // �վ�ͦ���m
                        Renderer objectRenderer = spawnedObject.GetComponent<Renderer>();
                        if (objectRenderer != null)
                        {
                            Vector3 minPoint = objectRenderer.bounds.min;
                            float minY = minPoint.y;
                            Vector3 newPosition = new Vector3(
                                Select.centerPosition.x,
                                Select.centerPosition.y + Select.centerPosition.y - minY - 0.1f,
                                Select.centerPosition.z
                            );
                            spawnedObject.transform.position = newPosition;
                        }
                        
                        Debug.Log("�ͦ��F�s����A���Ҭ��G" + objTag);
                        Debug.Log("�O�_��ܡG" + ischoose);
                        break;
                    }
                }
            }
        }
    }
}
