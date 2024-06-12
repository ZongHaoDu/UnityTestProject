using UnityEngine;

public class Detect : MonoBehaviour
{
    private Vector3 center = new Vector3(0, 0.6f, 0); // ���ߦ�m
    private Vector3 halfExtents = new Vector3(0.01f, 0.01f, 0.01f); // �Y�p��������b���
    private Quaternion orientation = Quaternion.identity; // �����骺����
    private LayerMask layerMask = ~0; // �w�]�˴��Ҧ��ϼh

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // ���U�ť���Ӷi���˴�
        {
            Debug.Log("�}�l���l�˴��A����: " + center + "�A�b���: " + halfExtents);

            // �ϥ� Physics.OverlapBox �˴��ϰ줺���I����
            Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask);

            if (hitColliders.Length > 0)
            {
                Debug.Log("�b���w�ϰ줺�˴��쪫��:");
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider != null && hitCollider.gameObject != null)
                    {
                        string tag = hitCollider.gameObject.tag;
                        Debug.Log("����W��: " + hitCollider.name + "�Atag: " + tag);
                    }
                }
            }
            else
            {
                Debug.Log("�b���w�ϰ줺�S���˴��쪫��");
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, halfExtents * 2);
    }
}
