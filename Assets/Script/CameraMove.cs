using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Ray ray; // 射線
    private RaycastHit hit; // 用來存儲射線檢測到的對象信息
    private Vector3 FirstVector;
    private Vector3 SecondVector;

    public float zoomSpeed = 10f; // 縮放速度
    public float moveSpeed = 0.005f; // x、y移動速度
    private string now = "undefine";
    // Update is called once per frame
    void Start()
    {
        now = "undefine";
    }
    void Update()
    {
        if (now != "undefine")
        {
            Vector3 currentPosition = Camera.main.transform.position;
            if (now == "D")
                currentPosition.x += moveSpeed; // 往右
            if (now == "A")
                currentPosition.x -= moveSpeed; // 往左
            if (now == "W")
                currentPosition.z += moveSpeed; // 往前
            if (now == "S")
                currentPosition.z -= moveSpeed; // 往後
            Camera.main.transform.position = currentPosition; // 將修改後的位置賦值給相機
        }
        if (Input.GetKeyUp(KeyCode.D)|| Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            now = "undefine";
        }
        if (Input.anyKeyDown)
        {
            string inputLetter = Input.inputString;
            now = inputLetter.ToUpper();
            if (!string.IsNullOrEmpty(inputLetter))
            {
                Debug.Log("按下的鍵盤字母為：" + inputLetter);
            }
        }

        /*
        // 檢測滑鼠左鍵按下
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // 記錄相機位置到撞擊點的向量
                FirstVector = hit.point - Camera.main.transform.position;
                Debug.Log("First Vector: " + FirstVector);
            }
        }

        // 檢測滑鼠左鍵釋放
        if (Input.GetMouseButtonUp(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // 記錄相機位置到撞擊點的向量
                SecondVector = hit.point - Camera.main.transform.position;
                Debug.Log("Second Vector: " + SecondVector);

                // 計算兩個向量的差
                Vector3 movementVector = SecondVector - FirstVector;
                Debug.Log("Movement Vector: " + movementVector);

                // 移動相機
                Camera.main.transform.position -= movementVector;
            }
        }
        */


        // 檢測滑鼠滾輪滾動
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            // 計算新的相機位置
            Vector3 direction = Camera.main.transform.forward; // 根據相機的方向移動
            Vector3 newPosition = Camera.main.transform.position + direction * scroll * zoomSpeed;

            // 更新相機位置
            Camera.main.transform.position = newPosition;
        }
    }
}
