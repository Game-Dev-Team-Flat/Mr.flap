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
                GameObject launchedRocket = Instantiate(rocket, standardObjectOfShot.position, Quaternion.LookRotation(transform.forward, Vector3.up));
                launchedRocket.GetComponent<Boom>().ignoreLayerMask = (int)Mathf.Pow(2, transform.root.gameObject.layer);
                lastFireTime = Time.time;
            }
        }
    }
}