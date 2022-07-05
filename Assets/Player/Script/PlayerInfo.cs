using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : EntityInfo
{
    [SerializeField]
    private Transform m_eyesOfObject;
    [SerializeField]
    private KeyCode m_reloadKey;
    [SerializeField]
    private KeyCode m_changeShotModeKey;

    public KeyCode reloadKey => m_reloadKey;
    public KeyCode changeShotModeKey => m_changeShotModeKey;
    public Transform eyesOfObject => m_eyesOfObject;

    private void Update()
    {
        CalculateDamage();
    }
}
