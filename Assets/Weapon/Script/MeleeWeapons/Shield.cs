using UnityEngine;

namespace Item.Weapon
{
    public class Shield : MeleeWeapon
    {
        public float pushPower;

        public Vector3 Push(Collider target, float pushPower)
        {
            return (transform.position - target.transform.position).normalized * pushPower;
        }
    }
}