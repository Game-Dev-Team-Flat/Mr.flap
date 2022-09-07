using UnityEngine;

namespace Item.Weapon
{
    public class RocketLauncher : Gun
    {
        [Header("-Rocket Gun setting")]
        public GameObject rocket;

        public override void WeaponAction()
        {
            ExtraAction();
            base.WeaponAction();
        }

        private void ExtraAction()
        {
            if (currentAmmo > 0 && startFire && Time.time - lastFireTime > 1 / fireRate)
            {
                Debug.Log("Fire");
                Instantiate(rocket, standardObjectOfShot.position, Quaternion.LookRotation(transform.forward, Vector3.up));
                lastFireTime = Time.time;
            }
        }
    }
}