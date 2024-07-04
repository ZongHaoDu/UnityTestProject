using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalBloodBar : MonoBehaviour
{
    private RectTransform healthBarRect; // ����� RectTransform �ե�
    private float targetHealth = 1f; // �ؼЦ�q�]0��1�����^�A��l�]�m������
    private float currentHealth; // ��e��q
    public float decreasePercentage = 0.1f; // �C���I����������q�ʤ���
    public Color startColor = Color.green; // ������ɪ��C��
    public float moveSpeed = 0.1f;
    public int bloodValue; // ��q�Ѽ�
    public float cooldownTime = 0.001f; // �N�o�ɶ�
    private float nextAllowedCollisionTime = 0f; // �U�@�����\�I�����ɶ�

    void Start()
    {
        // �b���󪺤l���󤤬d�� RectTransform �ե�]���]����Ϥ��O AnimalBloodBar ���󪺤l����^
        healthBarRect = GetComponentInChildren<RectTransform>();

        // �]�m��l��q������
        currentHealth = targetHealth;

        // ��s������
        UpdateHealthBar();

        // �����q�Ѽ�
        bloodValue = ParameterQuery.QueryParameters(gameObject, "blood");
    }

    // ��I����F��ɦ�����q
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObject = collision.gameObject;

        // �p�G�I�����O land �Ϊ��٦b�N�o�ɶ����A�h��^
        if (otherObject.tag == "land" || Time.time < nextAllowedCollisionTime)
        {
            return;
        }

        // ��������Ѽ�
        int attackValue = ParameterQuery.QueryParameters(otherObject, "attack");

        if (bloodValue != 0) // ����H�s
        {
            decreasePercentage = (float)attackValue / bloodValue; // ��s decreasePercentage
            DecreaseHealth();

            // �ק� Transform.position �����T��k
            Vector3 newPosition = transform.position;
            newPosition.x -= 1;
            transform.position = newPosition;

            // ��s�N�o�ɶ�
            nextAllowedCollisionTime = Time.time + cooldownTime;
        }
    }

    // ������q
    private void DecreaseHealth()
    {
        float amountToDecrease = targetHealth * decreasePercentage; // �p��n��������q

        currentHealth -= amountToDecrease; // ������q

        // �����q�b 0 �� 1 ����
        currentHealth = Mathf.Clamp(currentHealth, 0f, 1f);

        // ��s������
        UpdateHealthBar();
    }

    // ��s������
    private void UpdateHealthBar()
    {
        // �]�m����� x �b scale ����e��q���ʤ���
        healthBarRect.localScale = new Vector3(currentHealth, healthBarRect.localScale.y, healthBarRect.localScale.z);

        Debug.Log("Current Health: " + currentHealth);
    }

    // �b�C�ӴV��s�����m
    private void Update()
    {
        Move(); // �եβ��ʤ�k
        if (currentHealth < 0.0001)
        {
            Destroy(gameObject);
        }
    }

    //�������ʥ�(����n����)
    private void Move()
    {
        Vector3 currentPosition = transform.position;

        if (Input.GetKey(KeyCode.D))
            currentPosition.x += moveSpeed; // ���k
        if (Input.GetKey(KeyCode.A))
            currentPosition.x -= moveSpeed; // ����
        if (Input.GetKey(KeyCode.W))
            currentPosition.z += moveSpeed; // ���e
        if (Input.GetKey(KeyCode.S))
            currentPosition.z -= moveSpeed; // ����

        transform.position = currentPosition;
    }
}
