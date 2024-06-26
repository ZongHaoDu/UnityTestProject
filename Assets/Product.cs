using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Product : MonoBehaviour
{
    private bool isSet;                       // 標誌物件是否已經固定
    private Ray ray;                          // 射線，用於捕捉鼠標點擊位置
    private RaycastHit hit;                   // 碰撞資訊，用於捕捉射線碰撞到的物件
    private static Vector3 centerPosition;    // 中心位置，用於生成物件的位置
    private GameObject parentObject;          // 用於記錄生成物件的父物件

    void Start()
    {
        isSet = false; // 初始狀態為未固定

    }

    void Update()
    {
        // 讓生成物跟隨滑鼠移動
        if (!isSet)
        {
            // 將鼠標位置轉換為世界座標
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
            transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            Debug.Log(transform.position);
        }

        // 如果釋放鼠標左鍵並且物件未固定，進行射線檢測
        if (Input.GetMouseButtonUp(0) && !isSet)
        {
            Debug.Log(Input.mousePosition);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 使用射線檢測碰撞
            if (Physics.Raycast(ray, out hit))
            {
                // 取得碰撞到的物件
                GameObject hitObject = hit.collider.gameObject;

                // 檢查父物件是否已有子物件
                if (hitObject.transform.childCount > 0 || !hit.collider.CompareTag("land"))
                {
                    Debug.Log("父物件已有子物件，不生成新物件");
                    Destroy(gameObject); // 摧毀生成的物件
                }
                else
                {
                    // 設置物件位置
                    centerPosition = hit.collider.bounds.center;
                    centerPosition.y += 0.6f;
                    transform.position = centerPosition;

                    // 設置碰撞物件為生成物件的父物件
                    transform.SetParent(hitObject.transform);

                    // 添加 Capsule Collider
                    gameObject.AddComponent<CapsuleCollider>();


                    isSet = true; // 將物件設置為已固定狀態
                }
            }
        }
    }
}
