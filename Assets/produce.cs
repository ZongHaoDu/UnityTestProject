using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Produce : MonoBehaviour
{
    private string state; // ���A�аO�A�Ω�Ϥ����P�ާ@�]"init"�B"click"�B"drag"�^
    public List<GameObject> objectPrefabs; // �n�ͦ�������C��A�q�L Inspector �]�w
    private Ray ray; // �g�u�A�Ω󮷮������I����m
    private RaycastHit hit; // �I����T�A�Ω󮷮��g�u�I���쪺����
    public static Vector3 centerPosition; // ���ߦ�m�A�Ω�ͦ����󪺦�m
    private bool isSet; // �첾����O�_�w�T�w��m
    public static List<GameObject> objectPrefabsStatic; // �R�A�ܼơA�s�x�n�ͦ�������C��
    public static GameObject parentObject; // �R�A�ܼơA�s�x�I��������@���s���󪺤�����
    private GameObject spawnedObject; // ���ŧO�ܼơA�Ω�O�s�ͦ�������

    void Start()
    {
        // ��l���A�]�m
        state = "init";
        // ��l���R�A�ܼ� objectPrefabsStatic�A�T�O�b�Ҧ���Ҥ��O���@�P
        objectPrefabsStatic = objectPrefabs;
    }

    void Update()
    {
        // ���Uesc���A�^�k��l
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            state = "init";
            Select.scriptUse = true;
            Debug.Log("���A���m�� init");
        }
        //��l���A�]�w�A�קK�������D����
        if(state == "init")
        {
            isSet = true;
        }
        // �ˬd���Х����I���ƥ�
        if (Input.GetMouseButtonDown(0))
        {
            if (state == "init")
            {
                // �ˬd���ЬO�_�I���b UI �����W�]�첾�ͦ��Ρ^
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };

                    // �����Ҧ��I�����G
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);

                    foreach (RaycastResult result in results)
                    {
                        // ����Q�I���Ϥ������ҡA�åͦ��������w�s����
                        string objTag = result.gameObject.tag;
                        if (int.TryParse(objTag, out int index) && index < objectPrefabsStatic.Count)
                        {
                            // �ͦ��s����A�ͦ���m�������I����m
                            spawnedObject = Instantiate(objectPrefabsStatic[index], Vector3.zero, Quaternion.identity);
                            spawnedObject.tag = "plant";
                            spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
                            state = "drag";
                            isSet = false;
                            Debug.Log("�ͦ�����G" + objTag);
                            return; // �I���F UI ��������X�`��
                        }
                    }
                }
                else
                {
                    // �������]�Ω��I���ͦ��^
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit) && hit.collider != null)
                    {
                        Debug.Log("�I���쪫��G" + hit.collider.gameObject.name);
                        if (hit.collider.CompareTag("land"))
                        {
                            Debug.Log("�������");
                            centerPosition = hit.collider.bounds.center;
                            centerPosition.y += 0.6f;
                            parentObject = hit.collider.gameObject;
                            state = "click";
                            isSet = true;
                        }
                    }

                }
            }
            //�p�G�w�g��������
            else if (state == "click")
            {
                //�p�G�I����L���
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit) && hit.collider != null)
                {
                    Debug.Log("�I���쪫��G" + hit.collider.gameObject.name);
                    if (hit.collider.CompareTag("land"))
                    {
                        Debug.Log("�������");
                        centerPosition = hit.collider.bounds.center;
                        centerPosition.y += 0.6f;
                        parentObject = hit.collider.gameObject;
                        state = "click";
                        isSet = true;
                    }
                }
                //�p�G�I����Ϥ�
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
                    {
                        position = Input.mousePosition
                    };

                    // �����Ҧ��I�����G
                    List<RaycastResult> results = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(pointerEventData, results);

                    foreach (RaycastResult result in results)
                    {
                        // ����Q�I���Ϥ������ҡA�åͦ��������w�s����
                        string objTag = result.gameObject.tag;
                        if (int.TryParse(objTag, out int index) && index < objectPrefabsStatic.Count)
                        {
                            Debug.Log("�Ϥ� " + index);
                            // �ˬd������O�_�w���l����
                            if (parentObject != null && parentObject.transform.childCount > 0)
                            {
                                Debug.Log("������w���l����A���ͦ��s����");
                                state = "init";
                                return; // ������w���l����A���ͦ��s����
                            }

                            // �ͦ��s����A�ͦ���m�����e�O�������ߦ�m
                            spawnedObject = Instantiate(objectPrefabsStatic[index], centerPosition, Quaternion.identity);
                            spawnedObject.AddComponent<CapsuleCollider>();
                            if (parentObject != null)
                            {
                                spawnedObject.transform.SetParent(parentObject.transform);
                            }
                            spawnedObject.tag = "plant";
                            Renderer objectRenderer = spawnedObject.GetComponent<Renderer>();
                            if (objectRenderer != null)
                            {
                                Vector3 minPoint = objectRenderer.bounds.min;
                                float minY = minPoint.y;
                                Vector3 newPosition = new Vector3(
                                    centerPosition.x,
                                    centerPosition.y + centerPosition.y - minY - 0.1f,
                                    centerPosition.z
                                );
                                spawnedObject.transform.position = newPosition;
                            }
                            Debug.Log("�ͦ��F�s����A���Ҭ��G" + objTag);
                            // ���m���A�� "init"
                            state = "init";
                            // �ҥ� Select script 
                            Select.scriptUse = true;
                            break;
                        }
                    }
                }
            }
        }

        // �ˬd���Х�������ƥ�A�Ω�T�w�첾�ͦ�������
        if (Input.GetMouseButtonUp(0) && !isSet)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.transform.childCount > 0 || !hit.collider.CompareTag("land"))
                {
                    Debug.Log("������w���l������I�������O 'land'�A�R���ͦ�������");
                    Destroy(spawnedObject); // �R���ͦ�������
                    isSet = true;
                    state = "init";
                }
                else
                {
                    centerPosition = hit.collider.bounds.center;
                    centerPosition.y += 0.6f;
                    spawnedObject.transform.position = centerPosition;
                    spawnedObject.transform.SetParent(hitObject.transform);
                    spawnedObject.AddComponent<CapsuleCollider>();
                    isSet = true;
                    state = "init";
                }
            }
        }

        // ���첾�ͦ���������H�ƹ�����
        if (state == "drag" && !isSet)
        {
            Debug.Log("����첾��");
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
            spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            Debug.Log("�����m�G" + spawnedObject.transform.position);
        }
    }
}
