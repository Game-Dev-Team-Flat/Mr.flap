using UnityEngine;

namespace Item.Weapon
{
    public class Rocket : Boom
    {
        private void Update()
        {
            if (Time.time - shotTime > holdingTime)
            {
                Explosion();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Explosion();
        }
    }
}