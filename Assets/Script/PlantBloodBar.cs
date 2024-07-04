using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantBloodBar : MonoBehaviour
{
    private Transform healthBar; // 血條物件（綁定腳本的物件）
    private Image healthBarImage; // 血條的 Image 組件
    private float fillDuration = 5.0f; // 血條充滿所需的時間（秒）
    private float elapsedTime = 0f; // 已經經過的時間
    private Vector3 originalScale; // 血條的原始比例
    private float targetHealth = 1f; // 目標血量（0到1之間）
    public Color startColor = Color.green; // 血條滿時的顏色
    public Color fullColor = Color.yellow; // 血條滿時的顏色

    void Start()
    {
        // 取得 healthBar 的 Transform 和 Image 組件
        healthBar = transform;
        healthBarImage = healthBar.GetComponent<Image>();

        // 保存原始比例
        originalScale = healthBar.localScale;
        // 初始比例設置為0
        healthBar.localScale = new Vector3(0f, originalScale.y, originalScale.z);

        // 設置初始顏色為綠色
        healthBarImage.color = startColor;

        fillDuration = GetFillDuration();
    }

    void Update()
    {
        // 如果尚未達到目標時間
        if (elapsedTime < fillDuration)
        {
            // 增加經過時間
            elapsedTime += Time.deltaTime;
            // 計算當前血量比例，並設置到血條
            float currentHealth = Mathf.Lerp(0f, targetHealth, elapsedTime / fillDuration);
            float scaleX = currentHealth * originalScale.x; // 根據目前血量比例調整 x 軸的尺寸
            float offsetX = (1 - currentHealth) * originalScale.x / 2; // 計算偏移量，使血條整體往右增長
            healthBar.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            healthBar.localPosition = new Vector3(offsetX, healthBar.localPosition.y, healthBar.localPosition.z); // 只修改 X 軸位置，保持 Y 和 Z 軸不變

            // 檢查是否血條已滿
            if (currentHealth >= 1.0f)
            {
                // 血條填滿時，改變顏色為黃色
                healthBarImage.color = fullColor;
            }
        }
        else
        {
            // 確保血條完全填滿並居中顯示
            healthBar.localScale = new Vector3(targetHealth * originalScale.x, originalScale.y, originalScale.z);
            healthBar.localPosition = new Vector3(0f, healthBar.localPosition.y, healthBar.localPosition.z); // 固定在原始 Y 和 Z 軸位置
            healthBarImage.color = fullColor; // 血條填滿時顏色設置為黃色
        }
    }

    // 取得不同作物收成需要的時間
    private float GetFillDuration()
    {
        // 取得父物件的 transform
        Transform parentTransform = transform.parent;
        parentTransform = parentTransform.parent;
        // 迭代所有子物件並輸出名稱
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            // 獲取時間參數
            return ParameterQuery.QueryParameters(childTransform.gameObject, "time");
        }
        return 5.0f;
    }

    // 設置目標血量和填充時間，並重新開始
    public void SetTargetHealth(float health, float duration)
    {
        targetHealth = Mathf.Clamp(health, 0f, 1f); // 設置目標血量在 0 到 1 之間
        fillDuration = duration; // 設置充滿所需時間
        elapsedTime = 0f; // 重置經過時間
        originalScale = healthBar.localScale; // 重置原始比例
        healthBar.localScale = new Vector3(0f, originalScale.y, originalScale.z); // 重置初始比例
        healthBarImage.color = Color.green; // 重置顏色為綠色
    }
}
