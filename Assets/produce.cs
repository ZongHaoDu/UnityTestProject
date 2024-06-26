using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Produce : MonoBehaviour
{
    private string state; // 狀態標記，用於區分不同操作（"init"、"click"、"drag"）
    public List<GameObject> objectPrefabs; // 要生成的物件列表，通過 Inspector 設定
    private Ray ray; // 射線，用於捕捉鼠標點擊位置
    private RaycastHit hit; // 碰撞資訊，用於捕捉射線碰撞到的物件
    public static Vector3 centerPosition; // 中心位置，用於生成物件的位置
    private bool isSet; // 拖移物件是否已固定位置
    public static List<GameObject> objectPrefabsStatic; // 靜態變數，存儲要生成的物件列表
    public static GameObject parentObject; // 靜態變數，存儲點擊的物件作為新物件的父物件
    private GameObject spawnedObject; // 類級別變數，用於保存生成的物件

    void Start()
    {
        // 初始狀態設置
        state = "init";
        // 初始化靜態變數 objectPrefabsStatic，確保在所有實例中保持一致
        objectPrefabsStatic = objectPrefabs;
    }

    void Update()
    {
        // 按下esc狀態回歸初始
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = "init";
            Select.scriptUse = true;
            Debug.Log("狀態重置為 init");
        }
        //初始狀態設定，避免未知問題產生
        if(state == "init")
        {
            isSet = true;
        }
        // 檢查鼠標左鍵點擊事件
        if (Input.GetMouseButtonDown(0))
        {
            if (state == "init")
            {
                // 檢查鼠標是否點擊在 UI 元素上（拖移生成用）
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };

                    // 收集所有點擊結果
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);

                    foreach (RaycastResult result in results)
                    {
                        // 獲取被點擊圖片的標籤，並生成對應的預製物件
                        string objTag = result.gameObject.tag;
                        if (int.TryParse(objTag, out int index) && index < objectPrefabsStatic.Count)
                        {
                            // 生成新物件，生成位置為鼠標點擊位置
                            spawnedObject = Instantiate(objectPrefabsStatic[index], Vector3.zero, Quaternion.identity);
                            spawnedObject.tag = "plant";
                            spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
                            state = "drag";
                            isSet = false;
                            Debug.Log("生成物件：" + objTag);
                            return; // 點擊了 UI 元素後跳出循環
                        }
                    }
                }
                else
                {
                    // 選取方塊（用於點擊生成）
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.collider != null)
                    {
                        Debug.Log("碰撞到物件：" + hit.collider.gameObject.name);
                        if (hit.collider.CompareTag("land"))
                        {
                            Debug.Log("選取到方塊");
                            centerPosition = hit.collider.bounds.center;
                            centerPosition.y += 0.6f;
                            parentObject = hit.collider.gameObject;
                            state = "click";
                            isSet = true;
                        }
                    }

                }
            }
            //如果已經有選取方塊
            else if (state == "click")
            {
                //如果點取其他方塊
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.collider != null)
                {
                    Debug.Log("碰撞到物件：" + hit.collider.gameObject.name);
                    if (hit.collider.CompareTag("land"))
                    {
                        Debug.Log("選取到方塊");
                        centerPosition = hit.collider.bounds.center;
                        centerPosition.y += 0.6f;
                        parentObject = hit.collider.gameObject;
                        state = "click";
                        isSet = true;
                    }
                }
                //如果點擊到圖片
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };

                    // 收集所有點擊結果
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);

                    foreach (RaycastResult result in results)
                    {
                        // 獲取被點擊圖片的標籤，並生成對應的預製物件
                        string objTag = result.gameObject.tag;
                        if (int.TryParse(objTag, out int index) && index < objectPrefabsStatic.Count)
                        {
                            Debug.Log("圖片 " + index);
                            // 檢查父物件是否已有子物件
                            if (parentObject != null && parentObject.transform.childCount > 0)
                            {
                                Debug.Log("父物件已有子物件，不生成新物件");
                                state = "init";
                                return; // 父物件已有子物件，不生成新物件
                            }

                            // 生成新物件，生成位置為之前記錄的中心位置
                            spawnedObject = Instantiate(objectPrefabsStatic[index], centerPosition, Quaternion.identity);
                            spawnedObject.AddComponent<CapsuleCollider>();
                            if (parentObject != null)
                            {
                                spawnedObject.transform.SetParent(parentObject.transform);
                            }
                            spawnedObject.tag = "plant";
                            Renderer objectRenderer = spawnedObject.GetComponent<Renderer>();
                            if (objectRenderer != null)
                            {
                                Vector3 minPoint = objectRenderer.bounds.min;
                                float minY = minPoint.y;
                                Vector3 newPosition = new Vector3(
                                    centerPosition.x,
                                    centerPosition.y + centerPosition.y - minY - 0.1f,
                                    centerPosition.z
                                );
                                spawnedObject.transform.position = newPosition;
                            }
                            Debug.Log("生成了新物件，標籤為：" + objTag);
                            // 重置狀態為 "init"
                            state = "init";
                            // 啟用 Select script 
                            Select.scriptUse = true;
                            break;
                        }
                    }
                }
            }
        }

        // 檢查鼠標左鍵釋放事件，用於固定拖移生成的物件
        if (Input.GetMouseButtonUp(0) && !isSet)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.transform.childCount > 0 || !hit.collider.CompareTag("land"))
                {
                    Debug.Log("父物件已有子物件或點擊的不是 'land'，刪除生成的物件");
                    Destroy(spawnedObject); // 摧毀生成的物件
                    isSet = true;
                    state = "init";
                }
                else
                {
                    centerPosition = hit.collider.bounds.center;
                    centerPosition.y += 0.6f;
                    spawnedObject.transform.position = centerPosition;
                    spawnedObject.transform.SetParent(hitObject.transform);
                    spawnedObject.AddComponent<CapsuleCollider>();
                    isSet = true;
                    state = "init";
                }
            }
        }

        // 讓拖移生成的物件跟隨滑鼠移動
        if (state == "drag" && !isSet)
        {
            Debug.Log("物件拖移中");
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
            spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            Debug.Log("物件位置：" + spawnedObject.transform.position);
        }
    }
}
