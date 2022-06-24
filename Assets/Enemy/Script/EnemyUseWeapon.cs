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
            startFire = IsInFireArea();
            stopFire = !IsInFireArea();

            //if (enemyInfo.inventory.item.TryGetComponent(out Gun gun)) WeaponAction();
        }

        private bool IsInFireArea()
        {
            float radianRange = Mathf.Cos(shootAngle / 2 * Mathf.Deg2Rad);

            float targetRadian = Vector3.Dot(standardObjectOfShot.forward, (detectTarget.detectedObject.transform.position - transform.position).normalized);
            
            if (radianRange < targetRadian && Vector3.Distance(detectTarget.detectedObject.transform.position, transform.position) < shootDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}