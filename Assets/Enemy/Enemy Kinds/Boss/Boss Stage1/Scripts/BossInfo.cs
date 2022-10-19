using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossInfo : EnemyInfo
{
    public BossPage currentBossPage = BossPage.Page1;
    [SerializeField]
    private Slider bossHpBar;

    protected override void Update()
    {
        base.Update();
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
