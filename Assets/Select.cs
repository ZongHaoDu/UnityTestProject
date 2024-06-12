using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    public List<GameObject> objectPrefabs;  // �n�ͦ�������C��
    public Material newMaterial;            // �s����
    public Material originalMaterial;       // ��l����
    private Ray ray;                        // �g�u
    private RaycastHit hit;                 // �I����T
    public static Vector3 centerPosition;
    public static List<GameObject> objectPrefabsStatic;  // �R�A�ܼ�

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
                if (hit.collider.gameObject == this.gameObject&&hit.collider.CompareTag("land"))
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
                            if (otherRenderer != renderer && otherRenderer.tag=="land")
                            {
                                otherRenderer.material = originalMaterial;
                            }
                        }

                        // �󴫳Q���������骺����
                        renderer.material = newMaterial;
                    }
                }
            }
        }
    }
}
