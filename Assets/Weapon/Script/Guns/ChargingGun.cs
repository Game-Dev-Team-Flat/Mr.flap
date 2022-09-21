using System.Collections;
using UnityEngine;

namespace Item.Weapon
{
    public class ChargingGun : Gun
    {
        [Header("-Charging Gun setting")]
        public float chargeTime;
        private bool isCharged = false;


        public override void WeaponAction()
        {
            if (!isReload)
            {
                if (startFire)
                {
                    StartCoroutine(nameof(Charging), chargeTime);
                }

                if (stopFire)
                {
                    StopCoroutine(nameof(Charging));

                    if (isCharged)
                    {
                        StartCoroutine(nameof(GunAction));
                        isCharged = false;
                    }
                }

                if(isCharged)
                {
                    Debug.Log("Charged!");
                }

                if (inputReload)
                {
                    StartCoroutine(nameof(OnReload), this);
                }
            }
        }

        

        private IEnumerator Charging(float chargeTime)
        {
            float chargingTime = 0;
            while (chargingTime < chargeTime)
            {
                chargingTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            isCharged = true;
        }
    }
}
