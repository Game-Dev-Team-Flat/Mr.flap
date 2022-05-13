using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInTrigger : MonoBehaviour
{
    [Header("-Trigger")]
    public Trigger_Receiver Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something Triggered");
        if (other.gameObject.layer == 7)
        {
            Target.OnReceivedTrigger();
            Debug.Log("Player Triggered");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
