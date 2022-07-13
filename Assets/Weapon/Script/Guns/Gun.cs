using UnityEngine;

namespace Item.Weapon
{
    public class Gun : MonoBehaviour
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

        public enum ShotMode
        {
            Semiauto = 1,
            Auto = 2,
            Burst = 4
        }
    }
}
