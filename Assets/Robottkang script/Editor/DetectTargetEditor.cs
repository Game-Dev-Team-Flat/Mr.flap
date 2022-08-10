using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DetectTarget))]
public class DetectTargetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DetectTarget detectTarget = (DetectTarget)target;

        if (detectTarget.useDetectingArea)
        {
            float seta = detectTarget.detectionAngle * Mathf.Deg2Rad / 2f;

            detectTarget.detectingArea = (GameObject)EditorGUILayout.ObjectField("Detecting Area", detectTarget.detectingArea, typeof(GameObject), true);

            detectTarget.detectingArea.transform.localScale =
                Vector3.up * detectTarget.detectionDistance * Mathf.Cos(seta) / detectTarget.transform.localScale.y
                + new Vector3(1f / detectTarget.transform.localScale.x, 0f, 1f / detectTarget.transform.localScale.z) * detectTarget.detectionDistance * Mathf.Sin(seta) * 2;
            detectTarget.detectingArea.transform.GetChild(1).localScale =
                new Vector3(1f, 1f, 0f) + Vector3.forward / detectTarget.detectingArea.transform.localScale.y * detectTarget.detectionDistance * 2 * (1 - Mathf.Cos(seta)) / detectTarget.transform.localScale.z;
        }
    }
}