using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    public List<GameObject> objectPrefabs;  // 要生成的物件列表
    public Material newMaterial;            // 新材質
    public Material originalMaterial;       // 原始材質
    private Ray ray;                        // 射線
    private RaycastHit hit;                 // 碰撞資訊
    public static Vector3 centerPosition;
    public static List<GameObject> objectPrefabsStatic;  // 靜態變數

    void Start()
    {
        if (originalMaterial == null)
        {
            // 確保原始材質已經設置，如果沒有設置則自動獲取當前物體的材質
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
            }
        }

        // 初始化靜態變數
        objectPrefabsStatic = objectPrefabs;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 使用主攝像機創建一根射線，射線的方向是我們鼠標點擊的位置
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 使用物理類檢查射線的碰撞，若有打中則回傳 true
            if (Physics.Raycast(ray, out hit))
            {
                // 確認射線是否碰撞到當前掛載腳本的物件
                if (hit.collider.gameObject == this.gameObject&&hit.collider.CompareTag("land"))
                {
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        centerPosition = renderer.bounds.center; // 取得選取物位置

                        centerPosition.y += 0.6f;

                        // 在更換材質之前，將所有其他方塊的材質恢復為原始材質
                        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                        foreach (Renderer otherRenderer in allRenderers)
                        {
                            if (otherRenderer != renderer && otherRenderer.tag=="land")
                            {
                                otherRenderer.material = originalMaterial;
                            }
                        }

                        // 更換被擊中的物體的材質
                        renderer.material = newMaterial;
                    }
                }
            }
        }
    }
}
