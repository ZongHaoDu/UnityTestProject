using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Select_2 : MonoBehaviour
{
    public Material newMaterial;               // 點擊後要應用的新材質
    public Material originalMaterial;          // 原始材質，用於重置其他物件
    private Ray ray;                           // 射線，用於捕捉鼠標點擊位置
    private RaycastHit hit;                    // 碰撞資訊，用於捕捉射線碰撞到的物件
    public static bool scriptUse;              // 這個 script 是否啟用
    private GameObject lastSelected;           // 保存上次選中的物件
    public string tag;

    void Start()
    {
        lastSelected = null;
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
    }

    void Update()
    {
        Debug.Log("scriptuse"+scriptUse);
        // 檢查鼠標左鍵點擊
        if (scriptUse|| Input.GetMouseButtonDown(0))
        {
            // 檢查鼠標是否在 UI 元素上
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("鼠標在UI上，射線檢測跳過");
                return; // 如果鼠標在 UI 上，跳過射線檢測
            }
            Debug.Log("aaaaa");
            // 從主攝像機的位置創建一條射線指向鼠標點擊位置
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 使用射線檢測碰撞
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("選到的"+hit.collider.tag);
                // 碰撞到的是 "land" 標籤的物件
                if (hit.collider.CompareTag(tag))
                {
                    // 將上個選取的土地回復原材質
                    if (lastSelected != null)
                    {
                        Renderer lastRenderer = lastSelected.GetComponent<Renderer>();
                        if (lastRenderer != null)
                        {
                            lastRenderer.material = originalMaterial;
                        }
                    }

                    // 將點擊的物件設置為新材質
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = newMaterial;
                        lastSelected = hit.collider.gameObject;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        scriptUse = false;
                    }
                    
                }
                else
                {
                    // 碰撞到的不是 "land" 標籤的物件，恢復上次選中的物件為原始材質
                    if (lastSelected != null)
                    {
                        Renderer lastRenderer = lastSelected.GetComponent<Renderer>();
                        if (lastRenderer != null)
                        {
                            lastRenderer.material = originalMaterial;
                        }
                    }
                    lastSelected = null;
                    scriptUse = true;
                }
            }
        }
    }
}
