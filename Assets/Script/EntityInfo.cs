using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public Inventory[] inventory;
    public float maxHp;
    public float m_currentHp;
    public float currentHp { get => m_currentHp; set => m_currentHp = value < 0 ? 0 : value; }
    public float hpReduceRate;
    private float m_takenDamage;
    public float takenDamage { get => m_takenDamage; set => m_takenDamage = value; }

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
