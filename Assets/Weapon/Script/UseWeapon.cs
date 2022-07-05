using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class UseWeapon : MonoBehaviour
    {
        private AudioSource m_audioSource;
        private AudioSource audioSource
        {
            get
            {
                if (m_audioSource == null)
                {
                    m_audioSource = GetComponent<AudioSource>();
                }
                return m_audioSource;
            }
        }

        [SerializeField]
        protected Transform standardObjectOfShot;

        private RaycastHit m_collidertHit;
        public RaycastHit colliderHit => m_collidertHit;
        [SerializeField]
        private LayerMask targetLayerMask;
        private float lastFireTime;
        protected bool isReload = false;
        protected bool inputReload;
        protected bool startFire;
        protected bool stopFire;

        protected void WeaponAction(Gun gun)
        {
            if (!isReload)
            {
                if (startFire)
                {
                    StartCoroutine("GunAction", gun);
                }
                else if (stopFire)
                {
                    StopCoroutine("GunAction");
                }
                if (inputReload)
                {
                    StartCoroutine("OnReload", gun);
                }
            }
        }

        protected void WeaponAction(Knife knife)
        {
            if (startFire)
            {
                StartCoroutine("KnifeAction");
            }
            else if (stopFire)
            {
                StopCoroutine("KnifeAction");
            }
        }

        private IEnumerator GunAction(Gun gun)
        {
            if (gun.currentAmmo > 0)
            {
                switch (gun.currentShotMode)
                {
                    case Gun.ShotMode.Auto:
                        while (gun.currentAmmo > 0 && !isReload)
                        {
                            Shot(gun);
                            yield return null;
                        }
                        break;
                    case Gun.ShotMode.Burst:
                        int theNumberOfFire = 0;
                        while (gun.currentAmmo > 0 && !isReload && theNumberOfFire < 3)
                        {
                            Shot(gun);
                            theNumberOfFire++;
                            yield return null;
                        }
                        break;
                    case Gun.ShotMode.Semiauto:
                        Shot(gun);
                        yield return null;
                        break;
                }
            }
            if (gun.autoReload && gun.currentAmmo <= 0)
            {
                StartCoroutine("OnReload", gun);
            }
        }

        private IEnumerator KnifeAction(Knife knife)
        {
            while (true)
            {
                Stab(knife);
                yield return null;
            }
        }

        private void Shot(Gun gun)
        {
            if (Time.time - lastFireTime > 1 / gun.fireRate)
            {
                Debug.Log("Fire");
                gun.currentAmmo--;
                Hit(gun.damage, gun.range);
                lastFireTime = Time.time;
                //PlaySound(gun.audioClipFire);
            }
        }

        private void Stab(Knife knife)
        {
            if (Time.time - lastFireTime > knife.attackRate)
            {
                Debug.Log("Stab");
                Hit(knife.damage, knife.range);
                lastFireTime = Time.time;
            }
        }

        private void Hit(float damage, float range)
        {
            if (Physics.Raycast(standardObjectOfShot.position, standardObjectOfShot.forward, out m_collidertHit, range, targetLayerMask))
            {
                Debug.Log("Take Damage");
                if (m_collidertHit.transform.TryGetComponent(out EntityInfo entityInfo))
                {
                    entityInfo.takenDamage += damage;
                }
            }
        }

        protected IEnumerator OnReload(Gun gun)
        {
            isReload = true;
            float reloadTime = gun.reloadTime;
            while (reloadTime > 0)
            {
                reloadTime -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("Reload Complete");
            gun.currentAmmo = gun.maxAmmo;
            isReload = false;
        }

        private void PlaySound(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}