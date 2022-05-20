using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectTarget : MonoBehaviour
{
    [SerializeField]
    private float DetectionAngle;
    [SerializeField]
    private float DetectionDistance;
    [SerializeField]
    private LayerMask targetLayerMask;


    private void Update()
    {
        SearchTarget(targetLayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, EulerToVector( DetectionAngle / 2) * DetectionDistance);
        Gizmos.DrawRay(transform.position, EulerToVector(-DetectionAngle / 2) * DetectionDistance);
    }

    private void SearchTarget(LayerMask _targetLayerMask)
    {
        Collider[] objectsInOverlapSphere = Physics.OverlapSphere(transform.position, DetectionDistance, _targetLayerMask);

        float radianRange = Mathf.Cos(DetectionAngle / 2 * Mathf.Deg2Rad);

        for (int i = 0; i < objectsInOverlapSphere.Length; i++)
        {
            float targetRadian = Vector3.Dot(transform.forward, (objectsInOverlapSphere[i].transform.position - transform.position).normalized);

            if (targetRadian > radianRange)
            {
                Debug.DrawRay(transform.position, (objectsInOverlapSphere[i].transform.position - transform.position).normalized * DetectionDistance);
                if(Physics.Raycast(transform.position, (objectsInOverlapSphere[i].transform.position - transform.position).normalized, out RaycastHit colliderHit, DetectionDistance))
                {
                    if (colliderHit.transform.gameObject.layer == 7)
                    {
                        Debug.Log("Detect Player!");
                    }
                }
            }
        }
    }

    private Vector3 EulerToVector(float _degree)
    {
        _degree += transform.eulerAngles.y;
        _degree *= Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(_degree), 0, Mathf.Cos(_degree));
    }
}
