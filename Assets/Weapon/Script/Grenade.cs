using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float moveSpeed;
    public float holdingTime;
    private float shotTime;
    public float explosionRadius;
    public float explosionDamage;

    private void Awake()
    {
        shotTime = Time.time;
    }

    private void Update()
    {
        transform.position = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        if (Time.time - shotTime > holdingTime)
        {
            Explosion();
        }
    }

    private void Explosion()
    {
        Debug.Log("Boom");
        RaycastHit[] objectsHit = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.up, 0f);
        foreach (RaycastHit objectHit in objectsHit)
        {
            // objectHit.TryComponent해서 체력 정보 받아서 깎기
        }
        // 이펙트 발사
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            Explosion();
        }
    }
}
