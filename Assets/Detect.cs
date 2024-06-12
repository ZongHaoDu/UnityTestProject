using UnityEngine;

public class Detect : MonoBehaviour
{
    private Vector3 center = new Vector3(0, 0.6f, 0); // 中心位置
    private Vector3 halfExtents = new Vector3(0.01f, 0.01f, 0.01f); // 縮小的長方體半邊長
    private Quaternion orientation = Quaternion.identity; // 長方體的旋轉
    private LayerMask layerMask = ~0; // 預設檢測所有圖層

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 按下空白鍵來進行檢測
        {
            Debug.Log("開始盒子檢測，中心: " + center + "，半邊長: " + halfExtents);

            // 使用 Physics.OverlapBox 檢測區域內的碰撞體
            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask);

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
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, halfExtents * 2);
    }
}
