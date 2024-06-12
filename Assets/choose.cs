using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class choose : MonoBehaviour
{
    public static bool ischoose = false;
    public static string objTag = "-1";
    private Vector3 halfExtents = new Vector3(0.05f, 0.05f, 0.05f);
    private Quaternion orientation = Quaternion.identity;
    private LayerMask layerMask = ~0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                foreach (RaycastResult result in results)
                {
                    if (result.gameObject == gameObject)
                    {
                        ischoose = true;
                        objTag = gameObject.tag;
                        int index = int.Parse(objTag);
                        Debug.Log("�}�l���l�˴��A����: " + Select.centerPosition + "�A�b���: " + halfExtents);

                        // �˴��s����ͦ���m�O�_����L����
                        Collider[] hitColliders = Physics.OverlapBox(Select.centerPosition, halfExtents, orientation, layerMask);

                        if (hitColliders.Length > 0)
                        {
                            Debug.Log("�b���w�ϰ줺�˴��쪫��:");
                            foreach (Collider hitCollider in hitColliders)
                            {
                                if (hitCollider != null && hitCollider.gameObject != null)
                                {
                                    string tag = hitCollider.gameObject.tag;
                                    Debug.Log("����W��: " + hitCollider.name + "�Atag: " + tag);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("�b���w�ϰ줺�S���˴��쪫��");
                            // �ͦ�����
                            GameObject spawnedObject = Instantiate(Select.objectPrefabsStatic[index], Select.centerPosition, Quaternion.identity);
                            // ����ͦ����� Renderer
                            Renderer objectRenderer = spawnedObject.GetComponent<Renderer>();
                            if (objectRenderer != null)
                            {
                                //������󪺳̩������y�� 
                                Vector3 minPoint = objectRenderer.bounds.min;
                                float minX = minPoint.x;
                                float minY = minPoint.y;
                                float minZ = minPoint.z;
                                // �վ�ͦ���m�A�Ϩ䩳���P���w��m���
                                Vector3 newPosition = new Vector3(Select.centerPosition.x, Select.centerPosition.y + Select.centerPosition.y- minY-0.1f, Select.centerPosition.z);
                                // ��s�ͦ����󪺦�m
                                spawnedObject.transform.position = newPosition;
                            }

                            // �P�ɥͦ��z�������I�������ߤ���
                            GameObject detectionCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            detectionCube.transform.position = Select.centerPosition;
                            detectionCube.transform.localScale = halfExtents * 2; // �]�w�ߤ��骺�j�p�������ϰ쪺�j�p
                            detectionCube.GetComponent<Renderer>().enabled = false; // �z����
                            detectionCube.AddComponent<BoxCollider>(); // �K�[�I����
                            detectionCube.tag = "DetectionCube"; // �]�w����
                            detectionCube.layer = LayerMask.NameToLayer("Ignore Raycast"); // �����g�u�˴�

                            Debug.Log("�I���쪺������Ҭ��G" + objTag);
                            Debug.Log("�O�_��ܡG" + ischoose);
                            break;
                        }
                    }
                }
            }
        }
    }
}
