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
        standardObjectOfShot = playerInfo.inventory[playerInfo.inventorySlotNumber].item.transform.Find("Fire Point");
        GameObject itemHand = playerInventoryManager.itemHand;
        if (itemHand.tag == "Weapon")
        {
            inputReload = Input.GetKeyDown(playerInfo.reloadKey);

            startFire = ActiveStartFire(itemHand);
            stopFire = ActiveStopFire(itemHand);

            if (itemHand.TryGetComponent(out RocketLauncher rocketLauncher) && rocketLauncher.currentAmmo > 0 && startFire)
            {
                Instantiate(rocketLauncher.rocket, playerInfo.eyesOfObject.position + playerInfo.eyesOfObject.forward * 1f + playerInfo.eyesOfObject.up * -0.3f + playerInfo.eyesOfObject.right * 0.3f, playerInfo.eyesOfObject.rotation);
            }
            if (itemHand.TryGetComponent(out Gun gun))
            {
                WeaponAction(gun);
            }
            if (itemHand.TryGetComponent(out Knife knife))
            {
                WeaponAction(knife);
            }

            if (Input.GetKeyDown(playerInfo.changeShotModeKey) && gun != null)
            {
                ChangeShotMode(gun);
            }
        }
    }

    private void ChangeShotMode(Gun gun)
    {
        if (gun.currentShotMode == Gun.ShotMode.Semiauto)
        {
            if ((int)gun.shotMode / 2 % 2 != 0)
            {
                gun.currentShotMode = Gun.ShotMode.Auto;
            }
            else if ((int)gun.shotMode / 4 % 2 != 0)
            {
                gun.currentShotMode = Gun.ShotMode.Burst;
            }
        }
        if (gun.currentShotMode == Gun.ShotMode.Auto)
        {
            if ((int)gun.shotMode / 4 % 2 != 0)
            {
                gun.currentShotMode = Gun.ShotMode.Burst;
            }
            else if ((int)gun.shotMode % 2 != 0)
            {
                gun.currentShotMode = Gun.ShotMode.Semiauto;
            }

        }
        if (gun.currentShotMode == Gun.ShotMode.Burst)
        {
            if ((int)gun.shotMode % 2 != 0)
            {
                gun.currentShotMode = Gun.ShotMode.Semiauto;
            }
            else if ((int)gun.shotMode / 2 % 2 != 0)
            {
                gun.currentShotMode = Gun.ShotMode.Auto;
            }
        }
    }

    private bool ActiveStartFire(GameObject weapon)
    {
        if (weapon.TryGetComponent(out ChargingGun chargingGun))
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine("Charging", chargingGun.chargeTime);
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

    private bool ActiveStopFire(GameObject weapon)
    {
        if (weapon.TryGetComponent(out ChargingGun chargingGun))
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