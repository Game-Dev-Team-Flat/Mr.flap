using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_airInjector;
    public ParticleSystem airInjector => m_airInjector;

    public void InjectAir(Quaternion InjectingDirection)
    {
        airInjector.transform.rotation = InjectingDirection;
        airInjector.Play();
    }
}
