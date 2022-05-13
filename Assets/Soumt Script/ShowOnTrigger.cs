using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnTrigger : Trigger_Receiver
{
    private ShowOnTrigger showOnTrigger;
    public override void OnReceivedTrigger()
    {
        showOnTrigger.gameObject.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        showOnTrigger = GetComponent<ShowOnTrigger>();
        showOnTrigger.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
