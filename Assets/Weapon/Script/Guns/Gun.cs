using UnityEngine;
using System.Collections;

namespace Item.Weapon
{
    public class Gun : UseWeapon
    {
        [Header("-Audio")]
        //public AudioClip audioClipFire;

        [Header("-Public Gun setting")]
        public int maxAmmo;
        public int currentAmmo;
        public float fireRate;
        public float reloadTime;
        public bool autoReload = false;
        public float range;
        public float damage;
        [EnumFlags]
        public ShotMode shotMode;
        public ShotMode currentShotMode;
        [SerializeField]
        private ParticleSystem m_bulletParticle;
        private ParticleSystem bulletParticle
        {
            get
            {
                if (m_bulletParticle == null)
                {
                    m_bulletParticle = standardObjectOfShot.GetComponent<ParticleSystem>();
                }
                return m_bulletParticle;
            }
        }

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
                        while (currentAmmo > 0 && !isReload && theNumberOfFire < 3)
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
            if (autoReload && currentAmmo <= 0 && !isReload)
            {
                StartCoroutine(nameof(OnReload), this);
            }
        }

        private void Fire()
        {
            if (Time.time - lastFireTime > 1 / fireRate)
            {
                PlayBulletParticle();
                currentAmmo--;
                Hit(damage, range);
                lastFireTime = Time.time;
                //PlaySound(gun.audioClipFire);
            }
        }

        public virtual void PlayBulletParticle()
        {
            bulletParticle.Play();
        }
    }
}
