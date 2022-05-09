using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseWeapon : UseWeapon
{
    [Header("-Weapon")]
    [SerializeField]
    private Gun gun;
    [SerializeField]
    private Knife knife;
    [Space(10)]
    [SerializeField]
    private KeyCode keyReload;
    [SerializeField]private int slotNumber;

    private void Update()
    {
        Health = 0;
        reload = Input.GetKeyDown(keyReload);
        SwapWeapon();

        if (slotNumber == 0)
        {
            startFire = Input.GetMouseButtonDown(0);
            stopFire = Input.GetMouseButtonUp(0);
            WeaponAction(gun);
        }
        else if (slotNumber == 1)
        {
            WeaponAction(knife);
        }
    }

    private void SwapWeapon()
    {
        if (!IsFire)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f) slotNumber = (slotNumber == 0) ? 1 : 0;
            if (Input.GetKeyDown(KeyCode.Alpha1)) slotNumber = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) slotNumber = 1;
        }
    }
}
