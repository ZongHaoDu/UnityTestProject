/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Transform target; // �l�ܪ��ؼЪ���
    public float speed = 1.0f; // �V�q����
    public string requiredTag = "land"; // �a���ݭn������

    private Vector3 directionToTarget; // ��e����� target ���V�q
    private List<Vector3> rotatedDirections; // �s�x���P���פU�������V�q
    private List<Vector3> positions; // �s�x�C�ӱ�����m���V�q

    void Start()
    {
        // ��l��
        rotatedDirections = new List<Vector3>();
        positions = new List<Vector3>();
        directionToTarget = target.position - transform.position; // �p���e����� target ���V�q
        // �p�⤣�P���ת������V�q�M��m�V�q
        CalculateRotatedDirections();
        CalculatePositions();
    }

    void Update()
    {
        // �p�⤣�P���ת������V�q�M��m�V�q
        CalculateRotatedDirections();
        CalculatePositions();
        // �g�u�˴��å��L���G
        RaycastHit hitInfo;
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 startPosition = transform.position + rotatedDirections[i]; // �p��g�u�_�l��m
            Vector3 direction = -Vector3.up; // �g�u��V�V�U

            // �g�u�˴�
            if (Physics.Raycast(startPosition, direction, out hitInfo))
            {
                Debug.Log("�g�쪺���� (��m" + (i + 1) + "): " + hitInfo.collider.gameObject.tag + "�A��m�G" + hitInfo.point);

                // �P�_�g�쪺���󪺼��ҡA�ò��ʪ���
                if (hitInfo.collider.gameObject.CompareTag(requiredTag))
                {
                    Debug.Log("���ʨ��m" + (i + 1) + ": " + positions[i]);
                    transform.position = positions[i];
                    return; // �u���ų̰�����m�˴���Y�i���ʡA�������
                }
            }
            else
            {
                Debug.Log("���g�쪫�� (��m" + (i + 1) + ")");
            }
        }
    }

    // �p�⤣�P���ת������V�q
    void CalculateRotatedDirections()
    {
        rotatedDirections.Clear();

        // �����u�����ǲK�[���P���ת������V�q
        rotatedDirections.Add((Quaternion.Euler(0, 0, 0) * directionToTarget).normalized * speed); // 0��
        rotatedDirections.Add((Quaternion.Euler(0, 30, 0) * directionToTarget).normalized * speed); // 30��
        rotatedDirections.Add((Quaternion.Euler(0, -30, 0) * directionToTarget).normalized * speed); // -30��
        rotatedDirections.Add((Quaternion.Euler(0, 60, 0) * directionToTarget).normalized * speed); // 60��
        rotatedDirections.Add((Quaternion.Euler(0, -60, 0) * directionToTarget).normalized * speed); // -60��
        rotatedDirections.Add((Quaternion.Euler(0, 90, 0) * directionToTarget).normalized * speed); // 90��
        rotatedDirections.Add((Quaternion.Euler(0, -90, 0) * directionToTarget).normalized * speed); // -90��
        rotatedDirections.Add((Quaternion.Euler(0, 120, 0) * directionToTarget).normalized * speed); // 120��
        rotatedDirections.Add((Quaternion.Euler(0, -120, 0) * directionToTarget).normalized * speed); // -120��
        rotatedDirections.Add((Quaternion.Euler(0, 150, 0) * directionToTarget).normalized * speed); // 150��
        rotatedDirections.Add((Quaternion.Euler(0, -150, 0) * directionToTarget).normalized * speed); // -150��
        rotatedDirections.Add((Quaternion.Euler(0, 180, 0) * directionToTarget).normalized * speed); // 180��
        rotatedDirections.Add((Quaternion.Euler(0, -180, 0) * directionToTarget).normalized * speed); // -180��
    }

    // �p�⤣�P���ת���m�V�q
    void CalculatePositions()
    {
        positions.Clear();

        // �p��C�ӱ���᪺��m�V�q
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
    public float speed = 0.05f; // �e�i�t��
    public float maxRotationAngle = 90f; // �̤j���ਤ�׭���
    private Rigidbody rb; // ���骺����
    public float distance = 2.0f; // ray�P�_�Z��
    private string requiredTag = "Road";
    private List<Vector3> rotatedDirections; // �s�x���P���פU�������V�q
    private List<Vector3> positions; // �s�x�C�ӱ�����m���V�q
    public Transform target; // �l�ܪ��ؼЪ���
    private Vector3 directionToTarget; // ��e����� target ���V�q
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // ��� Rigidbody �ե�
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; // ���� X �M Z �b������
        // ��l��
        directionToTarget = target.position - transform.position; // �p���e����� target ���V�q
        rotatedDirections = new List<Vector3>();
        positions = new List<Vector3>();
    }

    void FixedUpdate()
    {
        // �O���b Y �b�W�����ਤ�׭���
        Vector3 eulerAngleVelocity = new Vector3(0, Input.GetAxis("Horizontal") * maxRotationAngle, 0);
        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);

        // ����e�p��s����V
        Vector3 newForward = deltaRotation * transform.forward;
        float angle = Vector3.Angle(newForward, directionToTarget);

        if (angle <= 180f)
        {
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        // �p�⤣�P���ת������V�q�M��m�V�q
        CalculateRotatedDirections();
        CalculatePositions();

        // �˴��C�ӱ���᪺��m�O�_�ŦX����
        RaycastHit hitInfo;
        for (int i = 0; i < positions.Count; i++)
        {
            Vector3 startPosition = transform.position + rotatedDirections[i]; // �p��g�u�_�l��m
            Vector3 direction = -Vector3.up; // �g�u��V�V�U

            // �g�u�˴�
            if (Physics.Raycast(startPosition, direction, out hitInfo))
            {
                // �P�_�g�쪺���󪺼��ҡA�ò��ʪ���
                if (hitInfo.collider.gameObject.CompareTag(requiredTag))
                {
                    Debug.Log(i);
                    if (i == 0)
                    {
                        // �b���骺���e�貾��
                        rb.MovePosition(rb.position + transform.forward * speed);
                    }
                    else if (i == 1)
                    {
                        Quaternion newRotation = Quaternion.Euler(0, -90, 0);
                        Vector3 rotatedForward = newRotation * transform.forward;
                        if (Vector3.Angle(rotatedForward, directionToTarget) <= 170f)
                        {
                            rb.MoveRotation(rb.rotation * newRotation);
                            // �P�ɽվ㪫�骺�e��V�q
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
                            // �P�ɽվ㪫�骺�e��V�q
                            transform.forward = newRotation * transform.forward;
                        }
                    }

                    // ��������
                    rb.angularVelocity = Vector3.zero;

                    return; // �u���ų̰�����m�˴���Y�i���ʡA�������
                }
            }
        }
    }

    // �p�⤣�P���ת������V�q
    void CalculateRotatedDirections()
    {
        rotatedDirections.Clear();

        // ����e��V�q�A�Y���骺���e��
        Vector3 forwardDirection = rb.transform.forward;
        rotatedDirections.Add(forwardDirection * distance); // 0��

        forwardDirection = -rb.transform.right;
        rotatedDirections.Add(forwardDirection * distance); // 90��

        forwardDirection = rb.transform.right;
        rotatedDirections.Add(forwardDirection * distance); // -90��
    }

    // �p�⤣�P���ת���m�V�q
    void CalculatePositions()
    {
        positions.Clear();

        // �p��C�ӱ���᪺��m�V�q
        foreach (Vector3 rotatedDirection in rotatedDirections)
        {
            positions.Add(transform.position + rotatedDirection);
        }
    }
}
