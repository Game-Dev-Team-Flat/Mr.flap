using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInTrigger : MonoBehaviour
{
    [Header("-Trigger")]
    public TriggerReceiver[] Targets;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            foreach(var Target in Targets)
                Target.OnReceivedTrigger();
            Debug.Log("Player Triggered");
        }
    }
}
