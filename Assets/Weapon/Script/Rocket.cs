using UnityEngine;

public class Rocket : Boom
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == targetLayerMask)
        {
            Explosion();
        }
    }
}
