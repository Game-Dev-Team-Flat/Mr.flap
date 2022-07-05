using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon;

namespace Enemy
{
    public class EnemyUseWeapon : UseWeapon
    {
        [SerializeField]
        private EnemyController enemyController;
        private DetectTarget detectTarget;
        [SerializeField]
        private float shootAngle;
        [SerializeField]
        private float shootDistance;
        [SerializeField]
        private EntityInfo enemyInfo;

        private void Awake()
        {
            detectTarget = GetComponent<DetectTarget>();
            enemyController = GetComponent<EnemyController>();
        }

        private void Update()
        {
            Targeting();

            startFire = IsInFireArea();
            stopFire = !IsInFireArea();

            if (enemyInfo.inventory[0].item.TryGetComponent(out Gun gun))
            {
                WeaponAction(gun);
            }
        }

        private bool IsInFireArea()
        {
            if (detectTarget.detectedObject != null && enemyController.enemyState == EnemyController.EnemyState.DetectingTarget)
            {
                float radianRange = Mathf.Cos(shootAngle / 2 * Mathf.Deg2Rad);

                float targetRadian = Vector3.Dot(standardObjectOfShot.forward, (enemyController.targetObject.transform.position - transform.position).normalized);

                if (radianRange > targetRadian && Vector3.Distance(enemyController.targetObject.transform.position, transform.position) < shootDistance)
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

        private void Targeting()
        {
            if (detectTarget.detectedObject != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(enemyController.targetObject.transform.position - enemyInfo.inventory[0].item.transform.position);

                float rotateSpeed = 5f;
                targetRotation = Quaternion.Euler(new Vector3(targetRotation.eulerAngles.x, enemyInfo.inventory[0].item.transform.eulerAngles.y, enemyInfo.inventory[0].item.transform.eulerAngles.z));
                enemyInfo.inventory[0].item.transform.rotation = Quaternion.Slerp(enemyInfo.inventory[0].item.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
            else
            {
                float rotateSpeed = 5f;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, enemyInfo.inventory[0].item.transform.eulerAngles.y, enemyInfo.inventory[0].item.transform.eulerAngles.z));
                enemyInfo.inventory[0].item.transform.rotation = Quaternion.Slerp(enemyInfo.inventory[0].item.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }
    }
}