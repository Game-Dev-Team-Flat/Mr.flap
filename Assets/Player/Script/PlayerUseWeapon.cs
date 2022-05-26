using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

public class PlayerUseWeapon : UseWeapon
{
    [Header("-Weapon")]
    [SerializeField]
    private List<GameObject> weapons;
    private GameObject weapon;
    [Space(10)]
    [SerializeField]
    private KeyCode reloadKey;
    [SerializeField]
    private KeyCode changeShotModeKey;
    [SerializeField]
    private int slotNumber;
    private bool isCharged = false;

    private void Awake()
    {
        weapon = Instantiate(weapons[slotNumber], transform.position + eyesOfObject.transform.forward * 0.5f + eyesOfObject.transform.up * -0.3f + eyesOfObject.transform.right * 0.3f, eyesOfObject.transform.rotation, eyesOfObject.transform);
    }

    private void Update()
    {
        inputReload = Input.GetKeyDown(reloadKey);
        SwapWeapon();
        ChangeShotMode();

        startFire = ActiveStartFire(weapons[slotNumber]);
        stopFire  = ActiveStopFire(weapons[slotNumber]);
        if (weapons[slotNumber].TryGetComponent(out RocketLauncher rocketLauncher) && rocketLauncher.currentAmmo > 0 && startFire)
        {
            Instantiate(rocketLauncher.rocket, eyesOfObject.transform.position + eyesOfObject.transform.forward * 1f + eyesOfObject.transform.up * -0.3f + eyesOfObject.transform.right * 0.3f, eyesOfObject.transform.rotation);
        }
        if (weapons[slotNumber].TryGetComponent(out Knife knife)) WeaponAction(knife);
        else if (weapons[slotNumber].TryGetComponent(out Gun gun)) WeaponAction(gun);
    }

    private void SwapWeapon()
    {
        int defaultSlotNumber = slotNumber;
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) slotNumber++;
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) slotNumber--; // 휠로 슬롯 변경

        for (int i = 0; i < weapons.Count; i++)
        {
            if (Input.GetKeyDown((KeyCode)(49 + i))) slotNumber = i; // alpha Number로 슬롯 변경
        }

        if (slotNumber >= weapons.Count) slotNumber = 0;
        else if (slotNumber < 0) slotNumber = weapons.Count - 1; // 슬롯 제한

        if (slotNumber != defaultSlotNumber) // 슬롯 변경됐을 때
        {
            DestroyAndInstantiateWeapon(); // 기존 무기를 제거하고 Slot Number에 맞는 무기 소환
        }
    }

    private void ChangeShotMode()
    {
        if (weapons[slotNumber].TryGetComponent(out Gun _gun) && Input.GetKeyDown(changeShotModeKey))
        {
            if(_gun.currentShotMode == Gun.ShotMode.Semiauto)
            {
                if      ((int)_gun.shotMode / 2 % 2 == 1) _gun.currentShotMode = Gun.ShotMode.Auto;
                else if ((int)_gun.shotMode / 4 % 2 == 1) _gun.currentShotMode = Gun.ShotMode.Burst;
            }
            if(_gun.currentShotMode == Gun.ShotMode.Auto)
            {
                if  ((int)_gun.shotMode / 4 % 2 == 1) _gun.currentShotMode = Gun.ShotMode.Burst;
                else if ((int)_gun.shotMode % 2 == 1) _gun.currentShotMode = Gun.ShotMode.Semiauto;
            }
            if(_gun.currentShotMode == Gun.ShotMode.Burst)
            {
                if          ((int)_gun.shotMode % 2 == 1) _gun.currentShotMode = Gun.ShotMode.Semiauto;
                else if ((int)_gun.shotMode / 2 % 2 == 1) _gun.currentShotMode = Gun.ShotMode.Auto;
            }
        }
    }

    private void DestroyAndInstantiateWeapon()
    {
        StopCoroutine("OnReload");
        isReload = false;
        Destroy(weapon);
        weapon = Instantiate(weapons[slotNumber], transform.position + eyesOfObject.transform.forward * 0.5f + eyesOfObject.transform.up * -0.3f + eyesOfObject.transform.right * 0.3f, eyesOfObject.transform.rotation, eyesOfObject.transform);
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
        if(_weapon.TryGetComponent(out ChargingGun _chargingGun))
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
