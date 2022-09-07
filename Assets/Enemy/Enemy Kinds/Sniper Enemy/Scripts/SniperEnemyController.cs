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
        private GameObject Dotsite;
        private UseWeapon usingWeapon;

        private void Awake()
        {
            Dotsite = Instantiate(Dotsite, transform.position, Quaternion.identity);
            usingWeapon = enemyInfo.inventory[enemyInfo.inventorySlotNumber].item.GetComponent<UseWeapon>();
        }

        protected override void Update()
        {
            base.Update();
            if (Physics.Raycast(usingWeapon.standardObjectOfShot.transform.position, usingWeapon.standardObjectOfShot.forward, out RaycastHit raycastHit))
            {
                Dotsite.transform.position = raycastHit.point;
            }
            else
            {
                Dotsite.transform.position = transform.position;
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
    }
}
