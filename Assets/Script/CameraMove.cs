using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Ray ray; // �g�u
    private RaycastHit hit; // �ΨӦs�x�g�u�˴��쪺��H�H��
    private Vector3 FirstVector;
    private Vector3 SecondVector;

    public float zoomSpeed = 10f; // �Y��t��
    public float moveSpeed = 0.005f; // x�By���ʳt��
    private string now = "undefine";
    // Update is called once per frame
    void Start()
    {
        now = "undefine";
    }
    void Update()
    {
        if (now != "undefine")
        {
            Vector3 currentPosition = Camera.main.transform.position;
            if (now == "D")
                currentPosition.x += moveSpeed; // ���k
            if (now == "A")
                currentPosition.x -= moveSpeed; // ����
            if (now == "W")
                currentPosition.z += moveSpeed; // ���e
            if (now == "S")
                currentPosition.z -= moveSpeed; // ����
            Camera.main.transform.position = currentPosition; // �N�ק�᪺��m��ȵ��۾�
        }
        if (Input.GetKeyUp(KeyCode.D)|| Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            now = "undefine";
        }
        if (Input.anyKeyDown)
        {
            string inputLetter = Input.inputString;
            now = inputLetter.ToUpper();
            if (!string.IsNullOrEmpty(inputLetter))
            {
                Debug.Log("���U����L�r�����G" + inputLetter);
            }
        }

        /*
        // �˴��ƹ�������U
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // �O���۾���m�켲���I���V�q
                FirstVector = hit.point - Camera.main.transform.position;
                Debug.Log("First Vector: " + FirstVector);
            }
        }

        // �˴��ƹ���������
        if (Input.GetMouseButtonUp(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                // �O���۾���m�켲���I���V�q
                SecondVector = hit.point - Camera.main.transform.position;
                Debug.Log("Second Vector: " + SecondVector);

                // �p���ӦV�q���t
                Vector3 movementVector = SecondVector - FirstVector;
                Debug.Log("Movement Vector: " + movementVector);

                // ���ʬ۾�
                Camera.main.transform.position -= movementVector;
            }
        }
        */


        // �˴��ƹ��u���u��
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            // �p��s���۾���m
            Vector3 direction = Camera.main.transform.forward; // �ھڬ۾�����V����
            Vector3 newPosition = Camera.main.transform.position + direction * scroll * zoomSpeed;

            // ��s�۾���m
            Camera.main.transform.position = newPosition;
        }
    }
}
