using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalBloodBar : MonoBehaviour
{
    private RectTransform healthBarRect; // 血條的 RectTransform 組件
    private float targetHealth = 1f; // 目標血量（0到1之間），初始設置為滿血
    private float currentHealth; // 當前血量
    public float decreasePercentage = 0.1f; // 每次碰撞扣除的血量百分比
    public Color startColor = Color.green; // 血條滿時的顏色
    public float moveSpeed = 0.1f;
    public int bloodValue; // 血量參數
    public float cooldownTime = 0.001f; // 冷卻時間
    private float nextAllowedCollisionTime = 0f; // 下一次允許碰撞的時間

    void Start()
    {
        // 在物件的子物件中查找 RectTransform 組件（假設血條圖片是 AnimalBloodBar 物件的子物件）
        healthBarRect = GetComponentInChildren<RectTransform>();

        // 設置初始血量為滿血
        currentHealth = targetHealth;

        // 更新血條顯示
        UpdateHealthBar();

        // 獲取血量參數
        bloodValue = ParameterQuery.QueryParameters(gameObject, "blood");
    }

    // 當碰撞到東西時扣除血量
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObject = collision.gameObject;

        // 如果碰撞的是 land 或者還在冷卻時間內，則返回
        if (otherObject.tag == "land" || Time.time < nextAllowedCollisionTime)
        {
            return;
        }

        // 獲取攻擊參數
        int attackValue = ParameterQuery.QueryParameters(otherObject, "attack");

        if (bloodValue != 0) // 防止除以零
        {
            decreasePercentage = (float)attackValue / bloodValue; // 更新 decreasePercentage
            DecreaseHealth();

            // 修改 Transform.position 的正確方法
            Vector3 newPosition = transform.position;
            newPosition.x -= 1;
            transform.position = newPosition;

            // 更新冷卻時間
            nextAllowedCollisionTime = Time.time + cooldownTime;
        }
    }

    // 扣除血量
    private void DecreaseHealth()
    {
        float amountToDecrease = targetHealth * decreasePercentage; // 計算要扣除的血量

        currentHealth -= amountToDecrease; // 扣除血量

        // 限制血量在 0 到 1 之間
        currentHealth = Mathf.Clamp(currentHealth, 0f, 1f);

        // 更新血條顯示
        UpdateHealthBar();
    }

    // 更新血條顯示
    private void UpdateHealthBar()
    {
        // 設置血條的 x 軸 scale 為當前血量的百分比
        healthBarRect.localScale = new Vector3(currentHealth, healthBarRect.localScale.y, healthBarRect.localScale.z);

        Debug.Log("Current Health: " + currentHealth);
    }

    // 在每個幀更新物件位置
    private void Update()
    {
        Move(); // 調用移動方法
        if (currentHealth < 0.0001)
        {
            Destroy(gameObject);
        }
    }

    //模擬移動用(之後要拿掉)
    private void Move()
    {
        Vector3 currentPosition = transform.position;

        if (Input.GetKey(KeyCode.D))
            currentPosition.x += moveSpeed; // 往右
        if (Input.GetKey(KeyCode.A))
            currentPosition.x -= moveSpeed; // 往左
        if (Input.GetKey(KeyCode.W))
            currentPosition.z += moveSpeed; // 往前
        if (Input.GetKey(KeyCode.S))
            currentPosition.z -= moveSpeed; // 往後

        transform.position = currentPosition;
    }
}
