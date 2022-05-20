using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Person { FirstPerson, ThirdPerson }

public static class PersonSetting { public static Person person = Person.FirstPerson; }

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject eyeOfObejct;
    private float xRotate;
    private float yRotate;
    public float rotSpeed = 200;

    [Header("-First Person")]
    [SerializeField]
    private Vector3 firstPersonVector;

    [Header("-Third Person")]
    [SerializeField]
    private Vector3 thirdPersonVector;
    [SerializeField]
    private float cameraBackDistance;

    private void Awake()
    {
        xRotate = transform.eulerAngles.x;
        yRotate = transform.eulerAngles.y;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.N)) PersonSetting.person = Person.FirstPerson;
        if (Input.GetKeyDown(KeyCode.M)) PersonSetting.person = Person.ThirdPerson;

        if (PersonSetting.person == Person.FirstPerson)
        {
            FirstPerson();
        }
        else if(PersonSetting.person == Person.ThirdPerson)
        {
            ThirdPerson();
        }
    }

    private void FirstPerson()
    {
        eyeOfObejct.GetComponentInChildren<Camera>().transform.localPosition = firstPersonVector;

        AimMovement();
    }

    private void ThirdPerson()
    {
        if (Physics.Raycast(transform.position + Vector3.up * thirdPersonVector.y, -eyeOfObejct.transform.forward, out RaycastHit hitCollider, Vector3.Distance(thirdPersonVector, Vector3.zero), LayerMask.GetMask("Floor")))
        {
            eyeOfObejct.GetComponentInChildren<Camera>().transform.position = hitCollider.point;
        }
        else
        {
            eyeOfObejct.GetComponentInChildren<Camera>().transform.position = Vector3.zero;
            eyeOfObejct.GetComponentInChildren<Camera>().transform.localPosition = thirdPersonVector;
        }
        eyeOfObejct.GetComponentInChildren<Camera>().transform.Translate(-thirdPersonVector.normalized * cameraBackDistance);

    AimMovement();
    }

    private void AimMovement()
    {
        float xMouse = Input.GetAxis("Mouse X");
        float yMouse = Input.GetAxis("Mouse Y");

        xRotate += rotSpeed * yMouse * Time.deltaTime;
        yRotate += rotSpeed * xMouse * Time.deltaTime;

        xRotate = Mathf.Clamp(xRotate, -80, 80);

        eyeOfObejct.transform.eulerAngles = new Vector3(-xRotate, eyeOfObejct.transform.eulerAngles.y, 0);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }
}