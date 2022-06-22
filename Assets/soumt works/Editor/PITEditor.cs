using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerInTrigger))]
[CanEditMultipleObjects]
public class PITEditor : Editor
{
    PlayerInTrigger inTrigger = null;
    // Start is called before the first frame update
    private void OnEnable()
    {
        inTrigger = target as PlayerInTrigger;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();   
        if(GUILayout.Button("트리거 테스트"))
        {
            if (EditorApplication.isPlaying)
                foreach(var Target in inTrigger.Targets)
                    Target.OnReceivedTrigger();
            else
                EditorApplication.EnterPlaymode();


        }
    }

    void OnSceneGUI()
    {
        if (inTrigger.Targets == null)
            return;
        Handles.color = Color.cyan;
        Vector3 center = inTrigger.transform.position;
        for (int i = 0; i < inTrigger.Targets.Length; i++)
        {
            GameObject connectedObject = inTrigger.Targets[i].gameObject;
            if (connectedObject)
            {
                Handles.DrawLine(center, connectedObject.transform.position, 3f);
            }
            else
            {
                Handles.DrawLine(center, Vector3.zero);
            }
        }
    }
}
