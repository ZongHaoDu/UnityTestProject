using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    public List<GameObject> objectPrefabs;     // 要生成的物件列表
    public Material newMaterial;               // 點擊後要應用的新材質
    public Material originalMaterial;          // 原始材質，用於重置其他物件
    private Ray ray;                           // 射線，用於捕捉鼠標點擊位置
    private RaycastHit hit;                    // 碰撞資訊，用於捕捉射線碰撞到的物件
    public static Vector3 centerPosition;      // 中心位置，用於生成物件的位置
    public static List<GameObject> objectPrefabsStatic; // 靜態變數，存儲要生成的物件列表
    public static GameObject parentObject;     // 靜態變數，存儲點擊的物件作為新物件的父物件

    void Start()
    {
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
        objectPrefabsStatic = objectPrefabs;
    }

    void Update()
    {
        // 檢查鼠標左鍵點擊
        if (Input.GetMouseButtonDown(0))
        {
            // 從主攝像機的位置創建一條射線指向鼠標點擊位置
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 使用射線檢測碰撞
            if (Physics.Raycast(ray, out hit))
            {
                // 確保射線碰撞到的是當前物件並且物件標籤為 "land"
                if (hit.collider.gameObject == this.gameObject && hit.collider.CompareTag("land"))
                {
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        // 設置中心位置為碰撞物件的中心位置，並上移一點
                        centerPosition = renderer.bounds.center;
                        centerPosition.y += 0.6f;

                        // 將所有 "land" 標籤的物件恢復為原始材質
                        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                        foreach (Renderer otherRenderer in allRenderers)
                        {
                            if (otherRenderer != renderer && otherRenderer.CompareTag("land"))
                            {
                                otherRenderer.material = originalMaterial;
                            }
                        }

                        // 將點擊的物件設置為新材質
                        renderer.material = newMaterial;

                        // 設置父物件為擊中的物件
                        parentObject = hit.collider.gameObject;
                    }
                }
            }
        }
    }
}
