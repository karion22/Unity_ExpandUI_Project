using KRN.Utility;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(eUIDebugger))]
public class eUIDebuggerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Info"))
        {
            var go = ((eUIDebugger)target).gameObject;
            if (go == null)
                return;
            var rt = go.GetComponent<RectTransform>();

            if (rt != null)
            {
                Debug.Log(Utility.BuildString("Anchored Position : {0} / {1}", rt.anchoredPosition.x, rt.anchoredPosition.y));
                Debug.Log(Utility.BuildString("Anchored Min : {0} / {1}", rt.anchorMin.x, rt.anchorMin.y));
                Debug.Log(Utility.BuildString("Anchored Max : {0} / {1}", rt.anchorMax.x, rt.anchorMax.y));
                Debug.Log(Utility.BuildString("Pivot : {0} / {1}", rt.pivot.x, rt.pivot.y));

                Debug.Log(Utility.BuildString("Width : {0} / Height : {1}", rt.sizeDelta.x, rt.sizeDelta.y));
            }
        }
    }
}
