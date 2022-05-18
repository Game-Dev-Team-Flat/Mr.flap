using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

public class PlayerUseWeapon : UseWeapon
{
    [Header("-Weapon")]
    [SerializeField]
    private GameObject[] weapons;
    [Space(10)]
    [SerializeField]
    private KeyCode keyReload;
    [SerializeField]
    private int slotNumber;

    private void Update()
    {
        inputReload = Input.GetKeyDown(keyReload);
        SwapWeapon();

        startFire = Input.GetMouseButtonDown(0);
        stopFire = Input.GetMouseButtonUp(0);

        if (weapons[slotNumber].TryGetComponent(out Gun gun)) WeaponAction(gun);
        else if (weapons[slotNumber].TryGetComponent(out Knife knife)) WeaponAction(knife);
    }

    private void SwapWeapon()
    {
        if (!isReload)
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") > 0) slotNumber++;
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0) slotNumber--; // 휠로 슬롯 변경

            if (slotNumber >= weapons.Length) slotNumber = 0;
            else if (slotNumber < 0) slotNumber = weapons.Length - 1; // 슬롯 제한

            for (int i = 0; i < weapons.Length; i++)
            {
                if (Input.GetKeyDown((KeyCode)(48 + i))) slotNumber = i - 1; // alpha Number로 슬롯 변경
            }
        }
    }
}
