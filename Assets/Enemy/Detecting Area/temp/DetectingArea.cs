using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingArea : MonoBehaviour
{
    [SerializeField]
    private DetectTarget m_detectTarget;
    private DetectTarget detectTarget
    {
        get
        {
            if (m_detectTarget == null)
            {
                m_detectTarget = transform.root.GetComponent<DetectTarget>();
            }
            return m_detectTarget;
        }
    }

    private Mesh myMesh;

    public float segments = 2;
    private float segmentAngle;

    private Vector3[] verts;
    private Vector3[] normals;
    private int[] triangles;
    private Vector2[] uvs;

    private float actualAngle;

    void LateUpdate()
    {
        myMesh = gameObject.GetComponent<MeshFilter>().mesh;

        buildMesh();
    }

    void buildMesh()
    {
        // Grab the Mesh off the gameObject
        //myMesh = gameObject.GetComponent<MeshFilter>().mesh;

        //Clear the mesh
        myMesh.Clear();

        // Calculate actual pythagorean angle
        actualAngle = 90.0f - detectTarget.detectionAngle / 2;

        // Segment Angle
        segmentAngle = detectTarget.detectionAngle / segments;

        // Initialise the array lengths
        verts = new Vector3[(int)segments * 3];
        normals = new Vector3[(int)segments * 3];
        triangles = new int[(int)segments * 3];
        uvs = new Vector2[(int)segments * 3];

        // Initialise the Array to origin Points
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = new Vector3(0, 0, 0);
            normals[i] = Vector3.up;
        }

        // Create a dummy angle
        float a = actualAngle;

        // Create the Vertices
        for (int i = 1; i < verts.Length; i += 3)
        {
            verts[i] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * detectTarget.detectionDistance, 0, Mathf.Sin(Mathf.Deg2Rad * a) * detectTarget.detectionDistance);

            a += segmentAngle;

            verts[i + 1] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * detectTarget.detectionDistance, 0, Mathf.Sin(Mathf.Deg2Rad * a) * detectTarget.detectionDistance);
        }

        // Create Triangle
        for (int i = 0; i < triangles.Length; i += 3)
        {
            triangles[i] = 0;
            triangles[i + 1] = i + 2;
            triangles[i + 2] = i + 1;
        }

        // Generate planar UV Coordinates
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(verts[i].x, verts[i].z);
        }

        // Put all these back on the mesh
        myMesh.vertices = verts;
        myMesh.normals = normals;
        myMesh.triangles = triangles;
        myMesh.uv = uvs;
    }
}
