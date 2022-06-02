using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

public class PlayerUseWeapon : UseWeapon
{
    private PlayerInfo playerInfo;
    private PlayerInventoryManager playerInventoryManager;
    private bool isCharged = false;

    private void Awake()
    {
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerInfo = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        GameObject itemHand = playerInventoryManager.itemHand;
        if (itemHand.tag == "Weapon")
        {
            inputReload = Input.GetKeyDown(playerInfo.reloadKey);

            startFire = ActiveStartFire(itemHand);
            stopFire  = ActiveStopFire(itemHand);

            if (itemHand.TryGetComponent(out RocketLauncher _rocketLauncher) && _rocketLauncher.currentAmmo > 0 && startFire)
            {
                Instantiate(_rocketLauncher.rocket, playerInfo.eyesOfObject.transform.position + playerInfo.eyesOfObject.transform.forward * 1f + playerInfo.eyesOfObject.transform.up * -0.3f + playerInfo.eyesOfObject.transform.right * 0.3f, playerInfo.eyesOfObject.transform.rotation);
            }
            if (itemHand.TryGetComponent(out Gun _gun))
            {
                WeaponAction(_gun);
            }
            if (itemHand.TryGetComponent(out Knife _knife))
            {
                WeaponAction(_knife);
            }
            
            if (Input.GetKeyDown(playerInfo.changeShotModeKey) && _gun != null)
            {
                ChangeShotMode(_gun);
            }
        }
    }

    private void ChangeShotMode(Gun _gun)
    {
        if (_gun.currentShotMode == Gun.ShotMode.Semiauto)
        {
            if ((int)_gun.shotMode / 2 % 2 != 0)
            {
                _gun.currentShotMode = Gun.ShotMode.Auto;
            }
            else if ((int)_gun.shotMode / 4 % 2 != 0)
            {
                _gun.currentShotMode = Gun.ShotMode.Burst;
            }
        }
        if (_gun.currentShotMode == Gun.ShotMode.Auto)
        {
            if ((int)_gun.shotMode / 4 % 2 != 0)
            {
                _gun.currentShotMode = Gun.ShotMode.Burst;
            }
            else if ((int)_gun.shotMode % 2 != 0)
            {
                _gun.currentShotMode = Gun.ShotMode.Semiauto;
            }

        }
        if (_gun.currentShotMode == Gun.ShotMode.Burst)
        {
            if ((int)_gun.shotMode % 2 != 0)
            {
                _gun.currentShotMode = Gun.ShotMode.Semiauto;
            }
            else if ((int)_gun.shotMode / 2 % 2 != 0)
            {
                _gun.currentShotMode = Gun.ShotMode.Auto;
            }
        }
    }

    private bool ActiveStartFire(GameObject _weapon)
    {
        if (_weapon.TryGetComponent(out ChargingGun _chargingGun))
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine("Charging", _chargingGun.chargeTime);
            }

            if ((Input.GetMouseButton(0) || Input.GetMouseButtonUp(0)) && !isReload)
            {
                if (isCharged)
                {
                    Debug.Log("Charged");
                    if (Input.GetMouseButtonUp(0))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                StopCoroutine("Charging");
                isCharged = false;
                return false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private bool ActiveStopFire(GameObject _weapon)
    {
        if (_weapon.TryGetComponent(out ChargingGun _chargingGun))
        {
            return false;
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private IEnumerator Charging(float _chargeTime)
    {
        float _chargingTime = 0;
        while (_chargingTime < _chargeTime)
        {
            _chargingTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        isCharged = true;
    }
}
