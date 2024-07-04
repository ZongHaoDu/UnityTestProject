using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Select_2 : MonoBehaviour
{
    public Material newMaterial;               // �I����n���Ϊ��s����
    public Material originalMaterial;          // ��l����A�Ω󭫸m��L����
    private Ray ray;                           // �g�u�A�Ω󮷮������I����m
    private RaycastHit hit;                    // �I����T�A�Ω󮷮��g�u�I���쪺����
    public static bool scriptUse;              // �o�� script �O�_�ҥ�
    private GameObject lastSelected;           // �O�s�W���襤������
    public string tag;

    void Start()
    {
        lastSelected = null;
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
    }

    void Update()
    {
        Debug.Log("scriptuse"+scriptUse);
        // �ˬd���Х����I��
        if (scriptUse|| Input.GetMouseButtonDown(0))
        {
            // �ˬd���ЬO�_�b UI �����W
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("���ЦbUI�W�A�g�u�˴����L");
                return; // �p�G���Цb UI �W�A���L�g�u�˴�
            }
            Debug.Log("aaaaa");
            // �q�D�ṳ������m�Ыؤ@���g�u���V�����I����m
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // �ϥήg�u�˴��I��
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("��쪺"+hit.collider.tag);
                // �I���쪺�O "land" ���Ҫ�����
                if (hit.collider.CompareTag(tag))
                {
                    // �N�W�ӿ�����g�a�^�_�����
                    if (lastSelected != null)
                    {
                        Renderer lastRenderer = lastSelected.GetComponent<Renderer>();
                        if (lastRenderer != null)
                        {
                            lastRenderer.material = originalMaterial;
                        }
                    }

                    // �N�I��������]�m���s����
                    Renderer renderer = hit.collider.gameObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = newMaterial;
                        lastSelected = hit.collider.gameObject;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        scriptUse = false;
                    }
                    
                }
                else
                {
                    // �I���쪺���O "land" ���Ҫ�����A��_�W���襤�����󬰭�l����
                    if (lastSelected != null)
                    {
                        Renderer lastRenderer = lastSelected.GetComponent<Renderer>();
                        if (lastRenderer != null)
                        {
                            lastRenderer.material = originalMaterial;
                        }
                    }
                    lastSelected = null;
                    scriptUse = true;
                }
            }
        }
    }
}
