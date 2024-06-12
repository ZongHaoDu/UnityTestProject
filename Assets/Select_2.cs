using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_2 : MonoBehaviour
{
    public List<GameObject> objectPrefabs;  // 要生成的物件列表
    public Material newMaterial;            // 新材質
    public Material originalMaterial;       // 原始材質
    private Ray ray;                        // 射線
    private RaycastHit hit;                 // 碰撞資訊
    public static Vector3 centerPosition;
    public static List<GameObject> objectPrefabsStatic;  // 靜態變數
    private Vector3 checkPosition;          // 檢查位置
    private Vector3 checkHalfExtents;       // 檢查長方體半邊長

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

        // 初始化檢查位置和半邊長
        checkHalfExtents = new Vector3(0.01f, 0.01f, 0.01f);
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
                if (hit.collider.gameObject == this.gameObject)
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
                            if (otherRenderer != renderer)
                            {
                                otherRenderer.material = originalMaterial;
                            }
                        }

                        // 更換被擊中的物體的材質
                        renderer.material = newMaterial;

                        // 設置檢查位置為選取物位置
                        checkPosition = centerPosition;

                        // 檢查是否已經有物體存在於檢查位置
                        if (!CheckObjectExists())
                        {
                            // 生成新物件
                            GenerateObjects(centerPosition);
                        }
                        else
                        {
                            Debug.Log("檢查位置已經有物體存在，無法生成新物件");
                        }
                    }
                }
            }
        }
    }

    void GenerateObjects(Vector3 position)
    {
        foreach (GameObject prefab in objectPrefabs)
        {
            GameObject newObject = Instantiate(prefab, position, Quaternion.identity);

            // 添加膠囊碰撞體
            CapsuleCollider collider = newObject.AddComponent<CapsuleCollider>();
            // 調整碰撞體參數，例如高度和半徑
            collider.height = 2f; // 膠囊的高度
            collider.radius = 0.5f; // 膠囊的半徑
        }
    }

    bool CheckObjectExists()
    {
        // 使用 Physics.OverlapBox 檢測檢查位置附近是否有物體存在
        Collider[] hitColliders = Physics.OverlapBox(checkPosition, checkHalfExtents, Quaternion.identity);

        // 若有物體存在則返回 true
        return hitColliders.Length > 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(checkPosition, checkHalfExtents * 2);
    }
}
