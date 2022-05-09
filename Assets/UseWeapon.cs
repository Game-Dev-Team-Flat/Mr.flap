using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject eyesOfObject;
    [HideInInspector]
    protected RaycastHit collidertHit;
    public LayerMask target;
    private float lastFireTime;
    private bool isReload = false;
    private bool isFire = false;
    public bool IsFire => isFire;
    protected bool reload;
    protected bool startFire;
    protected bool stopFire;
    protected float Health { get; set; }

    protected void WeaponAction(Gun _gun)
    {
        if (startFire)
        {
            isFire = true;
            StartCoroutine(GunAction(_gun));
        }
        else if (stopFire)
        {
            isFire = false;
        }
        if (reload && !isReload)
        {
            StartCoroutine(OnReload(_gun));
        }
    }

    protected void WeaponAction(Knife _knife)
    {
        if (Input.GetMouseButtonDown(0))
        {
            isFire = true;
            StartCoroutine(KnifeAction(_knife));
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isFire = false;
        }
    }

    private IEnumerator GunAction(Gun _gun)
    {
        while (isFire && !isReload)
        {
            if (_gun.currentAmmo > 0)
            {
                Shot(_gun);
            }
            yield return null;
        }
        yield return null;
    }

    private IEnumerator KnifeAction(Knife _knife)
    {
        while (isFire)
        {
            Stab(_knife);
            yield return null;
        }
    }

    private void Shot(Gun _gun)
    {
        if (Time.time - lastFireTime > _gun.fireRate)
        {
            _gun.currentAmmo--;
            Hit(_gun.damage, _gun.range);
            lastFireTime = Time.time;
        }
    }

    private void Stab(Knife _knife)
    {
        if (Time.time - lastFireTime > _knife.attackRate)
        {
            Hit(_knife.damage, _knife.range);
            lastFireTime = Time.time;
        }
    }

    private void Hit(float _damage, float _range)
    {
        if (Physics.Raycast(eyesOfObject.transform.position, eyesOfObject.transform.forward, out collidertHit, _range, target))
        {
            Health -= _damage;
            Debug.Log("take damage");
        }
        else Debug.Log("miss");
    }

    private IEnumerator OnReload(Gun _gun)
    {
        isReload = true;
        float _reloadTime = _gun.reloadTime;
        while (_reloadTime > 0)
        {
            _reloadTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Reload Complete");
        _gun.currentAmmo = _gun.maxAmmo;
        isReload = false;
    }
}