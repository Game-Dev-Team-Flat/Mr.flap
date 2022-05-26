using System.Collections;
using UnityEngine;

namespace Weapon
{
    public class UseWeapon : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource audioSource;

        [SerializeField]
        protected GameObject eyesOfObject;
        private RaycastHit m_collidertHit;
        public RaycastHit colliderHit => m_collidertHit;
        [SerializeField]
        private LayerMask targetLayerMask;
        private float lastFireTime;
        private bool m_isReload = false;
        protected bool isReload => m_isReload;
        protected bool inputReload;
        protected bool startFire;
        protected bool stopFire;

        protected void WeaponAction(Gun _gun)
        {
            if (!m_isReload)
            {
                if (startFire)
                {
                    StartCoroutine("GunAction", _gun);
                }
                else if (stopFire)
                {
                    StopCoroutine("GunAction");
                }
                if (inputReload)
                {
                    StartCoroutine("OnReload", _gun);
                }
            }
        }

        protected void WeaponAction(Knife _knife)
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

        private IEnumerator GunAction(Gun _gun)
        {
            if (_gun.currentAmmo > 0)
            {
                switch (_gun.currentShotMode)
                {
                    case Gun.ShotMode.Auto:
                        while (_gun.currentAmmo > 0 && !m_isReload)
                        {
                            Shot(_gun);
                            yield return null;
                        }
                        break;
                    case Gun.ShotMode.Burst:
                        int theNumberOfFire = 0;
                        while (_gun.currentAmmo > 0 && !m_isReload && theNumberOfFire < 3)
                        {
                            Shot(_gun);
                            theNumberOfFire++;
                            yield return null;
                        }
                        break;
                    case Gun.ShotMode.Semiauto:
                        Shot(_gun);
                        yield return null;
                        break;
                }
            }
            if (_gun.autoReload && _gun.currentAmmo <= 0)
            {
                StartCoroutine("OnReload", _gun);
            }
        }

        private IEnumerator KnifeAction(Knife _knife)
        {
            while (true)
            {
                Stab(_knife);
                yield return null;
            }
        }

        private void Shot(Gun _gun)
        {
            if (Time.time - lastFireTime > 1 / _gun.fireRate)
            {
                Debug.Log("Fire");
                _gun.currentAmmo--;
                Hit(_gun.damage, _gun.range);
                lastFireTime = Time.time;
                //PlaySound(_gun.audioClipFire);
            }
        }

        private void Stab(Knife _knife)
        {
            if (Time.time - lastFireTime > _knife.attackRate)
            {
                Debug.Log("Stab");
                Hit(_knife.damage, _knife.range);
                lastFireTime = Time.time;
            }
        }

        private void Hit(float _damage, float _range)
        {
            if (Physics.Raycast(eyesOfObject.transform.position, eyesOfObject.transform.forward, out m_collidertHit, _range, targetLayerMask))
            {
                Debug.Log("Take Damage");
            }
        }

        protected IEnumerator OnReload(Gun _gun)
        {
            m_isReload = true;
            float _reloadTime = _gun.reloadTime;
            while (_reloadTime > 0)
            {
                _reloadTime -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("Reload Complete");
            _gun.currentAmmo = _gun.maxAmmo;
            m_isReload = false;
        }

        private void PlaySound(AudioClip _clip)
        {
            audioSource.Stop();
            audioSource.clip = _clip;
            audioSource.Play();
        }
    }
}