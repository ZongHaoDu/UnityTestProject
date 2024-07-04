using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantBloodBar : MonoBehaviour
{
    private Transform healthBar; // �������]�j�w�}��������^
    private Image healthBarImage; // ����� Image �ե�
    private float fillDuration = 5.0f; // ����R���һݪ��ɶ��]��^
    private float elapsedTime = 0f; // �w�g�g�L���ɶ�
    private Vector3 originalScale; // �������l���
    private float targetHealth = 1f; // �ؼЦ�q�]0��1�����^
    public Color startColor = Color.green; // ������ɪ��C��
    public Color fullColor = Color.yellow; // ������ɪ��C��

    void Start()
    {
        // ���o healthBar �� Transform �M Image �ե�
        healthBar = transform;
        healthBarImage = healthBar.GetComponent<Image>();

        // �O�s��l���
        originalScale = healthBar.localScale;
        // ��l��ҳ]�m��0
        healthBar.localScale = new Vector3(0f, originalScale.y, originalScale.z);

        // �]�m��l�C�⬰���
        healthBarImage.color = startColor;

        fillDuration = GetFillDuration();
    }

    void Update()
    {
        // �p�G�|���F��ؼЮɶ�
        if (elapsedTime < fillDuration)
        {
            // �W�[�g�L�ɶ�
            elapsedTime += Time.deltaTime;
            // �p���e��q��ҡA�ó]�m����
            float currentHealth = Mathf.Lerp(0f, targetHealth, elapsedTime / fillDuration);
            float scaleX = currentHealth * originalScale.x; // �ھڥثe��q��ҽվ� x �b���ؤo
            float offsetX = (1 - currentHealth) * originalScale.x / 2; // �p�ⰾ���q�A�Ϧ�����驹�k�W��
            healthBar.localScale = new Vector3(scaleX, originalScale.y, originalScale.z);
            healthBar.localPosition = new Vector3(offsetX, healthBar.localPosition.y, healthBar.localPosition.z); // �u�ק� X �b��m�A�O�� Y �M Z �b����

            // �ˬd�O�_����w��
            if (currentHealth >= 1.0f)
            {
                // ����񺡮ɡA�����C�⬰����
                healthBarImage.color = fullColor;
            }
        }
        else
        {
            // �T�O��������񺡨é~�����
            healthBar.localScale = new Vector3(targetHealth * originalScale.x, originalScale.y, originalScale.z);
            healthBar.localPosition = new Vector3(0f, healthBar.localPosition.y, healthBar.localPosition.z); // �T�w�b��l Y �M Z �b��m
            healthBarImage.color = fullColor; // ����񺡮��C��]�m������
        }
    }

    // ���o���P�@�������ݭn���ɶ�
    private float GetFillDuration()
    {
        // ���o������ transform
        Transform parentTransform = transform.parent;
        parentTransform = parentTransform.parent;
        // ���N�Ҧ��l����ÿ�X�W��
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            Transform childTransform = parentTransform.GetChild(i);
            // ����ɶ��Ѽ�
            return ParameterQuery.QueryParameters(childTransform.gameObject, "time");
        }
        return 5.0f;
    }

    // �]�m�ؼЦ�q�M��R�ɶ��A�í��s�}�l
    public void SetTargetHealth(float health, float duration)
    {
        targetHealth = Mathf.Clamp(health, 0f, 1f); // �]�m�ؼЦ�q�b 0 �� 1 ����
        fillDuration = duration; // �]�m�R���һݮɶ�
        elapsedTime = 0f; // ���m�g�L�ɶ�
        originalScale = healthBar.localScale; // ���m��l���
        healthBar.localScale = new Vector3(0f, originalScale.y, originalScale.z); // ���m��l���
        healthBarImage.color = Color.green; // ���m�C�⬰���
    }
}
