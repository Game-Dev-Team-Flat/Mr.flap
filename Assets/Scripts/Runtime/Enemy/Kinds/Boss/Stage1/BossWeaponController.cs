using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Enemy.Boss
{
    public class BossWeaponController : EnemyWeaponController
    {
        [Header("-Boss Setting")]
        [SerializeField]
        private float buttstockAttackRange;

        private Animator m_animator;
        private Animator animator
        {
            get
            {
                if (m_animator == null)
                {
                    m_animator = GetComponent<Animator>();
                }
                return m_animator;
            }
            set => m_animator = value;
        }

        private float skillCoolTime = 0f;
        private float lastSkillCastTime = 0f;
        private bool isCastingSkill = false;
        private float defaultShotAngle;
        private float defaultShotDistance;
        private BossInfo bossInfo;


        private void Awake()
        {
            bossInfo = enemyInfo as BossInfo;
            defaultShotAngle = shotAngle;
            defaultShotDistance = shotDistance;
        }

        protected override void Update()
        {
            if (enemyController.targetObject != null && !isCastingSkill)
            {
                if (Vector3.Distance(enemyController.targetObject.transform.position, transform.position) < buttstockAttackRange)
                {
                    ButtstockAttack();
                }
            }

            if (enemyController.enemyState == EnemyController.EnemyState.DetectingTarget && Time.time - lastSkillCastTime > skillCoolTime && !isCastingSkill)
            {
                skillCoolTime = Random.Range(6f, 7f);
                lastSkillCastTime = Time.time;

                if (bossInfo.currentBossPage == BossInfo.BossPage.Page1)
                {
                    StartCoroutine(GrenadeAttack());
                }
                else if (bossInfo.currentBossPage == BossInfo.BossPage.Page2)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        RocketLuncherAttack();
                    }
                    else
                    {
                        StartCoroutine(GrenadeAttack());
                    }
                }
            }

            if (!isCastingSkill)
            {
                animator.enabled = false;
                base.Update();
            }
            else
            {
                Targeting();
            }
        }

        private void ButtstockAttack()
        {
            enemyInfo.inventorySlotNumber = 2; // 개머리판 공격
            CastSkill();
            animator.SetInteger("Skill", 2);
            //enemyWeapon.WeaponAction();
            //yield return new WaitForSeconds(0.7f);
            //ResetSkill();
        }

        private IEnumerator GrenadeAttack()
        {
            enemyInfo.inventorySlotNumber = 3; // 수류탄
            CastSkill();
            //GameObject grenadeObject = enemyInfo.inventory[3].item.GetComponent<Item.Weapon.RocketLauncher>().rocket; // 디자인 생성 후 파괴
            //Destroy(Instantiate(grenadeObject, enemyInfo.inventory[3].item.transform), 2f);

            yield return new WaitForSeconds(1.5f);
            enemyWeapon.WeaponAction();
            yield return new WaitForSeconds(1f);
            ResetSkill();
        }

        private void RocketLuncherAttack()
        {
            enemyInfo.inventorySlotNumber = 1; // 로켓런쳐
            animator.SetInteger("Skill", 1);
            CastSkill();
            animator.Play("RocketLuncherEquipe");
            //yield return new WaitForSeconds(1f);
            //animator.enabled = false;
            //yield return new WaitForSeconds(targetingTime);
            //enemyWeapon.WeaponAction();
            //animator.enabled = true;
            //yield return new WaitForSeconds(0.1f);
            //animator.Play("RocketLunchDisarm");
            
            //yield return new WaitForSeconds(1f);
            //ResetSkill();
        }

        private void AnimationEnable(int whetherActive)
        {
            animator.enabled = whetherActive > 0 ? true : false;
        }

        private IEnumerator StartWeaponAction(float targetingTime)
        {
            yield return new WaitForSeconds(targetingTime);
            enemyWeapon.WeaponAction();
        }
        private IEnumerator StartRocketLuncherWeaponAction(float targetingTime)
        {
            yield return new WaitForSeconds(targetingTime);
            enemyWeapon.WeaponAction();
            animator.enabled = true;
            animator.Play("RocketLuncherDisarm");
        }

        private void CastSkill()
        {
            isCastingSkill = true;
            animator.enabled = true;
            enemyWeapon.startFire = true;
        }

        private void ResetSkill()
        {
            shotAngle = defaultShotAngle;
            shotDistance = defaultShotDistance;
            enemyInfo.inventorySlotNumber = 0;
            animator.SetInteger("Skill", 0);
            isCastingSkill = false;
        }
    }
}