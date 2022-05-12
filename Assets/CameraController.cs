using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Person { FirstPerson, ThirdPerson }

public static class PersonSetting { public static Person person = Person.FirstPerson; }

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject eyeOfObejct;
    [SerializeField]
    private Vector3 firstPersonVector;
    [SerializeField]
    private Vector3 thirdPersonVector;
    private float xRotate;
    private float yRotate;
    public float rotSpeed = 200;

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

        float xMouse = Input.GetAxis("Mouse X");
        float yMouse = Input.GetAxis("Mouse Y");

        xRotate += rotSpeed * yMouse * Time.deltaTime;
        yRotate += rotSpeed * xMouse * Time.deltaTime;

        xRotate = Mathf.Clamp(xRotate, -80, 80);

        eyeOfObejct.transform.eulerAngles = new Vector3(-xRotate, eyeOfObejct.transform.eulerAngles.y, 0);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }

    private void ThirdPerson()
    {
        eyeOfObejct.GetComponentInChildren<Camera>().transform.localPosition = thirdPersonVector;

        float xMouse = Input.GetAxis("Mouse X");
        float yMouse = Input.GetAxis("Mouse Y");

        xRotate += rotSpeed * yMouse * Time.deltaTime;
        yRotate += rotSpeed * xMouse * Time.deltaTime;

        xRotate = Mathf.Clamp(xRotate, -80, 80);

        eyeOfObejct.transform.eulerAngles = new Vector3(-xRotate, eyeOfObejct.transform.eulerAngles.y, 0);
        transform.eulerAngles = new Vector3(0, yRotate, 0);
    }
}