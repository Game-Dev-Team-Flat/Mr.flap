using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMobSpawnerController : TriggerReceiver
{
    private RandomMobSpawnerController spawnerController;
    // Start is called before the first frame update
    public override void OnReceivedTrigger()
    {
        Invoke("SpawnMob", delay);
    }

    [Header("-MobSpawner")]
    public float delay = 0f;
    public int Mobs = 1;
    public GameObject mobPrefab;

    private bool canSpawn = true;

    public void MakeToCanSpawan()
    {
        canSpawn = true;
    }
    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = spawnerController.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = spawnerController.transform.localScale.x;
        float range_Y = spawnerController.transform.localScale.y;
        float range_Z = spawnerController.transform.localScale.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);

        range_Y = Random.Range((range_Y / 2) * -1, range_Y / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, range_Y, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
    private void SpawnMob()
    {
        if (canSpawn)
        {
            for (int i = 0; i < Mobs; i++)
            {
                Instantiate(mobPrefab, Return_RandomPosition(), Quaternion.identity);
            }
            canSpawn = false;
        }
    }

    void Start()
    {
        spawnerController = GetComponent<RandomMobSpawnerController>();
        spawnerController.gameObject.GetComponent<MeshRenderer>().enabled = false;

    }

}
