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
                m_targetObjects = FindGameObjectsWithLayer(m_targetLayerMask);
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
    

    private void OnDrawGizmos()
    {
        if (!useDetectingArea)
        {
            Gizmos.DrawRay(transform.position, EulerToVector(detectionAngle / 2) * detectionDistance);
            Gizmos.DrawRay(transform.position, EulerToVector(-detectionAngle / 2) * detectionDistance);
        }
    }

    public void SearchTarget(LayerMask targetLayerMask)
    {
        float radianRange = Mathf.Cos(detectionAngle / 2 * Mathf.Deg2Rad);

        for (int i = 0; i < targetObjects.Length; i++)
        {
            float targetRadian = Vector3.Dot(transform.forward, (targetObjects[i].transform.position - transform.position).normalized);

            if (targetRadian < radianRange)
            {
                detectedObject = null;
                return;
            }

            Debug.DrawRay(transform.position, (targetObjects[i].transform.position - transform.position).normalized * detectionDistance);
            
            if (Physics.Raycast(transform.position, (targetObjects[i].transform.position - transform.position).normalized, out RaycastHit raycastHitCollider, detectionDistance))
            {
                if (((int)Mathf.Pow(2, raycastHitCollider.transform.gameObject.layer) & targetLayerMask) != 0)
                {
                    Debug.Log("Detect Tagert!");
                    detectedObject = targetObjects[i];
                }
                else
                {
                    detectedObject = null;
                }
            }
            else
            {
                detectedObject = null;
            }
        }
    }

    private GameObject[] FindGameObjectsWithLayer(LayerMask target)
    {
        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        List<GameObject> goList = new List<GameObject>();
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
