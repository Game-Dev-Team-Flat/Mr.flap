using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Item.Weapon;

namespace Enemy
{
    public class EnemyWeaponController : MonoBehaviour
    {
        private EnemyController m_enemyController;
        private DetectTarget m_detectTarget;
        private EntityInfo m_enemyInfo;
        private UseWeapon m_enemyWeapon;

        [SerializeField]
        protected bool isTargeting;
        protected EnemyController enemyController
        {
            get
            {
                if (m_enemyController == null)
                {
                    m_enemyController = GetComponent<EnemyController>();
                }
                return m_enemyController;
            }
            set => m_enemyController = value;
        }
        protected DetectTarget detectTarget
        {
            get
            {
                if (m_detectTarget == null)
                {
                    m_detectTarget = GetComponent<DetectTarget>();
                }
                return m_detectTarget;
            }
            set => m_detectTarget = value;
        }
        protected EntityInfo enemyInfo
        {
            get
            {
                if (m_enemyInfo == null)
                {
                    m_enemyInfo = GetComponent<EntityInfo>();
                }
                return m_enemyInfo;
            }
            set => m_enemyInfo = value;
        }
        [SerializeField]
        protected float shotAngle;
        [SerializeField]
        protected float shotDistance;
        protected bool isShooting = false;
        private int defaultInventorySlotNumber;

        protected UseWeapon enemyWeapon
        {
            get
            {
                if (m_enemyWeapon == null || enemyInfo.inventorySlotNumber != defaultInventorySlotNumber)
                {
                    m_enemyWeapon = enemyInfo.inventory[enemyInfo.inventorySlotNumber].item.GetComponent<UseWeapon>();
                    defaultInventorySlotNumber = enemyInfo.inventorySlotNumber;
                }
                return m_enemyWeapon;
            }
            set => m_enemyWeapon = value;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(enemyWeapon.standardObjectOfShot.transform.position,
                EulerToVector(enemyWeapon.standardObjectOfShot.transform, shotAngle / 2) * shotDistance);
            Gizmos.DrawRay(enemyWeapon.standardObjectOfShot.transform.position,
                EulerToVector(enemyWeapon.standardObjectOfShot.transform, -shotAngle / 2) * shotDistance);
        }

        protected virtual void Update()
        {
            if (isTargeting)
            {
                Targeting();
            }

            if (IsInFireArea())
            {
                if (!isShooting)
                {
                    Debug.Log("Find");
                    enemyWeapon.stopFire = false;
                    enemyWeapon.startFire = true;
                    isShooting = true;
                }
                else
                {
                    Debug.Log("Found");
                    enemyWeapon.startFire = false;
                }
            }
            else // 적이 못 봤을 때
            {
                Debug.Log("Not Found");
                enemyWeapon.startFire = false;
                enemyWeapon.stopFire = true;
                isShooting = false;
            }

            enemyWeapon.WeaponAction();
        }

        protected bool IsInFireArea()
        {
            if (detectTarget.detectedObject != null && enemyController.enemyState == EnemyController.EnemyState.DetectingTarget)
            {
                float radianRange = Mathf.Cos(shotAngle / 2 * Mathf.Deg2Rad);

                float targetRadian = Vector3.Dot(enemyWeapon.standardObjectOfShot.forward, (enemyController.targetObject.transform.position - enemyWeapon.standardObjectOfShot.transform.position).normalized);

                if (radianRange < targetRadian && Vector3.Distance(enemyController.targetObject.transform.position, transform.position) < shotDistance)
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

        protected void Targeting()
        {
            Quaternion targetRotation;
            float rotateSpeed = 5f;

            if (enemyController.enemyState != EnemyController.EnemyState.DetectingNothing)
            {
                targetRotation = Quaternion.LookRotation(enemyController.targetObject.transform.position - enemyInfo.inventory[enemyInfo.inventorySlotNumber].item.transform.position);

                enemyInfo.inventory[enemyInfo.inventorySlotNumber].item.transform.rotation = Quaternion.Slerp(enemyInfo.inventory[enemyInfo.inventorySlotNumber].item.transform.rotation, targetRotation, Time.deltaTime * rotateSpeed);
            }
        }

        private Vector3 EulerToVector(Transform transform, float degree)
        {
            degree += transform.eulerAngles.y;
            degree *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
        }
    }
}