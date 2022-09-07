using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInfo : EntityInfo
{
    public BossPage currentBossPage = BossPage.Page1;


    protected override void Update()
    {
        base.Update();
        if (currentHp < (maxHp / 2f))
        {
            currentBossPage = BossPage.Page2;
        }
    }

    public enum BossPage
    {
        Page1,
        Page2
    }
}
