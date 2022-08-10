using System.Collections.Generic;
using UnityEngine;

public class DetectTarget : MonoBehaviour
{
    [Header("-Detecting Area Setting")]
    [SerializeField]
    private float m_detectionAngle;
    [SerializeField]
    private float m_detectionDistance;
    [SerializeField]
    private LayerMask m_targetLayerMask;
    [SerializeField]
    private string targetTag;
    [SerializeField]
    private LayerMask m_ignoreLayerMask;
    public bool useDetectingArea;
    [HideInInspector]
    public GameObject detectingArea;

    private GameObject[] m_targetObjects;
    private GameObject m_detectedObject = null;
    
    private GameObject[] targetObjects
    {
        get
        {
            if (m_targetObjects == null)
            {
                m_targetObjects = GameObject.FindGameObjectsWithTag(targetTag);
            }
            return m_targetObjects;
        }
    }
    public float detectionAngle => m_detectionAngle;
    public float detectionDistance => m_detectionDistance;
    public GameObject detectedObject
    {
        get => m_detectedObject;
        set
        {
            if (m_detectedObject == null)
            {
                m_detectedObject = value;
            }
            else
            {
                if (value == null)
                {
                    m_detectedObject = null;
                }
            }
        }
    }
    public LayerMask targetLayerMask => m_targetLayerMask;
    public LayerMask ignoreLayerMask => m_ignoreLayerMask;

    public enum DetectingState
    {
        DetectingNothing,
        TargetInDetectingArea,
        DetectingTarget
    }
    

    private void OnDrawGizmos()
    {
        if (!useDetectingArea)
        {
            Gizmos.DrawRay(transform.position, EulerToVector(detectionAngle / 2) * detectionDistance);
            Gizmos.DrawRay(transform.position, EulerToVector(-detectionAngle / 2) * detectionDistance);
        }
    }

    public DetectingState SearchTarget()
    {
        float radianRange = Mathf.Cos(detectionAngle / 2 * Mathf.Deg2Rad);

        foreach (GameObject v in targetObjects)
        {
            float targetRadian = Vector3.Dot(transform.forward, (v.transform.position - transform.position).normalized);

            if (targetRadian < radianRange)
            {
                detectedObject = null;
                return DetectingState.DetectingNothing;
            }

            Debug.DrawRay(transform.position, (v.transform.position - transform.position).normalized * detectionDistance);

            if (Vector3.Distance(transform.position, v.transform.position) > detectionDistance)
            {
                detectedObject = null;
                return DetectingState.TargetInDetectingArea;
            }

            if (Physics.Raycast(transform.position, (v.transform.position - transform.position).normalized, out RaycastHit raycastHitCollider, detectionDistance, ~(ignoreLayerMask | Physics.IgnoreRaycastLayer)) &&
                ((int)Mathf.Pow(2, raycastHitCollider.transform.gameObject.layer) & targetLayerMask) == 0)
            {
                detectedObject = null;
                return DetectingState.TargetInDetectingArea;
            }

            Debug.Log("Detect Tagert!");
            detectedObject = v;
            return DetectingState.DetectingTarget;
        }

        return DetectingState.DetectingNothing;
    }

    private GameObject[] FindGameObjectsWithLayer(LayerMask target)
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new();
        for (var i = 0; i < goArray.Length; i++)
        {
            if ((int)Mathf.Pow(2, goArray[i].layer) == target)
            {
                goList.Add(goArray[i]);
            }
        }
        if (goList.Count == 0)
        {
            return null;
        }
        return goList.ToArray();
    }

    private Vector3 EulerToVector(float _degree)
    {
        _degree += transform.eulerAngles.y;
        _degree *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(_degree), 0, Mathf.Cos(_degree));
    }
}
