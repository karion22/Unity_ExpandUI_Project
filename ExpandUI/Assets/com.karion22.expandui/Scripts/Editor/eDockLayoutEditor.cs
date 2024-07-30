using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(eDockLayout))]
public class eDockLayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Apply"))
        {
            var script = (eDockLayout)target;
            script.UpdateLayout();
        }
    }
}
