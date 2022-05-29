using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : EntityInfo
{
    private void Awake()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        CalculateDamage();
    }
}
