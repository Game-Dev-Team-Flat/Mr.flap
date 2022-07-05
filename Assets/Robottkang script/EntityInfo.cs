using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public Inventory[] inventory;
    private int m_inventorySlotNumber;
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
    public float maxHp;
    [SerializeField]
    private float m_currentHp;
    public float currentHp { get => m_currentHp; set => m_currentHp = value < 0 ? 0 : value; }
    public float hpReduceRate;
    public float takenDamage;

    private void Awake()
    {
        currentHp = maxHp;
    }

    private void Update()
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
        public int count;
    }
}
