using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Item.Weapon
{
    public class Shield : MeleeWeapon
    {
        public float pushPower;
        public float stunDuration;
        [SerializeField]
        private bool isUsePushAnimation; // 보스가 Push animation 꺼내 쓰니까 오류 생겨서 만든 변수
        private float changedLastFireTime;

        public override void WeaponAction()
        {
            base.WeaponAction();

            if (changedLastFireTime != lastFireTime)
            {
                RaycastHit[] colliderHit = Physics.SphereCastAll(transform.position, range, Vector3.up, 0f, targetLayerMask);//~((int)Mathf.Pow(2, gameObject.layer) | Physics.IgnoreRaycastLayer));

                for (int i = 0; i < colliderHit.Length; i++)
                {
                    if (!colliderHit[i].transform.CompareTag("Weapon"))
                    {
                        GrantStun(colliderHit[i].transform);
                        if (colliderHit[i].transform.gameObject.layer == LayerMask.NameToLayer("Player"))     // 목표가 Player일 때
                        {
                            PlayerController playerController = colliderHit[i].transform.GetComponent<PlayerController>();
                            if (isUsePushAnimation)
                            {
                                GetComponent<Animator>().Play("Push");
                            }
                            playerController.extraCharacterVelocity += Push(colliderHit[i].transform.position);
                            playerController.extraCharacterVelocityY = 0f;
                        }
                        //else if (colliderHit[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) // 목표가 Enemy일 때
                        //{
                        //    NavMeshAgent enemyNavMeshAgent = colliderHit[i].transform.GetComponent<NavMeshAgent>();
                        //    enemyNavMeshAgent.enabled = false;
                        //}
                    }
                }
                changedLastFireTime = lastFireTime;
            }
        }

        private void GrantStun(Transform grantedTransform)
        {
            grantedTransform.GetComponent<EntityInfo>().effect.Stun = stunDuration;
        }

        private Vector3 Push(Vector3 targetPosition)
        {
            return (targetPosition - standardObjectOfShot.transform.position).normalized * pushPower;
        }
    }
}