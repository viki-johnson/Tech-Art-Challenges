using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LandscapeMaker))]
public class testeditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if(GUILayout.Button("make a landscape")) {
            LandscapeMaker lsm = (LandscapeMaker)target;
            lsm.MakeLandscape();
        }
    }
}
