using Item.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Normal
{
    public class SniperEnemyController : RangeEnemyController
    {
        [Space(20)]
        [SerializeField]
        private GameObject dotsite;
        private UseWeapon usingWeapon;

        private void Awake()
        {
            dotsite = Instantiate(dotsite, transform.position, Quaternion.identity);
            usingWeapon = enemyInfo.inventory[enemyInfo.inventorySlotNumber].item.GetComponent<UseWeapon>();
        }

        protected override void Update()
        {
            base.Update();

            if (Physics.Raycast(usingWeapon.standardObjectOfShot.transform.position, usingWeapon.standardObjectOfShot.forward, out RaycastHit raycastHit))
            {
                dotsite.transform.position = raycastHit.point;
            }
            else
            {
                dotsite.transform.position = transform.position;
            }

            if (dotsite.GetComponent<Dotsite>().targetLockOn)
            {
                usingWeapon.startFire = true;
                usingWeapon.WeaponAction();
            }
        }

        protected override void HandleTargetHindInObject()
        {
            HandleMissingTargetSideways();
        }

        protected override void HandleTargetOverDetectingDistance()
        {
            HandleMissingTargetSideways();
        }

        private void OnDestroy()
        {
            Destroy(dotsite);
        }
    }
}
