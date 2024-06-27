using UnityEngine;

public class PlantBloodBar : MonoBehaviour
{
    private RectTransform healthBar; // ������� RectTransform�]�j�w�}��������^
    public float fillDuration = 5.0f; // ����R���һݪ��ɶ��]��^
    private float elapsedTime = 0f; // �w�g�g�L���ɶ�
    private float originalWidth; // �������l�e��
    private float targetHealth = 1f; // �ؼЦ�q�]0��1�����^

    void Start()
    {
        // �N healthBar �]�m����e���� RectTransform
        healthBar = GetComponent<RectTransform>();

        // �O�s��l�e��
        originalWidth = healthBar.sizeDelta.x;
        // ��l�e�׳]�m��0�]�Y�̥���^
        healthBar.sizeDelta = new Vector2(0f, healthBar.sizeDelta.y);
    }

    void Update()
    {
        // �p�G�|���F��ؼЮɶ�
        if (elapsedTime < fillDuration)
        {
            // �W�[�g�L�ɶ�
            elapsedTime += Time.deltaTime;
            // �p���e��q��ҡA�ó]�m�����]�q�̥���̥k�^
            float currentHealth = Mathf.Lerp(0f, targetHealth, elapsedTime / fillDuration);
            healthBar.sizeDelta = new Vector2(currentHealth * originalWidth, healthBar.sizeDelta.y);
        }
        else
        {
            // �T�O��������񺡡]�̥k��^
            healthBar.sizeDelta = new Vector2(targetHealth * originalWidth, healthBar.sizeDelta.y);
        }
    }

    // �]�m�ؼЦ�q�M��R�ɶ��A�í��s�}�l
    public void SetTargetHealth(float health, float duration)
    {
        targetHealth = Mathf.Clamp01(health); // �]�m�ؼЦ�q�b 0 �� 1 ����
        fillDuration = duration; // �]�m�R���һݮɶ�
        elapsedTime = 0f; // ���m�g�L�ɶ�
        // ���s�p���l�e�ס]�]�����i��b�L�{������w�g���ܡ^
        originalWidth = healthBar.sizeDelta.x;
        healthBar.sizeDelta = new Vector2(0f, healthBar.sizeDelta.y); // ���m��l�e�ס]�̥���^
    }
}
