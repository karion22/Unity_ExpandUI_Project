using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(eDockPanel))]
public class eDockPanelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Apply"))
        {
            var script = (eDockPanel)target;
            script.UpdateLayout();
        }
    }
}
