using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject m_eyesOfObject;
    public Inventory[] inventory;
    public float maxHp;
    [SerializeField]
    private float m_currentHp;
    public float currentHp { get => m_currentHp; set => m_currentHp = value < 0 ? 0 : value; }
    public float hpReduceRate;
    public float takenDamage;
    public GameObject eyesOfObject => m_eyesOfObject;

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
