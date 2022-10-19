using UnityEngine;

namespace Item.Weapon
{
    public class Grenade : Boom
    {
        [Header("-Grenade Setting")]
        [Tooltip("접착 여부")]
        [SerializeField]
        private bool adhesion;
        private bool isObjectAdhered = false;
        private GameObject adheredObject; // 부착한 오브젝트
        private Vector3 adheredVector;

        private void Update()
        {
            if (isObjectAdhered)
            {
                transform.position = adheredObject.transform.position - adheredVector;
            }

            if (Time.time - shotTime > holdingTime)
            {
                Explosion();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (adhesion)
            {
                Projectile projectile = GetComponent<Projectile>();
                projectile.moveSpeed = 0f;
                projectile.isUseGravity = false;

                Destroy(GetComponent<Collider>());
                Destroy(GetComponent<Rigidbody>());
                Destroy(GetComponent<Projectile>());

                isObjectAdhered = true;
                adheredObject = collision.gameObject;
                adheredVector = collision.transform.position - transform.position;
            }
        }
    }
}