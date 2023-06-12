using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Enemy;

public class EnemyValueCheckScript : MonoBehaviour
{
    [SerializeField]
    TextMeshPro state;       // 확인용
    [SerializeField]
    TextMeshPro healthPoint; // 확인용

    void Update()
    {
        state.text = GetComponent<EnemyController>().enemyState.ToString(); //확인용
        healthPoint.text = GetComponent<EntityInfo>().currentHp.ToString(); //확인용
    }
}
