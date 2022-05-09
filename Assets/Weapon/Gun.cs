using UnityEngine;

[CreateAssetMenu]
public class Gun : ScriptableObject
{
    public int maxAmmo;
    public int currentAmmo;
    public float fireRate;
    public float reloadTime;
    public float range;
    public float damage;
}
