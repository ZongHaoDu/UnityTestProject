using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class choose : MonoBehaviour
{
    public static bool ischoose = false;  // 標誌是否已選擇物件
    public static string objTag = "-1";   // 保存選擇物件的圖片標籤

    void Update()
    {
        // 檢查鼠標左鍵點擊
        if (Input.GetMouseButtonDown(0))
        {
            // 檢查鼠標是否點擊在 UI 元素上
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;

                // 收集所有點擊結果
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, results);

                // 檢查點擊結果是否包含當前物件
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject == gameObject)
                    {
                        ischoose = true;
                        objTag = gameObject.tag;
                        int index = int.Parse(objTag);

                        // 檢查父物件是否已有子物件
                        if (Select.parentObject != null && Select.parentObject.transform.childCount > 0)
                        {
                            Debug.Log("父物件已有子物件，不生成新物件");
                            return; // 父物件已有子物件，不生成新物件
                        }

                        // 生成新物件
                        GameObject spawnedObject = Instantiate(Select.objectPrefabsStatic[index], Select.centerPosition, Quaternion.identity);

                        // 設置生成物件的父物件
                        if (Select.parentObject != null)
                        {
                            spawnedObject.transform.SetParent(Select.parentObject.transform);
                        }
                        // 將生成物件的標籤設置為 "plant"
                        spawnedObject.tag = "plant";
                        // 調整生成位置
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

                        Debug.Log("生成了新物件，標籤為：" + objTag);
                        Debug.Log("是否選擇：" + ischoose);
                        break;
                    }
                }
            }
        }
    }
}
