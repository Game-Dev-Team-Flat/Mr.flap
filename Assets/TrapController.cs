using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TrapController : MonoBehaviour
{
    private TrapController trapController;

    [Header("-Movement")]
    public float Speed;
    public Vector3 Move_To;

    [Header("-After Activated")]
    public float Back_time;
    public int Damage;
    public PlayerController DamageReceiver;


    private bool Is_Activated;
    private Vector3 Back_To;
    // Start is called before the first frame update
    
    public void Activate_Trap()
    {
        Is_Activated = true;
        Invoke("Move_Back", Back_time);
    }
    public void Move_Back()
    {
        Is_Activated = false;
    }
    void Start()
    {
        trapController = GetComponent<TrapController>();
        Back_To = trapController.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Is_Activated)
        {
            trapController.transform.position = Vector3.MoveTowards(trapController.transform.position, Move_To, Speed);
        }
        else
        {
            trapController.transform.position = Vector3.MoveTowards(trapController.transform.position, Back_To, Speed);
        }
    }
}
