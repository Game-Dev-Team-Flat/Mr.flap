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
    private int m_inventorySlotNumber;

    public KeyCode reloadKey => m_reloadKey;
    public KeyCode changeShotModeKey => m_changeShotModeKey;
    public Transform eyesOfObject => m_eyesOfObject;
    // 인벤토리의 슬롯 넘버를 제한
    public int inventorySlotNumber
    {
        get => m_inventorySlotNumber;
        set
        {
            if (value < 0)
            {
                m_inventorySlotNumber = inventory.Length - 1;
            }
            else if (value >= inventory.Length)
            {
                m_inventorySlotNumber = 0;
            }
            else
            {
                m_inventorySlotNumber = value;
            }
        }
    }

    private void Awake()
    {
        currentHp = maxHp;
    }

    private void Update()
    {
        CalculateDamage();
    }
}
