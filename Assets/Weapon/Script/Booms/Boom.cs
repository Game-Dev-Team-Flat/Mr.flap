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
        public LayerMask ignoreLayerMask;
        [SerializeField]
        protected bool isUseGravity;
        [SerializeField]
        private float gravityDownForce;
        public float moveSpeed;
        protected Vector3 velocity;
        protected float velocityY;
        [Tooltip("폭탄 유지 시간")]
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
            RaycastHit[] objectsHit = Physics.SphereCastAll(transform.position, explosionRadius, Vector3.up, 0f, (~LayerMask.GetMask("Floor") | ~Physics.IgnoreRaycastLayer) & targetLayerMask);
            foreach (RaycastHit objectHit in objectsHit)
            {
                if (Physics.Raycast(transform.position, (objectHit.transform.position - transform.position).normalized, float.MaxValue, LayerMask.GetMask("Floor")))
                {
                    return;
                }

                if (((int)Mathf.Pow(2, objectHit.transform.gameObject.layer) & targetLayerMask & ~ignoreLayerMask) != 0)
                {
                    objectHit.transform.GetComponent<EntityInfo>().takenDamage += explosionDamage;
                }
            }
            // 이펙트 발사
            Destroy(gameObject);
        }

        protected void Movement()
        {
            if (isUseGravity)
            {
                velocityY -= gravityDownForce * Time.deltaTime;
                velocity.y = velocityY;
            }
            transform.position = transform.position + velocity * Time.deltaTime;
        }
    }
}