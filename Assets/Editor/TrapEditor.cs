using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TrapController))]
[CanEditMultipleObjects]
class TrapEditor : Editor
{
    TrapController trapController = null;
    private void OnEnable()
    {
        trapController = target as TrapController;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("현재 위치를 함정의 이동 위치로"))
            trapController.Move_To = trapController.transform.position;
        GUILayout.Label("This is First Trap-Control");

    }
}