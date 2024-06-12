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
                        Debug.Log("開始盒子檢測，中心: " + Select.centerPosition + "，半邊長: " + halfExtents);

                        // 檢測新物件生成位置是否有其他物體
                        Collider[] hitColliders = Physics.OverlapBox(Select.centerPosition, halfExtents, orientation, layerMask);

                        if (hitColliders.Length > 0)
                        {
                            Debug.Log("在指定區域內檢測到物體:");
                            foreach (Collider hitCollider in hitColliders)
                            {
                                if (hitCollider != null && hitCollider.gameObject != null)
                                {
                                    string tag = hitCollider.gameObject.tag;
                                    Debug.Log("物體名稱: " + hitCollider.name + "，tag: " + tag);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("在指定區域內沒有檢測到物體");
                            // 生成物件
                            GameObject spawnedObject = Instantiate(Select.objectPrefabsStatic[index], Select.centerPosition, Quaternion.identity);
                            // 獲取生成物件的 Renderer
                            Renderer objectRenderer = spawnedObject.GetComponent<Renderer>();
                            if (objectRenderer != null)
                            {
                                //獲取物件的最底部的座標 
                                Vector3 minPoint = objectRenderer.bounds.min;
                                float minX = minPoint.x;
                                float minY = minPoint.y;
                                float minZ = minPoint.z;
                                // 調整生成位置，使其底部與指定位置對齊
                                Vector3 newPosition = new Vector3(Select.centerPosition.x, Select.centerPosition.y + Select.centerPosition.y- minY-0.1f, Select.centerPosition.z);
                                // 更新生成物件的位置
                                spawnedObject.transform.position = newPosition;
                            }

                            // 同時生成透明的有碰撞器的立方體
                            GameObject detectionCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            detectionCube.transform.position = Select.centerPosition;
                            detectionCube.transform.localScale = halfExtents * 2; // 設定立方體的大小為偵測區域的大小
                            detectionCube.GetComponent<Renderer>().enabled = false; // 透明化
                            detectionCube.AddComponent<BoxCollider>(); // 添加碰撞器
                            detectionCube.tag = "DetectionCube"; // 設定標籤
                            detectionCube.layer = LayerMask.NameToLayer("Ignore Raycast"); // 忽略射線檢測

                            Debug.Log("點擊到的物件標籤為：" + objTag);
                            Debug.Log("是否選擇：" + ischoose);
                            break;
                        }
                    }
                }
            }
        }
    }
}
