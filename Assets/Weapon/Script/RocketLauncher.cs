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
        }

        private void ExtraAction()
        {
            if (currentAmmo > 0 && startFire)
            {
                //Instantiate(rocket, playerInfo.eyesOfObject.position + playerInfo.eyesOfObject.forward * 1f + playerInfo.eyesOfObject.up * -0.3f + playerInfo.eyesOfObject.right * 0.3f, playerInfo.eyesOfObject.rotation);
            }
        }
    }
}
