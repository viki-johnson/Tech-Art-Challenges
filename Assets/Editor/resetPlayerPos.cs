using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Movement))]
public class resetPlayerPos : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("go back up")) {
            Movement m = (Movement)target;
            m.ResetPos();
        }
    }
}
