using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfVeiw : MonoBehaviour
{
    [SerializeField]
    private DetectTarget m_detectTarget;
    [SerializeField]
    private Light m_flashLight;
    private Light flashLight
    {
        get
        {
            if (m_flashLight == null)
            {
                m_flashLight = GetComponent<Light>();
            }
            return m_flashLight;
        }
    }
    private DetectTarget detectTarget
    {
        get
        {
            if (m_detectTarget == null)
            {
                m_detectTarget = GetComponent<DetectTarget>();
            }
            return m_detectTarget;
        }
    }

    private void Update()
    {
        flashLight.spotAngle = detectTarget.detectionAngle;
        flashLight.range = detectTarget.detectionDistance;
    }
}
