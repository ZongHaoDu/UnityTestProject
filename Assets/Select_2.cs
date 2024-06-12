using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select_2 : MonoBehaviour
{
    public List<GameObject> objectPrefabs;  // �n�ͦ�������C��
    public Material newMaterial;            // �s����
    public Material originalMaterial;       // ��l����
    private Ray ray;                        // �g�u
    private RaycastHit hit;                 // �I����T
    public static Vector3 centerPosition;
    public static List<GameObject> objectPrefabsStatic;  // �R�A�ܼ�
    private Vector3 checkPosition;          // �ˬd��m
    private Vector3 checkHalfExtents;       // �ˬd������b���

    void Start()
    {
        if (originalMaterial == null)
        {
            // �T�O��l����w�g�]�m�A�p�G�S���]�m�h�۰������e���骺����
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
            }
        }

        // ��l���R�A�ܼ�
        objectPrefabsStatic = objectPrefabs;

        // ��l���ˬd��m�M�b���
        checkHalfExtents = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // �ϥΥD�ṳ���Ыؤ@�ڮg�u�A�g�u����V�O�ڭ̹����I������m
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // �ϥΪ��z���ˬd�g�u���I���A�Y�������h�^�� true
            if (Physics.Raycast(ray, out hit))
            {
                // �T�{�g�u�O�_�I�����e�����}��������
                if (hit.collider.gameObject == this.gameObject)
                {
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        centerPosition = renderer.bounds.center; // ���o�������m

                        centerPosition.y += 0.6f;

                        // �b�󴫧��褧�e�A�N�Ҧ���L����������_����l����
                        Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                        foreach (Renderer otherRenderer in allRenderers)
                        {
                            if (otherRenderer != renderer)
                            {
                                otherRenderer.material = originalMaterial;
                            }
                        }

                        // �󴫳Q���������骺����
                        renderer.material = newMaterial;

                        // �]�m�ˬd��m���������m
                        checkPosition = centerPosition;

                        // �ˬd�O�_�w�g������s�b���ˬd��m
                        if (!CheckObjectExists())
                        {
                            // �ͦ��s����
                            GenerateObjects(centerPosition);
                        }
                        else
                        {
                            Debug.Log("�ˬd��m�w�g������s�b�A�L�k�ͦ��s����");
                        }
                    }
                }
            }
        }
    }

    void GenerateObjects(Vector3 position)
    {
        foreach (GameObject prefab in objectPrefabs)
        {
            GameObject newObject = Instantiate(prefab, position, Quaternion.identity);

            // �K�[���n�I����
            CapsuleCollider collider = newObject.AddComponent<CapsuleCollider>();
            // �վ�I����ѼơA�Ҧp���שM�b�|
            collider.height = 2f; // ���n������
            collider.radius = 0.5f; // ���n���b�|
        }
    }

    bool CheckObjectExists()
    {
        // �ϥ� Physics.OverlapBox �˴��ˬd��m����O�_������s�b
        Collider[] hitColliders = Physics.OverlapBox(checkPosition, checkHalfExtents, Quaternion.identity);

        // �Y������s�b�h��^ true
        return hitColliders.Length > 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(checkPosition, checkHalfExtents * 2);
    }
}
