using UnityEngine;
using System.Collections;

namespace Item.Weapon
{
    public class Gun : UseWeapon
    {
        [Header("-Audio")]
        //public AudioClip audioClipFire;

        [Header("-Public Gun setting")]
        [SerializeField]
        private bool m_infinityAmmo;
        public int ownAmmo;
        public int magazineMaxAmmo;
        public int currentAmmo;
        [Tooltip("Shot Per Second")]
        public float fireRate;
        public float reloadTime;
        public bool autoReload = false;
        public float range;
        public float damage;
        [EnumFlags]
        public ShotMode shotMode;
        public ShotMode currentShotMode;
        [SerializeField]
        protected GameObject projectileObject;
        protected GameObject LaunchedProjectile;

        public bool infinityAmmo => m_infinityAmmo;

        public enum ShotMode
        {
            Semiauto = 1,
            Auto = 2,
            Burst = 4
        }

        public override void WeaponAction()
        {
            if (!isReload)
            {
                if (startFire)
                {
                    StartCoroutine(nameof(GunAction));
                }
                else if (stopFire)
                {
                    StopCoroutine(nameof(GunAction));
                }
                if (inputReload)
                {
                    StartCoroutine(nameof(OnReload), this);
                }
            }
        }

        protected IEnumerator GunAction()
        {
            if (currentAmmo > 0)
            {
                switch (currentShotMode)
                {
                    case ShotMode.Auto:
                        while (currentAmmo > 0 && !isReload)
                        {
                            Fire();
                            yield return null;
                        }
                        break;
                    case ShotMode.Burst:
                        int theNumberOfFire = 0;
                        while (currentAmmo > 0 && !isReload && theNumberOfFire < 4)
                        {
                            Fire();
                            theNumberOfFire++;
                            yield return null;
                        }
                        break;
                    case ShotMode.Semiauto:
                        Fire();
                        yield return null;
                        break;
                }
            }
            if (autoReload && !isReload && currentAmmo <= 0 && (ownAmmo > 0 || infinityAmmo))
            {
                StartCoroutine(nameof(OnReload), this);
            }
        }

        private void Fire()
        {
            if (Time.time - lastFireTime > 1 / fireRate)
            {
                currentAmmo--;
                LaunchedProjectile = Instantiate(projectileObject, standardObjectOfShot.position, Quaternion.LookRotation(transform.forward, Vector3.up));
                LaunchedProjectile.GetComponent<Projectile>().gunUsingThis = this;
                //Hit(damage, range);
                lastFireTime = Time.time;
                //PlaySound(gun.audioClipFire);
            }
        }
    }
}
