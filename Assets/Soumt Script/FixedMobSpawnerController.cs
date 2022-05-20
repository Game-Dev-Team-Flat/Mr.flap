using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMobSpawnerController : TriggerReceiver
{
    private FixedMobSpawnerController spawnerController;
    // Start is called before the first frame update
    public override void OnReceivedTrigger()
    {
        Instantiate(mobPrefab, spawnerController.transform.position, Quaternion.identity);
    }

    [Header("-MobSpawner")]
    public float delay = 0f;
    public GameObject mobPrefab;

    private bool canSpawn = true;

    void Start()
    {
        spawnerController = GetComponent<FixedMobSpawnerController>();
        spawnerController.gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
