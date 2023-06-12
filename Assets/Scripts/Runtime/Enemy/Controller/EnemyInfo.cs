using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : EntityInfo
{
    protected override void CalculateDamage()
    {
        if (currentHp <= 0)
        {
            DieEnemy();
        }

        base.CalculateDamage();
    }

    protected void DieEnemy()
    {
        //죽는 효과
        Destroy(gameObject);
    }
}
