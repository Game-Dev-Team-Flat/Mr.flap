using System.Collections;
using UnityEngine;

namespace Item.Weapon
{
    public abstract class UseWeapon : MonoBehaviour
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
        private Transform m_standardObjectOfShot;
        public Transform standardObjectOfShot
        {
            get
            {
                if (m_standardObjectOfShot == null)
                {
                    m_standardObjectOfShot = transform.Find("Fire Point");
                }
                return m_standardObjectOfShot;
            }
        }

        private RaycastHit m_collidertHit;
        public RaycastHit colliderHit => m_collidertHit;
        [SerializeField]
        private LayerMask m_targetLayerMask;
        public LayerMask targetLayerMask => m_targetLayerMask;
        protected bool m_isReload = false;
        public bool isReload => m_isReload;
        [HideInInspector]
        public bool inputReload;
        [HideInInspector]
        public bool startFire;
        [HideInInspector]
        public bool stopFire;
        protected float lastFireTime;

        public abstract void WeaponAction();


        protected void Hit(float damage, float range)
        {
            if (Physics.Raycast(standardObjectOfShot.position, standardObjectOfShot.forward, out m_collidertHit, range, (targetLayerMask | LayerMask.GetMask("Floor")) & ~(int)Mathf.Pow(2, transform.root.gameObject.layer)))
            {
                Debug.Log("Take Damage");
                if (m_collidertHit.collider.TryGetComponent(out EntityInfo entityInfo))
                {
                    entityInfo.takenDamage += damage;
                }
            }
        }

        protected IEnumerator OnReload(Gun gun)
        {
            m_isReload = true;

            yield return new WaitForSeconds(gun.reloadTime);

            if (gun.infinityAmmo)
            {
                gun.currentAmmo = gun.magazineMaxAmmo;
            }
            else if (gun.ownAmmo < gun.magazineMaxAmmo) // 탄창 요구 총량이 가진 총알보다 많을 때
            {
                gun.currentAmmo = gun.ownAmmo;
                gun.ownAmmo = 0;
            }
            else // 가진 총알이 충분할때
            {
                gun.ownAmmo -= gun.magazineMaxAmmo;
                gun.currentAmmo = gun.magazineMaxAmmo;
            }

            Debug.Log("Reload Complete");

            m_isReload = false;
        }

        protected void PlaySound(AudioClip clip)
        {
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}