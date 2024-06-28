/*using System.Collections.Generic;
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
                            centerPosition.y += 0.5f;
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
                        centerPosition.y += 0.5f;
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
            RaycastHit[] hits = Physics.RaycastAll(ray); // ����Ҧ����I�����G
            RaycastHit hit = new RaycastHit(); // ��l�� hit
            GameObject hitObject = null;

            // �M���Ҧ��I�����G�A���Ĥ@�Ө㦳 tag �� "land" ������
            foreach (RaycastHit h in hits)
            {
                if (h.collider.CompareTag("land"))
                {
                    hit = h;
                    hitObject = hit.collider.gameObject;
                    break;
                }
            }

            // �p�G�����Ҭ� "land" ������
            if (hitObject != null)
            {
                if (hitObject.transform.childCount > 0)
                {
                    Debug.Log(hit.collider.name);
                    Debug.Log("������w���l����A�R���ͦ�������");
                    Destroy(spawnedObject); // �R���ͦ�������
                }
                else
                {
                    centerPosition = hit.collider.bounds.center;
                    centerPosition.y += 0.5f;
                    spawnedObject.transform.position = centerPosition;
                    spawnedObject.transform.SetParent(hitObject.transform);
                    spawnedObject.AddComponent<CapsuleCollider>();
                }
                isSet = true;
                state = "init";
            }
        }



        // ���첾�ͦ���������H�ƹ�����
        if (state == "drag" && !isSet)
        {
            Debug.Log("����첾��");
            // Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
            //spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // �I���쪺�O "land" ���Ҫ�����
                if (hit.collider.CompareTag("land"))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    centerPosition = hit.collider.bounds.center;
                    centerPosition.y += 0.5f;
                    spawnedObject.transform.position = centerPosition;
                }
            }
            Debug.Log("�����m�G" + spawnedObject.transform.position);
        }
    }
}
*/
using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Produce : MonoBehaviour
{
    public NetworkUser NetworkUser;

    public NetworkPrefabRef prefabRef;

    private string state; // ���A�аO�A�Ω�Ϥ����P�ާ@�]"init"�B"click"�B"drag"�^
    
    public List<NetworkPrefabRef> objectPrefabs; // �n�ͦ�������C��A�q�L Inspector �]�w
    private Ray ray; // �g�u�A�Ω󮷮������I����m
    private RaycastHit hit; // �I����T�A�Ω󮷮��g�u�I���쪺����
    public static Vector3 centerPosition; // ���ߦ�m�A�Ω�ͦ����󪺦�m
    private bool isSet; // �첾����O�_�w�T�w��m
    public static List<NetworkPrefabRef> objectPrefabsStatic; // �R�A�ܼơA�s�x�n�ͦ�������C��
    public static GameObject parentObject; // �R�A�ܼơA�s�x�I��������@���s���󪺤�����
    private GameObject spawnedObject; // ���ŧO�ܼơA�Ω�O�s�ͦ�������

    private NetworkRunner _networkRunner;


    void Start()
    {
        InitializeState();
        Debug.Log("Spawn Local Item");

    }

    void SpawnNetworkObject(NetworkPrefabRef prefabRef, Vector3 position, Quaternion quaternion)
    {
        _networkRunner.Spawn(prefabRef, position, quaternion);
    }



    private void Awake()
    {
        //NetworkUser = GetComponent<NetworkUser>();
        _networkRunner = FindObjectOfType<NetworkRunner>();
    }

    void Update()
    {

        //if (_networkRunner != null && _networkRunner.IsRunning)
        //{
        //    // �ͦ�����
        //    _networkRunner.Spawn(prefabRef, position: Vector3.zero, rotation: Quaternion.identity);
        //}

        // ���Uesc���A�^�k��l
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ResetToInitState();
        }

        //��l���A�]�w�A�קK�������D����
        if (state == "init")
        {
            isSet = true;
        }

        // �ˬd���Х����I���ƥ�
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        // �ˬd���Х�������ƥ�A�Ω�T�w�첾�ͦ�������
        //if (Input.GetMouseButtonUp(0) && !isSet)
        //{
        //    HandleMouseRelease();
        //}

        // ���첾�ͦ���������H�ƹ�����
        //if (state == "drag" && !isSet)
        //{
        //    DragSpawnedObject();
        //}
    }

    public bool GetClickObject(out GameObject gameObject)
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider != null)
        {
            if (hit.collider.CompareTag("land"))
            {
                gameObject = hit.collider.gameObject;
                return true;
            }
        }
        gameObject = null;
        return false;
    }

    private void InitializeState()
    {
        state = "init";
        objectPrefabsStatic = objectPrefabs;
    }

    private void ResetToInitState()
    {
        state = "init";
        Select.scriptUse = true;
        Debug.Log("���A���m�� init");
    }

    private void HandleMouseClick()
    {
        if (state == "init")
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                TryInstantiateObjectFromUI();
            }
            else
            {
                SelectLandBlock();
            }
        }
        else if (state == "click")
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                TryInstantiateObjectFromUI();
            }
            else
            {
                SelectLandBlock();
            }
        }
    }

    private void TryInstantiateObjectFromUI()
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
            string objTag = result.gameObject.tag;
            Debug.Log("�ثeTAG" + objTag + "�ثe���A" + state);
            if (int.TryParse(objTag, out int index) && index < objectPrefabsStatic.Count)
            {
                if (state == "init")
                {
                    //InstantiateObjectAtMousePosition(index);
                    state = "drag";
                    isSet = false;
                }
                else if (state == "click")
                {
                    InstantiateObjectAtCenterPosition(index);
                    state = "init";
                    Select.scriptUse = true;
                }
                return;
            }
        }
    }

    private void SelectLandBlock()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit) && hit.collider != null)
        {
            if (hit.collider.CompareTag("land"))
            {
                centerPosition = hit.collider.bounds.center;
                centerPosition.y += 0.5f;
                parentObject = hit.collider.gameObject;
                state = "click";
                isSet = true;
            }
        }
    }

    //private void InstantiateObjectAtMousePosition(int index)
    //{


    //    spawnedObject = Instantiate(objectPrefabsStatic[index], Vector3.zero, Quaternion.identity);
    //    spawnedObject.tag = "plant";
    //    spawnedObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5));
    //    Debug.Log("�ͦ�����G" + index);
    //}

    private void InstantiateObjectAtCenterPosition(int index)
    {
        if (parentObject != null && parentObject.transform.childCount > 0)
        {
            Debug.Log("������w���l����A���ͦ��s����");
            return;
        }
        Debug.Log($"�i�J����ͦ�");
        SpawnNetworkObject(objectPrefabs[index], centerPosition, Quaternion.identity);
        //spawnedObject.AddComponent<CapsuleCollider>();
        //if (parentObject != null)
        //{
        //    spawnedObject.transform.SetParent(parentObject.transform);
        //}
        //spawnedObject.tag = "plant";
        Debug.Log("�ͦ��F�s����A���Ҭ��G" + index);
    }

    private void HandleMouseRelease()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray); // ����Ҧ����I�����G

        foreach (RaycastHit h in hits)
        {
            if (h.collider.CompareTag("land"))
            {
                if (h.collider.transform.childCount > 0)
                {
                    Destroy(spawnedObject);
                }
                else
                {
                    centerPosition = h.collider.bounds.center;
                    centerPosition.y += 0.5f;
                    spawnedObject.transform.position = centerPosition;
                    spawnedObject.transform.SetParent(h.collider.transform);
                    spawnedObject.AddComponent<CapsuleCollider>();
                }
                isSet = true;
                state = "init";
                return;
            }
        }
    }

    private void DragSpawnedObject()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("land"))
            {
                centerPosition = hit.collider.bounds.center;
                centerPosition.y += 0.5f;
                spawnedObject.transform.position = centerPosition;
            }
        }
        Debug.Log("�����m�G" + spawnedObject.transform.position);
    }
}
