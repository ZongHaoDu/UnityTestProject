using UnityEngine;

public class PlantBloodBar : MonoBehaviour
{
    private RectTransform healthBar; // 血條物件的 RectTransform（綁定腳本的物件）
    public float fillDuration = 5.0f; // 血條充滿所需的時間（秒）
    private float elapsedTime = 0f; // 已經經過的時間
    private float originalWidth; // 血條的原始寬度
    private float targetHealth = 1f; // 目標血量（0到1之間）

    void Start()
    {
        // 將 healthBar 設置為當前物件的 RectTransform
        healthBar = GetComponent<RectTransform>();

        // 保存原始寬度
        originalWidth = healthBar.sizeDelta.x;
        // 初始寬度設置為0（即最左邊）
        healthBar.sizeDelta = new Vector2(0f, healthBar.sizeDelta.y);
    }

    void Update()
    {
        // 如果尚未達到目標時間
        if (elapsedTime < fillDuration)
        {
            // 增加經過時間
            elapsedTime += Time.deltaTime;
            // 計算當前血量比例，並設置到血條（從最左到最右）
            float currentHealth = Mathf.Lerp(0f, targetHealth, elapsedTime / fillDuration);
            healthBar.sizeDelta = new Vector2(currentHealth * originalWidth, healthBar.sizeDelta.y);
        }
        else
        {
            // 確保血條完全填滿（最右邊）
            healthBar.sizeDelta = new Vector2(targetHealth * originalWidth, healthBar.sizeDelta.y);
        }
    }

    // 設置目標血量和填充時間，並重新開始
    public void SetTargetHealth(float health, float duration)
    {
        targetHealth = Mathf.Clamp01(health); // 設置目標血量在 0 到 1 之間
        fillDuration = duration; // 設置充滿所需時間
        elapsedTime = 0f; // 重置經過時間
        // 重新計算原始寬度（因為有可能在過程中血條已經改變）
        originalWidth = healthBar.sizeDelta.x;
        healthBar.sizeDelta = new Vector2(0f, healthBar.sizeDelta.y); // 重置初始寬度（最左邊）
    }
}
