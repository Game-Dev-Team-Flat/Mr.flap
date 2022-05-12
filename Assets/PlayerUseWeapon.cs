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
    [SerializeField]private int slotNumber;

    private void Update()
    {
        Health = 0;
        reload = Input.GetKeyDown(keyReload);
        SwapWeapon();

        startFire = Input.GetMouseButtonDown(0);
        stopFire = Input.GetMouseButtonUp(0);

        if (weapons[slotNumber].TryGetComponent(out Gun gun)) WeaponAction(gun);
        else if (weapons[slotNumber].TryGetComponent(out Knife knife)) WeaponAction(knife);
    }

    private void SwapWeapon()
    {
        if (!IsFire)
        {
            slotNumber += (int)Input.GetAxisRaw("Mouse ScrollWheel");
            for (int i = 0; i < weapons.Length; i++)
            {
                if (Input.GetKeyDown((KeyCode)(48 + i))) slotNumber = i - 1;
            }
        }
    }
}
