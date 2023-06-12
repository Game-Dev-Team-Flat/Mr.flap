using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Enemy;

public class BossInfo : EnemyInfo
{
    public BossPage currentBossPage = BossPage.Page1;
    [SerializeField]
    private Slider bossHpBar;
    private EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponent<EnemyController>();
    }

    protected override void Update()
    {
        base.Update();

        if(enemyController.enemyState != EnemyController.EnemyState.DetectingNothing)
        {
            bossHpBar.gameObject.SetActive(true);
        }

        if (currentHp < (maxHp / 2f))
        {
            currentBossPage = BossPage.Page2;
        }
        else
        {
            currentBossPage = BossPage.Page1;
        }

        if (bossHpBar != null)
        {
            bossHpBar.value = currentHp / maxHp;
        }
    }

    public enum BossPage
    {
        Page1,
        Page2
    }
}
