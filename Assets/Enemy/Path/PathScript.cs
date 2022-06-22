using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    private Transform path;

    private void OnDrawGizmos()
    {
        path = GetComponent<Transform>();

        for (int i = 0; i < path.childCount - 1; i++)
        {
            Gizmos.DrawLine(path.GetChild(i).position, path.GetChild(i + 1).position);
        }
        Gizmos.DrawLine(path.GetChild(0).position, path.GetChild(path.childCount - 1).position);
    }
}
