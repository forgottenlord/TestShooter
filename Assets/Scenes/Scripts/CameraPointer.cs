using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MouseLook камерой и поинтер для персонажа.
/// </summary>
public class CameraPointer : MonoBehaviour
{
    public GameObject prefab;
    GameObject cameraGO;
    Transform cameraContainerH;
    Transform cameraContainerV;
    public float Sencitivity = 3;
    public CharacterController character;
    private Camera cam;
    void Start()
    {
        name = "Player";
        Cursor.lockState = CursorLockMode.Locked;
        cameraContainerH = transform.Find("CameraContainerH");
        cameraContainerV = cameraContainerH.Find("CameraContainerV");
        cameraGO = cameraContainerV.Find("CameraGO").gameObject;
        cam = cameraGO.GetComponent<Camera>();
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Targeting(Input.GetAxis("Mouse X") * Sencitivity,
                Input.GetAxis("Mouse Y") * Sencitivity);
        }
        if (character != null)
        {
            transform.position = character.transform.position;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                character.IsAlive = !character.IsAlive;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                character.AttackMode = !character.AttackMode;
            }
            OrderRayCast();
        }
    }
    /// <summary>
    /// Наведение/ориентировка камеры.
    /// </summary>
    /// <param name="HSpeed"></param>
    /// <param name="VSpeed"></param>
    public void Targeting(float HSpeed, float VSpeed)
    {
        cameraContainerH.localEulerAngles += new Vector3(0, HSpeed, 0);
        float vertical = cameraContainerV.localEulerAngles.x - VSpeed;
        cameraContainerV.localEulerAngles = new Vector3(vertical, 0, 0);
    }
    /// <summary>
    /// Рейкаст на карту местности.
    /// </summary>
    public void OrderRayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 10000f, 1))
            {
                character.SetTarget(hitInfo.point);
            }
        }
    }
}
