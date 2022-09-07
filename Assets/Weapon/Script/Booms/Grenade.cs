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
            Movement();

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
                velocity = Vector3.zero;
                Destroy(GetComponent<Rigidbody>());
                Destroy(GetComponent<Collider>());
                adheredObject = collision.gameObject;
                isObjectAdhered = true;
                adheredVector = collision.transform.position - transform.position;
                isUseGravity = false;
            }
        }
    }
}