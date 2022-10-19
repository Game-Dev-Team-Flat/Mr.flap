using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : EntityInfo
{
    public bool isHit = false;
    [SerializeField]
    private Transform m_eyesOfObject;
    [SerializeField]
    private KeyCode m_reloadKey;
    [SerializeField]
    private KeyCode m_changeShotModeKey;

    public KeyCode reloadKey => m_reloadKey;
    public KeyCode changeShotModeKey => m_changeShotModeKey;
    public Transform eyesOfObject => m_eyesOfObject;

    protected override void CalculateDamage()
    {
        if (takenDamage > 0)
        {
            isHit = true;
        }

        base.CalculateDamage();
    }
}
