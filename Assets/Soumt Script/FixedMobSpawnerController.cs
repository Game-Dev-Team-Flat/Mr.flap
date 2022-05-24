using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedMobSpawnerController : TriggerReceiver
{
    private FixedMobSpawnerController spawnerController;
    // Start is called before the first frame update
    public override void OnReceivedTrigger()
    {
        Invoke("SpawnMob", delay);
    }

    [Header("-MobSpawner")]
    public float delay = 0f;
    public GameObject mobPrefab;

    private bool canSpawn = true;

    public void MakeToCanSpawan()
    {
        canSpawn = true;
    }
    private void SpawnMob()
    {
        if (canSpawn)
        {
            Instantiate(mobPrefab, spawnerController.transform.position, Quaternion.identity);
            canSpawn = false;
        }
    }

    void Start()
    {
        spawnerController = GetComponent<FixedMobSpawnerController>();
        spawnerController.gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

}
