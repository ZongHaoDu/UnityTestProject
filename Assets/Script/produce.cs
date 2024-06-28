/*using System.Collections.Generic;
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
                            centerPosition.y += 0.5f;
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
                        centerPosition.y += 0.5f;
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
            RaycastHit[] hits = Physics.RaycastAll(ray); // 獲取所有的碰撞結果
            RaycastHit hit = new RaycastHit(); // 初始化 hit
            GameObject hitObject = null;

            // 遍歷所有碰撞結果，找到第一個具有 tag 為 "land" 的物件
            foreach (RaycastHit h in hits)
            {
                if (h.collider.CompareTag("land"))
                {
                    hit = h;
                    hitObject = hit.collider.gameObject;
                    break;
                }
            }

            // 如果找到標籤為 "land" 的物件
            if (hitObject != null)
            {
                if (hitObject.transform.childCount > 0)
                {
                    Debug.Log(hit.collider.name);
                    Debug.Log("父物件已有子物件，刪除生成的物件");
                    Destroy(spawnedObject); // 摧毀生成的物件
                }
                else
                {
                    centerPosition = hit.collider.bounds.center;
                    centerPosition.y += 0.5f;
                    spawnedObject.transform.position = centerPosition;
                    spawnedObject.transform.SetParent(hitObject.transform);
                    spawnedObject.AddComponent<CapsuleCollider>();
                }
                isSet = true;
                state = "init";
            }
        }



        // 讓拖移生成的物件跟隨滑鼠移動
        if (state == "drag" && !isSet)
        {
            Debug.Log("物件拖移中");
            // Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
            //spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // 碰撞到的是 "land" 標籤的物件
                if (hit.collider.CompareTag("land"))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    centerPosition = hit.collider.bounds.center;
                    centerPosition.y += 0.5f;
                    spawnedObject.transform.position = centerPosition;
                }
            }
            Debug.Log("物件位置：" + spawnedObject.transform.position);
        }
    }
}
*/
using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Produce : MonoBehaviour
{
    public NetworkUser NetworkUser;

    public NetworkPrefabRef prefabRef;

    private string state; // 狀態標記，用於區分不同操作（"init"、"click"、"drag"）
    
    public List<NetworkPrefabRef> objectPrefabs; // 要生成的物件列表，通過 Inspector 設定
    private Ray ray; // 射線，用於捕捉鼠標點擊位置
    private RaycastHit hit; // 碰撞資訊，用於捕捉射線碰撞到的物件
    public static Vector3 centerPosition; // 中心位置，用於生成物件的位置
    private bool isSet; // 拖移物件是否已固定位置
    public static List<NetworkPrefabRef> objectPrefabsStatic; // 靜態變數，存儲要生成的物件列表
    public static GameObject parentObject; // 靜態變數，存儲點擊的物件作為新物件的父物件
    private GameObject spawnedObject; // 類級別變數，用於保存生成的物件

    private NetworkRunner _networkRunner;


    void Start()
    {
        InitializeState();
        Debug.Log("Spawn Local Item");

    }

    void SpawnNetworkObject(NetworkPrefabRef prefabRef, Vector3 position, Quaternion quaternion)
    {
        _networkRunner.Spawn(prefabRef, position, quaternion);
    }



    private void Awake()
    {
        //NetworkUser = GetComponent<NetworkUser>();
        _networkRunner = FindObjectOfType<NetworkRunner>();
    }

    void Update()
    {

        //if (_networkRunner != null && _networkRunner.IsRunning)
        //{
        //    // 生成物件
        //    _networkRunner.Spawn(prefabRef, position: Vector3.zero, rotation: Quaternion.identity);
        //}

        // 按下esc狀態回歸初始
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetToInitState();
        }

        //初始狀態設定，避免未知問題產生
        if (state == "init")
        {
            isSet = true;
        }

        // 檢查鼠標左鍵點擊事件
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        // 檢查鼠標左鍵釋放事件，用於固定拖移生成的物件
        //if (Input.GetMouseButtonUp(0) && !isSet)
        //{
        //    HandleMouseRelease();
        //}

        // 讓拖移生成的物件跟隨滑鼠移動
        //if (state == "drag" && !isSet)
        //{
        //    DragSpawnedObject();
        //}
    }

    public bool GetClickObject(out GameObject gameObject)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider != null)
        {
            if (hit.collider.CompareTag("land"))
            {
                gameObject = hit.collider.gameObject;
                return true;
            }
        }
        gameObject = null;
        return false;
    }

    private void InitializeState()
    {
        state = "init";
        objectPrefabsStatic = objectPrefabs;
    }

    private void ResetToInitState()
    {
        state = "init";
        Select.scriptUse = true;
        Debug.Log("狀態重置為 init");
    }

    private void HandleMouseClick()
    {
        if (state == "init")
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                TryInstantiateObjectFromUI();
            }
            else
            {
                SelectLandBlock();
            }
        }
        else if (state == "click")
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                TryInstantiateObjectFromUI();
            }
            else
            {
                SelectLandBlock();
            }
        }
    }

    private void TryInstantiateObjectFromUI()
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
            string objTag = result.gameObject.tag;
            Debug.Log("目前TAG" + objTag + "目前狀態" + state);
            if (int.TryParse(objTag, out int index) && index < objectPrefabsStatic.Count)
            {
                if (state == "init")
                {
                    //InstantiateObjectAtMousePosition(index);
                    state = "drag";
                    isSet = false;
                }
                else if (state == "click")
                {
                    InstantiateObjectAtCenterPosition(index);
                    state = "init";
                    Select.scriptUse = true;
                }
                return;
            }
        }
    }

    private void SelectLandBlock()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider != null)
        {
            if (hit.collider.CompareTag("land"))
            {
                centerPosition = hit.collider.bounds.center;
                centerPosition.y += 0.5f;
                parentObject = hit.collider.gameObject;
                state = "click";
                isSet = true;
            }
        }
    }

    //private void InstantiateObjectAtMousePosition(int index)
    //{


    //    spawnedObject = Instantiate(objectPrefabsStatic[index], Vector3.zero, Quaternion.identity);
    //    spawnedObject.tag = "plant";
    //    spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
    //    Debug.Log("生成物件：" + index);
    //}

    private void InstantiateObjectAtCenterPosition(int index)
    {
        if (parentObject != null && parentObject.transform.childCount > 0)
        {
            Debug.Log("父物件已有子物件，不生成新物件");
            return;
        }
        Debug.Log($"進入物件生成");
        SpawnNetworkObject(objectPrefabs[index], centerPosition, Quaternion.identity);
        //spawnedObject.AddComponent<CapsuleCollider>();
        //if (parentObject != null)
        //{
        //    spawnedObject.transform.SetParent(parentObject.transform);
        //}
        //spawnedObject.tag = "plant";
        Debug.Log("生成了新物件，標籤為：" + index);
    }

    private void HandleMouseRelease()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray); // 獲取所有的碰撞結果

        foreach (RaycastHit h in hits)
        {
            if (h.collider.CompareTag("land"))
            {
                if (h.collider.transform.childCount > 0)
                {
                    Destroy(spawnedObject);
                }
                else
                {
                    centerPosition = h.collider.bounds.center;
                    centerPosition.y += 0.5f;
                    spawnedObject.transform.position = centerPosition;
                    spawnedObject.transform.SetParent(h.collider.transform);
                    spawnedObject.AddComponent<CapsuleCollider>();
                }
                isSet = true;
                state = "init";
                return;
            }
        }
    }

    private void DragSpawnedObject()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("land"))
            {
                centerPosition = hit.collider.bounds.center;
                centerPosition.y += 0.5f;
                spawnedObject.transform.position = centerPosition;
            }
        }
        Debug.Log("物件位置：" + spawnedObject.transform.position);
    }
}
