/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform target; // 追蹤的目標物體
    public float speed = 1.0f; // 向量長度
    public string requiredTag = "land"; // 地面需要的標籤

    private Vector3 directionToTarget; // 當前物件到 target 的向量
    private List<Vector3> rotatedDirections; // 存儲不同角度下的旋轉後向量
    private List<Vector3> positions; // 存儲每個旋轉後位置的向量

    void Start()
    {
        // 初始化
        rotatedDirections = new List<Vector3>();
        positions = new List<Vector3>();
        directionToTarget = target.position - transform.position; // 計算當前物件到 target 的向量
        // 計算不同角度的旋轉後向量和位置向量
        CalculateRotatedDirections();
        CalculatePositions();
    }

    void Update()
    {
        // 計算不同角度的旋轉後向量和位置向量
        CalculateRotatedDirections();
        CalculatePositions();
        // 射線檢測並打印結果
        RaycastHit hitInfo;
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 startPosition = transform.position + rotatedDirections[i]; // 計算射線起始位置
            Vector3 direction = -Vector3.up; // 射線方向向下

            // 射線檢測
            if (Physics.Raycast(startPosition, direction, out hitInfo))
            {
                Debug.Log("射到的物件 (位置" + (i + 1) + "): " + hitInfo.collider.gameObject.tag + "，位置：" + hitInfo.point);

                // 判斷射到的物件的標籤，並移動物體
                if (hitInfo.collider.gameObject.CompareTag(requiredTag))
                {
                    Debug.Log("移動到位置" + (i + 1) + ": " + positions[i]);
                    transform.position = positions[i];
                    return; // 優先級最高的位置檢測到即可移動，結束函數
                }
            }
            else
            {
                Debug.Log("未射到物件 (位置" + (i + 1) + ")");
            }
        }
    }

    // 計算不同角度的旋轉後向量
    void CalculateRotatedDirections()
    {
        rotatedDirections.Clear();

        // 按照優先順序添加不同角度的旋轉後向量
        rotatedDirections.Add((Quaternion.Euler(0, 0, 0) * directionToTarget).normalized * speed); // 0度
        rotatedDirections.Add((Quaternion.Euler(0, 30, 0) * directionToTarget).normalized * speed); // 30度
        rotatedDirections.Add((Quaternion.Euler(0, -30, 0) * directionToTarget).normalized * speed); // -30度
        rotatedDirections.Add((Quaternion.Euler(0, 60, 0) * directionToTarget).normalized * speed); // 60度
        rotatedDirections.Add((Quaternion.Euler(0, -60, 0) * directionToTarget).normalized * speed); // -60度
        rotatedDirections.Add((Quaternion.Euler(0, 90, 0) * directionToTarget).normalized * speed); // 90度
        rotatedDirections.Add((Quaternion.Euler(0, -90, 0) * directionToTarget).normalized * speed); // -90度
        rotatedDirections.Add((Quaternion.Euler(0, 120, 0) * directionToTarget).normalized * speed); // 120度
        rotatedDirections.Add((Quaternion.Euler(0, -120, 0) * directionToTarget).normalized * speed); // -120度
        rotatedDirections.Add((Quaternion.Euler(0, 150, 0) * directionToTarget).normalized * speed); // 150度
        rotatedDirections.Add((Quaternion.Euler(0, -150, 0) * directionToTarget).normalized * speed); // -150度
        rotatedDirections.Add((Quaternion.Euler(0, 180, 0) * directionToTarget).normalized * speed); // 180度
        rotatedDirections.Add((Quaternion.Euler(0, -180, 0) * directionToTarget).normalized * speed); // -180度
    }

    // 計算不同角度的位置向量
    void CalculatePositions()
    {
        positions.Clear();

        // 計算每個旋轉後的位置向量
        foreach (Vector3 rotatedDirection in rotatedDirections)
        {
            positions.Add(transform.position + rotatedDirection);
            Debug.Log(rotatedDirection);
        }
    }

    
}
*/
using UnityEngine;
using System.Collections.Generic;

public class Move : MonoBehaviour
{
    public float speed = 0.05f; // 前進速度
    public float maxRotationAngle = 90f; // 最大旋轉角度限制
    private Rigidbody rb; // 物體的剛體
    public float distance = 2.0f; // ray判斷距離
    private string requiredTag = "Road";
    private List<Vector3> rotatedDirections; // 存儲不同角度下的旋轉後向量
    private List<Vector3> positions; // 存儲每個旋轉後位置的向量
    public Transform target; // 追蹤的目標物體
    private Vector3 directionToTarget; // 當前物件到 target 的向量
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 獲取 Rigidbody 組件
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // 限制 X 和 Z 軸的旋轉
        // 初始化
        directionToTarget = target.position - transform.position; // 計算當前物件到 target 的向量
        rotatedDirections = new List<Vector3>();
        positions = new List<Vector3>();
    }

    void FixedUpdate()
    {
        // 保持在 Y 軸上的旋轉角度限制
        Vector3 eulerAngleVelocity = new Vector3(0, Input.GetAxis("Horizontal") * maxRotationAngle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);

        // 旋轉前計算新的方向
        Vector3 newForward = deltaRotation * transform.forward;
        float angle = Vector3.Angle(newForward, directionToTarget);

        if (angle <= 180f)
        {
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        // 計算不同角度的旋轉後向量和位置向量
        CalculateRotatedDirections();
        CalculatePositions();

        // 檢測每個旋轉後的位置是否符合條件
        RaycastHit hitInfo;
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 startPosition = transform.position + rotatedDirections[i]; // 計算射線起始位置
            Vector3 direction = -Vector3.up; // 射線方向向下

            // 射線檢測
            if (Physics.Raycast(startPosition, direction, out hitInfo))
            {
                // 判斷射到的物件的標籤，並移動物體
                if (hitInfo.collider.gameObject.CompareTag(requiredTag))
                {
                    Debug.Log(i);
                    if (i == 0)
                    {
                        // 在物體的正前方移動
                        rb.MovePosition(rb.position + transform.forward * speed);
                    }
                    else if (i == 1)
                    {
                        Quaternion newRotation = Quaternion.Euler(0, -90, 0);
                        Vector3 rotatedForward = newRotation * transform.forward;
                        if (Vector3.Angle(rotatedForward, directionToTarget) <= 170f)
                        {
                            rb.MoveRotation(rb.rotation * newRotation);
                            // 同時調整物體的前方向量
                            transform.forward = newRotation * transform.forward;
                        }
                    }
                    else if (i == 2)
                    {
                        Quaternion newRotation = Quaternion.Euler(0, 90, 0);
                        Vector3 rotatedForward = newRotation * transform.forward;
                        if (Vector3.Angle(rotatedForward, directionToTarget) <= 170f)
                        {
                            rb.MoveRotation(rb.rotation * newRotation);
                            // 同時調整物體的前方向量
                            transform.forward = newRotation * transform.forward;
                        }
                    }

                    // 停止物體旋轉
                    rb.angularVelocity = Vector3.zero;

                    return; // 優先級最高的位置檢測到即可移動，結束函數
                }
            }
        }
    }

    // 計算不同角度的旋轉後向量
    void CalculateRotatedDirections()
    {
        rotatedDirections.Clear();

        // 獲取前方向量，即物體的正前方
        Vector3 forwardDirection = rb.transform.forward;
        rotatedDirections.Add(forwardDirection * distance); // 0度

        forwardDirection = -rb.transform.right;
        rotatedDirections.Add(forwardDirection * distance); // 90度

        forwardDirection = rb.transform.right;
        rotatedDirections.Add(forwardDirection * distance); // -90度
    }

    // 計算不同角度的位置向量
    void CalculatePositions()
    {
        positions.Clear();

        // 計算每個旋轉後的位置向量
        foreach (Vector3 rotatedDirection in rotatedDirections)
        {
            positions.Add(transform.position + rotatedDirection);
        }
    }
}
