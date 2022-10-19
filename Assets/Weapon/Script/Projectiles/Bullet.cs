using UnityEngine;

namespace Item.Weapon
{
    public class Bullet : Projectile
    {
        protected override void Awake()
        {
            base.Awake();
            Destroy(gameObject, 10);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((int)Mathf.Pow(2, other.gameObject.layer) & (gunUsingThis.targetLayerMask | LayerMask.GetMask("Floor"))) != 0)
            {
                if (other.transform.TryGetComponent(out EntityInfo entityInfo))
                {
                    entityInfo.takenDamage += gunUsingThis.damage;
                }
                Destroy(gameObject);
            }
        }
    }
}