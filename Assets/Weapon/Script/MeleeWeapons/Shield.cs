using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Item.Weapon
{
    public class Shield : MeleeWeapon
    {
        public float pushPower;
        public float stunDuration;

        public override void WeaponAction()
        {
            float defaultLastFireTime = lastFireTime;
            base.WeaponAction();

            if (lastFireTime != defaultLastFireTime)
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
                            playerController.extraCharacterVelocity += Push(colliderHit[i].transform.position);
                        }
                        //else if (colliderHit[i].transform.gameObject.layer == LayerMask.NameToLayer("Enemy")) // 목표가 Enemy일 때
                        //{
                        //    NavMeshAgent enemyNavMeshAgent = colliderHit[i].transform.GetComponent<NavMeshAgent>();
                        //    enemyNavMeshAgent.enabled = false;
                        //}
                        StartCoroutine(ShieldDash(transform.root.transform.forward, 2f, 5f)); // 오류 있음
                    }
                }
            }
        }

        private void GrantStun(Transform grantedTransform)
        {
            grantedTransform.GetComponent<EntityInfo>().effect.stun = stunDuration;
        }

        private Vector3 Push(Vector3 targetPosition)
        {
            return (targetPosition - standardObjectOfShot.transform.position).normalized * pushPower;
        }

        private IEnumerator ShieldDash(Vector3 direction, float distance, float speed)
        {
            Vector3 defaultRootPosition = transform.root.position;
            CharacterController characterController = transform.root.GetComponent<CharacterController>();
            while (Vector3.Distance(defaultRootPosition, transform.root.position) < distance)
            {
                characterController.Move(direction * Time.deltaTime * speed * 10f);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}