using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GUIBuildScript))]
public class GUIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();

        GUIBuildScript myScript = (GUIBuildScript)target;
        if(GUILayout.Button("Width resizing"))
        {
            myScript.build();
        }
    }
}
