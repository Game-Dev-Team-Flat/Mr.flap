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
        protected UseWeapon enemyWeapon
        {
            get
            {
                if (m_enemyWeapon == null)
                {
                    m_enemyWeapon = enemyInfo.inventory[0].item.GetComponent<UseWeapon>();
                }
                return m_enemyWeapon;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(enemyWeapon.standardObjectOfShot.transform.position, EulerToVector( shotAngle / 2) * shotDistance);
            Gizmos.DrawRay(enemyWeapon.standardObjectOfShot.transform.position, EulerToVector(-shotAngle / 2) * shotDistance);
        }

        protected virtual void Update()
        {
            if (isTargeting)
            {
                Targeting();
            }

            enemyWeapon.startFire = IsInFireArea();
            enemyWeapon.stopFire = !IsInFireArea();

            enemyWeapon.WeaponAction();
        }

        protected bool IsInFireArea()
        {
            if (detectTarget.detectedObject != null && enemyController.enemyState == EnemyController.EnemyState.DetectingTarget)
            {
                float radianRange = Mathf.Cos(shotAngle / 2 * Mathf.Deg2Rad);

                float targetRadian = Vector3.Dot(enemyWeapon.standardObjectOfShot.forward, (enemyController.targetObject.transform.position - transform.position).normalized);

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

        private Vector3 EulerToVector(float degree)
        {
            degree += transform.eulerAngles.y;
            degree *= Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(degree), 0, Mathf.Cos(degree));
        }
    }
}