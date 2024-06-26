using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    public Material newMaterial;               // 點擊後要應用的新材質
    public Material originalMaterial;          // 原始材質，用於重置其他物件
    private Ray ray;                           // 射線，用於捕捉鼠標點擊位置
    private RaycastHit hit;                    // 碰撞資訊，用於捕捉射線碰撞到的物件
    //public static Vector3 centerPosition;      // 中心位置，用於生成物件的位置
    //public static List<GameObject> objectPrefabsStatic; // 靜態變數，存儲要生成的物件列表
   // public static GameObject parentObject;     // 靜態變數，存儲點擊的物件作為新物件的父物件
    public static bool scriptUse; //這個script是否啟用
    void Start()
    {
        scriptUse = true;
        // 確保 originalMaterial 有值，如果沒有，嘗試自動設置為當前物件的材質
        if (originalMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
            }
        }

        // 初始化靜態變數 objectPrefabsStatic
        //objectPrefabsStatic = objectPrefabs;
    }

    void Update()
    {
        // 檢查鼠標左鍵點擊
        if (scriptUse|| Input.GetMouseButtonDown(0))
        {
            // 從主攝像機的位置創建一條射線指向鼠標點擊位置
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 使用射線檢測碰撞
            if (Physics.Raycast(ray, out hit))
            {
                // 碰撞到的是 "land" 標籤的物件
                if (hit.collider.CompareTag("land"))
                {
                    // 獲取所有 "land" 標籤的物件並恢復為原始材質
                    Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                    foreach (Renderer otherRenderer in allRenderers)
                    {
                        if (otherRenderer.CompareTag("land"))
                        {
                            otherRenderer.material = originalMaterial;
                        }
                    }

                    // 將點擊的物件設置為新材質
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = newMaterial;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        // 設置中心位置為碰撞物件的中心位置，並上移一點
                        //centerPosition = hit.collider.bounds.center;
                        //centerPosition.y += 0.6f;
                        //Debug.Log(centerPosition);
                        // 設置父物件為擊中的物件
                        //parentObject = hit.collider.gameObject;
                        scriptUse = false;
                    }
                    
                }
                else
                {
                    // 碰撞到的不是 "land" 標籤的物件，恢復所有 "land" 物件為原始材質
                    Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                    foreach (Renderer otherRenderer in allRenderers)
                    {
                        if (otherRenderer.CompareTag("land"))
                        {
                            otherRenderer.material = originalMaterial;
                        }
                    }
                    scriptUse = true;
                }
            }
        }
    }
}
