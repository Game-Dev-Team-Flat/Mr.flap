using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    [SerializeField]
    private Inventory[] m_inventory;
    private int m_inventorySlotNumber = 0;
    [SerializeField]
    protected float maxHp;
    [SerializeField]
    private float m_currentHp;
    public float hpReduceRate;
    public float takenDamage;
    public Effect effect;

    public Inventory[] inventory => m_inventory;
    public int inventorySlotNumber // 인벤토리의 슬롯 넘버를 제한
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
    public float currentHp { get => m_currentHp; set => m_currentHp = value < 0 ? 0 : value; }
    
    public struct Effect
    {
        public float stun;
    }

    protected virtual void Update()
    {
        CalculateDamage();
    }

    protected void CalculateDamage()
    {
        if (takenDamage > 0)
        {
            currentHp -= Time.deltaTime * hpReduceRate;
            takenDamage -= Time.deltaTime * hpReduceRate;
        }
    }

    [System.Serializable]
    public struct Inventory
    {
        public GameObject item;
    }
}
