using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSwitch : MonoBehaviour
{
    [Header("-Trigger")]
    public TrapController Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Target.Activate_Trap();
            Debug.Log("Player Triggered");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
