using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item.Weapon;

public class PlayerWeaponController : MonoBehaviour
{
    private PlayerInfo m_playerInfo;
    private PlayerInventoryManager m_playerInventoryManager;
    private UseWeapon playerWeapon;

    private PlayerInventoryManager playerInventoryManager
    {
        get
        {
            if (m_playerInventoryManager == null)
            {
                m_playerInventoryManager = GetComponent<PlayerInventoryManager>();
            }
            return m_playerInventoryManager;
        }
    }
    private PlayerInfo playerInfo
    {
        get
        {
            if (m_playerInfo == null)
            {
                m_playerInfo = GetComponent<PlayerInfo>();
            }
            return m_playerInfo;
        }
    }

    private void Update()
    {
        GameObject itemHand = playerInventoryManager.itemHand;
        
        if (itemHand.CompareTag("Weapon"))
        {
            playerWeapon = itemHand.GetComponent<UseWeapon>();
            playerWeapon.inputReload = Input.GetKeyDown(playerInfo.reloadKey);

            playerWeapon.startFire = Input.GetMouseButtonDown(0);
            playerWeapon.stopFire = Input.GetMouseButtonUp(0);

            Debug.DrawRay(playerWeapon.standardObjectOfShot.position, playerWeapon.standardObjectOfShot.forward);

            playerWeapon.WeaponAction();

            if (Input.GetKeyDown(playerInfo.changeShotModeKey) && itemHand.TryGetComponent(out Gun gun))
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
        else if (gun.currentShotMode == Gun.ShotMode.Auto)
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
        else if (gun.currentShotMode == Gun.ShotMode.Burst)
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
}