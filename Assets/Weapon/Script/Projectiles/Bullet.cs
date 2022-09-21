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

        private void OnCollisionEnter(Collision collision)
        {
            if (((int)Mathf.Pow(2, collision.gameObject.layer) & (gunUsingThis.targetLayerMask | LayerMask.GetMask("Floor"))) != 0)
            {
                if (collision.transform.TryGetComponent(out EntityInfo entityInfo))
                {
                    entityInfo.takenDamage += gunUsingThis.damage;
                }
                Destroy(gameObject);
            }
        }
    }
}