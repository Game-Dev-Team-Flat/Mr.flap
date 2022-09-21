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
        [Tooltip("폭탄 유지 시간")]
        [SerializeField]
        protected float holdingTime;
        protected float shotTime;
        public float explosionRadius;
        public float explosionDamage;

        private void Awake()
        {
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
                    continue;
                }

                if (((int)Mathf.Pow(2, objectHit.transform.gameObject.layer) & targetLayerMask & ~ignoreLayerMask) != 0)
                {
                    objectHit.transform.GetComponent<EntityInfo>().takenDamage += explosionDamage;
                }
            }
            // 이펙트 발사
            Destroy(gameObject);
        }
    }
}