using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item.Weapon
{
    public class Boom : MonoBehaviour
    {
        [Header("-Boom Setting")]
        [SerializeField]
        protected LayerMask targetLayerMask;
        [SerializeField]
        protected bool useGravity;
        [SerializeField]
        private float gravityDownForce;
        public float moveSpeed;
        protected Vector3 velocity;
        protected float velocityY;
        [SerializeField]
        protected float holdingTime;
        protected float shotTime;
        public float explosionRadius;
        public float explosionDamage;

        private void Awake()
        {
            velocity = transform.forward * moveSpeed;
            velocityY = velocity.y;
            shotTime = Time.time;
        }

        protected void Explosion()
        {
            Debug.Log("Boom");
            RaycastHit[] objectsHit = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.up, 0f, ~LayerMask.GetMask("Floor"));
            foreach (RaycastHit objectHit in objectsHit)
            {
                if (Physics.Raycast(transform.position, (objectHit.transform.position - transform.position).normalized, out _, float.MaxValue, ~LayerMask.GetMask("Floor")))
                {
                    if (objectHit.transform.TryGetComponent(out EntityInfo entityInfo))
                    {
                        entityInfo.takenDamage += (entityInfo.transform.gameObject.layer == LayerMask.GetMask("Player")) ? (explosionDamage / 10f) : explosionDamage;
                    }
                }
            }
            // 이펙트 발사
            Destroy(gameObject);
        }

        protected void Movement()
        {
            if (useGravity)
            {
                velocityY -= gravityDownForce * Time.deltaTime;
                velocity.y = velocityY;
            }
            transform.position = transform.position + velocity * Time.deltaTime;
        }
    }
}