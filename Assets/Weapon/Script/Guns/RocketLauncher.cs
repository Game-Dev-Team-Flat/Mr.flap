using UnityEngine;

namespace Item.Weapon
{
    public class RocketLauncher : Gun
    {
        public override void WeaponAction()
        {
            ExtraAction();
        }

        private void ExtraAction()
        {
            if (currentAmmo > 0 && startFire && Time.time - lastFireTime > 1 / fireRate)
            {
                Debug.Log("Fire");
                LaunchedProjectile = Instantiate(projectileObject, standardObjectOfShot.position, Quaternion.LookRotation(transform.forward, Vector3.up));
                LaunchedProjectile.GetComponent<Boom>().ignoreLayerMask = (int)Mathf.Pow(2, transform.root.gameObject.layer);
                ownAmmo--;
                lastFireTime = Time.time;
            }
        }
    }
}