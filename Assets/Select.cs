using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    public Material newMaterial;               // �I����n���Ϊ��s����
    public Material originalMaterial;          // ��l����A�Ω󭫸m��L����
    private Ray ray;                           // �g�u�A�Ω󮷮������I����m
    private RaycastHit hit;                    // �I����T�A�Ω󮷮��g�u�I���쪺����
    //public static Vector3 centerPosition;      // ���ߦ�m�A�Ω�ͦ����󪺦�m
    //public static List<GameObject> objectPrefabsStatic; // �R�A�ܼơA�s�x�n�ͦ�������C��
   // public static GameObject parentObject;     // �R�A�ܼơA�s�x�I��������@���s���󪺤�����
    public static bool scriptUse; //�o��script�O�_�ҥ�
    void Start()
    {
        scriptUse = true;
        // �T�O originalMaterial ���ȡA�p�G�S���A���զ۰ʳ]�m����e���󪺧���
        if (originalMaterial == null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                originalMaterial = renderer.material;
            }
        }

        // ��l���R�A�ܼ� objectPrefabsStatic
        //objectPrefabsStatic = objectPrefabs;
    }

    void Update()
    {
        // �ˬd���Х����I��
        if (scriptUse|| Input.GetMouseButtonDown(0))
        {
            // �q�D�ṳ������m�Ыؤ@���g�u���V�����I����m
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // �ϥήg�u�˴��I��
            if (Physics.Raycast(ray, out hit))
            {
                // �I���쪺�O "land" ���Ҫ�����
                if (hit.collider.CompareTag("land"))
                {
                    // ����Ҧ� "land" ���Ҫ�����ë�_����l����
                    Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                    foreach (Renderer otherRenderer in allRenderers)
                    {
                        if (otherRenderer.CompareTag("land"))
                        {
                            otherRenderer.material = originalMaterial;
                        }
                    }

                    // �N�I��������]�m���s����
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = newMaterial;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        // �]�m���ߦ�m���I�����󪺤��ߦ�m�A�äW���@�I
                        //centerPosition = hit.collider.bounds.center;
                        //centerPosition.y += 0.6f;
                        //Debug.Log(centerPosition);
                        // �]�m����������������
                        //parentObject = hit.collider.gameObject;
                        scriptUse = false;
                    }
                    
                }
                else
                {
                    // �I���쪺���O "land" ���Ҫ�����A��_�Ҧ� "land" ���󬰭�l����
                    Renderer[] allRenderers = FindObjectsOfType<Renderer>();
                    foreach (Renderer otherRenderer in allRenderers)
                    {
                        if (otherRenderer.CompareTag("land"))
                        {
                            otherRenderer.material = originalMaterial;
                        }
                    }
                    scriptUse = true;
                }
            }
        }
    }
}
