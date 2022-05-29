using UnityEngine;

namespace Weapon
{
    public class Rocket : Boom
    {
        private void Update()
        {
            Movement();

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