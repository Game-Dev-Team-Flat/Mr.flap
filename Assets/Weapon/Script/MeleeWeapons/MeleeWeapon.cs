using System.Collections;
using UnityEngine;

namespace Item.Weapon
{
    public class MeleeWeapon : UseWeapon
    {
        public float attackRate;
        public float range;
        public float damage;

        public override void WeaponAction()
        {
            if (startFire)
            {
                StartCoroutine(nameof(MeleeAction));
            }
            else if (stopFire)
            {
                StopCoroutine(nameof(MeleeAction));
            }
        }

        private IEnumerator MeleeAction()
        {
            while (true)
            {
                Stab();
                yield return null;
            }
        }

        private void Stab()
        {
            if (Time.time - lastFireTime > 1 / attackRate)
            {
                Debug.Log("Stab");
                Hit(damage, range);
                lastFireTime = Time.time;
            }
        }
    }
}