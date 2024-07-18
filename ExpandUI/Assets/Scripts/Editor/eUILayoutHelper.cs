using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects, CustomEditor(typeof(eUILayout))]
public class eUIPanelLayoutHelper : Editor
{
    #region Const Value
    private const float ICON_SIZE = 48.0f;
    private const float LABEL_WIDTH = 48.0f;
    private const float VALUE_WIDTH = 64.0f;
    #endregion

    private bool m_bPaddingFold = false;

    public override void OnInspectorGUI()
    {
        eUILayout layout = (eUILayout)target;
        if (layout == null) return;

        EditorGUILayout.Separator();

        #region Alignment Group
        GUILayout.Label("Alignments");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("LT", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 0.0f, 1.0f);
        }
        if (GUILayout.Button("CT", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 0.5f, 1.0f);
        }
        if (GUILayout.Button("RT", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 1.0f, 1.0f);
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("LM", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 0.0f, 0.5f);
        }
        if (GUILayout.Button("CM", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 0.5f, 0.5f);
        }
        if (GUILayout.Button("RM", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 1.0f, 0.5f);
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("LB", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 0.0f, 0.0f);
        }
        if (GUILayout.Button("CB", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 0.5f, 0.0f);
        }
        if (GUILayout.Button("RB", GUILayout.Width(ICON_SIZE)))
        {
            Alignment(layout, 1.0f, 0.0f);
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Center"))
        {
            Alignment(layout, 0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.5f);
            layout.AutoStretch();
        }
        if (GUILayout.Button("Stretch"))
        {
            layout.AutoStretch();
        }
        if (GUILayout.Button("Middle"))
        {
            Alignment(layout, 0.0f, 1.0f, 0.5f, 0.5f, 0.5f, 0.5f);
            layout.AutoStretch();
        }
        EditorGUILayout.EndHorizontal();
        #endregion

        #region Padding Group
        EditorGUI.BeginChangeCheck();
        m_bPaddingFold = EditorGUILayout.BeginFoldoutHeaderGroup(m_bPaddingFold, "[ Padding ]");
        if (m_bPaddingFold)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Top", GUILayout.Width(LABEL_WIDTH));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            layout.Top = EditorGUILayout.FloatField(layout.Top, GUILayout.Width(VALUE_WIDTH));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Left", GUILayout.Width(LABEL_WIDTH));
            layout.Left = EditorGUILayout.FloatField(layout.Left, GUILayout.Width(VALUE_WIDTH));
            EditorGUILayout.Space();
            layout.Right = EditorGUILayout.FloatField(layout.Right, GUILayout.Width(VALUE_WIDTH));
            EditorGUILayout.LabelField("Right", GUILayout.Width(LABEL_WIDTH));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            layout.Bottom = EditorGUILayout.FloatField(layout.Bottom, GUILayout.Width(VALUE_WIDTH));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Bottom", GUILayout.Width(LABEL_WIDTH));
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        if (EditorGUI.EndChangeCheck())
            layout.RefreshLayout();
        #endregion

        EditorGUILayout.Separator();

        #region Actual Size
        EditorGUI.BeginDisabledGroup(true);
        RectTransform rectTr = layout.GetComponent<RectTransform>();
        if (rectTr != null)
            EditorGUILayout.Vector2Field("Actual Size", new Vector2(rectTr.rect.width, rectTr.rect.height));
        EditorGUI.EndDisabledGroup();
        #endregion

        if (EditorApplication.isPlaying == false)
        {
            if (GUI.changed)
            {
                EditorUtility.SetDirty(layout);
                EditorSceneManager.MarkSceneDirty(layout.gameObject.scene);
            }
        }
    }

    private void Alignment(eUILayout helper, float x, float y)
    {
        helper?.Alignment(x, y);
        GUI.FocusControl(null);
    }

    private void Alignment(eUILayout helper, float minX, float maxX, float pivotX, float minY, float maxY, float pivotY)
    {
        helper?.Alignment(minX, maxX, pivotX, minY, maxY, pivotY);
        GUI.FocusControl(null);
    }
}
