using KRN.Utility;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class eUIBuilderEditor : EditorWindow
{
    #region Static 
    public static eUIBuilderEditor s_Wnd = null;
    private static readonly Vector2 windowSize = new Vector2(164f, 640f);

    [MenuItem("UI/Builder Window", priority = 1)]
    public static void ShowWindow()
    {
        if(s_Wnd == null)
        {
            s_Wnd = EditorWindow.GetWindow<eUIBuilderEditor>(true, "UI Builder");
            s_Wnd.minSize = s_Wnd.maxSize = windowSize;
        }
        s_Wnd.Show();
    }
    #endregion

    // "Default UI"
    [SerializeField] private GameObject m_CanvasPrefab = null;
    [SerializeField] private GameObject m_CameraPrefab = null;
    [SerializeField] private GameObject m_PanelPrefab = null;
    [SerializeField] private GameObject m_TextPrefab = null;
    [SerializeField] private GameObject m_ImagePrefab = null;
    [SerializeField] private GameObject m_RawImagePrefab = null;
    [SerializeField] private GameObject m_TogglePrefab = null;
    [SerializeField] private GameObject m_SliderPrefab = null;
    [SerializeField] private GameObject m_ScrollbarPrefab = null;
    [SerializeField] private GameObject m_ScrollViewPrefab = null;
    [SerializeField] private GameObject m_ButtonPrefab = null;
    [SerializeField] private GameObject m_DropdownPrefab = null;
    [SerializeField] private GameObject m_InputFieldPrefab = null;
    [SerializeField] private GameObject m_ProgressPrefab = null;

    // "Expand Layout"
    [SerializeField] private GameObject m_DockLayoutPrefab = null;

    // "Expand UI"
    [SerializeField] private GameObject m_LoopScrollPrefab = null;

    private Vector2 m_IconSize = new Vector2(48.0f, 48.0f);
    private Vector2 m_ExpandBtnSize = new Vector2(24.0f, 48.0f);
    private Vector2 m_SpacingSize = new Vector2(8.0f, 4.0f);

    private enum eStyleIndex
    {
        TextAlignmentMiddle = 0,
        HeaderLabel,

        MAX_GUI_STYLE
    }
    private bool m_Initialize = false;
    private GUIStyle[] m_Styles = new GUIStyle[(int)eStyleIndex.MAX_GUI_STYLE];

    private int m_SelectedIndex = 0;

    private Vector2 m_PrevSize = Vector2.zero;

    private bool m_ShowFoldOut = false;
    private Vector2 m_ScrollPos = Vector2.zero;

    private eCanvas m_Canvas = null;
    private eUICamera m_UICamera = null;

    private float m_ItemWidth = 0f;
    private const float LEFT_PADDING = 8.0f;

    private void OnGUI()
    {
        if (m_Initialize == false)
        {
            CreateGUIStyle();            
            m_Initialize = true;
        }

        if (m_PrevSize != position.size)
        {
            OnResize();
            m_PrevSize = position.size;
        }

        // »ó´Ü UI
        EditorGUILayout.BeginVertical();
        CreateHeaderGUI();

        EditorGUILayout.Space(8.0f, false);

        //int widthCnt = (int)(m_PrevSize.x / (m_IconSize.x + m_ExpandBtnSize. x + m_SpacingSize.x));
        //int heightCnt = (int)(m_PrevSize.y / (m_IconSize.y + m_ExpandBtnSize.y + m_SpacingSize.y));

        #region Default UI
        m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
        EditorGUILayout.LabelField("[ Default UI ]");
        
        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);

        // Canvas
        CreateIconButton("Canvas Icon", "Canvas", OnCanvasBtnClicked);

        EditorGUILayout.Space(m_SpacingSize.x, false);

        // Panel
        CreateIconButton("d_LayoutElement Icon", "Panel", m_PanelPrefab, OnBtnClicked<ePanel>);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(m_SpacingSize.y, false);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);
        // Image
        CreateIconButton("d_Image Icon", "Image", m_ImagePrefab, OnBtnClicked<eImage>);

        EditorGUILayout.Space(m_SpacingSize.x, false);

        // RawImage
        CreateIconButton("d_RawImage Icon", "RawImage", m_RawImagePrefab, OnBtnClicked<eRawImage>);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(m_SpacingSize.y, false);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);
        // Text
        CreateIconButton("d_Text Icon", "Text", m_TextPrefab, OnBtnClicked<eText>);

        EditorGUILayout.Space(m_SpacingSize.x, false);

        // Input Field
        CreateIconButton("d_InputField Icon", "InputField", m_InputFieldPrefab, OnBtnClicked<eInputField>);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(m_SpacingSize.y, false);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);
        // Slider
        CreateIconButton("d_Slider Icon", "Slider", m_SliderPrefab, OnBtnClicked<eSlider>);

        EditorGUILayout.Space(m_SpacingSize.x, false);

        // Progress
        CreateIconButton("sv_icon_name0", "Progress", m_ProgressPrefab, OnBtnClicked<eProgress>);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(m_SpacingSize.y, false);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);
        // Button
        CreateIconButton("d_Button Icon", "Button", m_ButtonPrefab, OnBtnClicked<eButton>);

        EditorGUILayout.Space(m_SpacingSize.x, false);

        // Toggle
        CreateIconButton("d_Toggle Icon", "Toggle", m_TogglePrefab, OnBtnClicked<eToggle>);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(m_SpacingSize.y, false);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);
        // Dropdown
        CreateIconButton("d_Dropdown Icon", "Dropdown", m_DropdownPrefab, OnBtnClicked<eDropdown>);
        EditorGUILayout.Space(m_SpacingSize.x, false);

        // Scrollbar
        CreateIconButton("d_Scrollbar Icon", "Scrollbar", m_ScrollbarPrefab, OnBtnClicked<eScrollbar>);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(m_SpacingSize.y, false);
        #endregion

        EditorGUILayout.Space(16.0f, false);

        #region Expand Layout 
        EditorGUILayout.LabelField("[ Expand Layout ]");
        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);
        CreateIconButton("d_TimelineAsset On Icon", "DockLayout", m_DockLayoutPrefab, OnBtnClicked<eDockLayout>, true);
        EditorGUILayout.EndHorizontal();
        #endregion

        #region Expand UI
        EditorGUILayout.LabelField("[ Expand UI ]");
        EditorGUILayout.BeginHorizontal(GUILayout.Width(m_ItemWidth));
        EditorGUILayout.Space(LEFT_PADDING, false);
        CreateButton("LoopScroll", "LoopScroll", m_LoopScrollPrefab, OnBtnClicked<eLoopScroll>, true);
        EditorGUILayout.EndHorizontal();
        #endregion

        EditorGUILayout.EndScrollView();

        EditorGUILayout.EndVertical();
    }

    private void CreateGUIStyle()
    {
        m_Styles[(int)eStyleIndex.TextAlignmentMiddle] = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, clipping = TextClipping.Overflow };
        m_Styles[(int)eStyleIndex.HeaderLabel] = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 16 };
    }

    private void CreateUICamera()
    {
        m_UICamera = FindFirstObjectByType(typeof(eUICamera)) as eUICamera;
        if (m_UICamera == null)
        {
            var newObject = (GameObject)PrefabUtility.InstantiatePrefab(m_CameraPrefab);
            if (newObject == null)
            {
                DebugLog.Assert("Camera Prefab instantiate failed");
                return;
            }

            m_UICamera = newObject.GetComponent<eUICamera>();
        }
    }

    private void OnResize()
    {
        m_ItemWidth = (m_IconSize.x * 2.0f) + LEFT_PADDING + m_SpacingSize.x;
    }

    private void CreateHeaderGUI()
    {
        m_ShowFoldOut = EditorGUILayout.Foldout(m_ShowFoldOut, "[ Icon Size ]");
        if(m_ShowFoldOut)
        {
            m_IconSize = EditorGUILayout.Vector2Field("Icon Size", m_IconSize);
            m_ExpandBtnSize = EditorGUILayout.Vector2Field("Expand Icon Size", m_ExpandBtnSize);
            m_SpacingSize = EditorGUILayout.Vector2Field("Spacing Size", m_SpacingSize);
        }
    }

    private void CreateButton(string inButtonName, string inDescription, GameObject inPrefab, UnityAction<GameObject, bool> onClicked, bool searchByChild = false, UnityAction onExpandBtnClicked = null)
    {
        CreateButton(inButtonName, inDescription, inPrefab, onClicked, searchByChild, onExpandBtnClicked, m_IconSize.x, m_IconSize.y, m_ExpandBtnSize.x, m_ExpandBtnSize.y); 
    }

    private void CreateButton(string inButtonName, string inDescription, GameObject inPrefab, UnityAction<GameObject, bool> onClicked, bool searchByChild, UnityAction onExpandBtnClicked, float inWidth, float inHeight, float inExpandWidth, float inExpandHeight)
    {
        float realWidth = inWidth;
        if (onExpandBtnClicked != null)
            realWidth += inExpandWidth;

        EditorGUILayout.BeginVertical(GUILayout.Width(realWidth));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(inButtonName, GUILayout.Width(inWidth), GUILayout.Height(inHeight)))
            onClicked?.Invoke(inPrefab, searchByChild);

        if (onExpandBtnClicked != null)
            CreateExpandIconButton(onExpandBtnClicked, m_ExpandBtnSize.x, m_ExpandBtnSize.y);
        EditorGUILayout.EndHorizontal();

        if (string.IsNullOrEmpty(inDescription) == false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(inDescription, m_Styles[(int)eStyleIndex.TextAlignmentMiddle], GUILayout.Width(inWidth));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private void CreateExpandButton(UnityAction onClicked)
    {

    }
    
    private void CreateIconButton(string inIconName, string inDescription, UnityAction onClicked, UnityAction onExpandBtnClicked = null)
    {
        CreateIconButton_Impl(inIconName, inDescription, onClicked, onExpandBtnClicked, m_IconSize.x, m_IconSize.y, m_ExpandBtnSize.x, m_ExpandBtnSize.y);
    }    

    private void CreateIconButton(string inIconName, string inDescription, GameObject inPrefab, UnityAction<GameObject, bool> onClicked, bool searchByChild = false, UnityAction onExpandBtnClicked = null)
    {
        CreateIconButton_Impl(inIconName, inDescription, inPrefab, onClicked, searchByChild, onExpandBtnClicked, m_IconSize.x, m_IconSize.y, m_ExpandBtnSize.x, m_ExpandBtnSize.y);
    }

    private void CreateIconButton_Impl(string inIconName, string inDescription, UnityAction onClicked, UnityAction onExpandBtnClicked, float inWidth, float inHeight, float inExpandWidth, float inExpandHeight)
    {
        float realWidth = inWidth;
        if (onExpandBtnClicked != null)
            realWidth += inExpandWidth;

        EditorGUILayout.BeginVertical(GUILayout.Width(realWidth));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent(inIconName), GUILayout.Width(inWidth), GUILayout.Height(inHeight)))
            onClicked?.Invoke();

        if (onExpandBtnClicked != null)
            CreateExpandIconButton(onExpandBtnClicked, m_ExpandBtnSize.x, m_ExpandBtnSize.y);

        EditorGUILayout.EndHorizontal();

        if (string.IsNullOrEmpty(inDescription) == false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(inDescription, m_Styles[(int)eStyleIndex.TextAlignmentMiddle], GUILayout.Width(inWidth));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }    

    private void CreateIconButton_Impl(string inIconName, string inDescription, GameObject inPrefab, UnityAction<GameObject, bool> onClicked, bool searchByChild, UnityAction onExpandBtnClicked, float inWidth, float inHeight, float inExpandWidth, float inExpandHeight)
    {
        float realWidth = inWidth;
        if (onExpandBtnClicked != null)
            realWidth += inExpandWidth;

        EditorGUILayout.BeginVertical(GUILayout.Width(realWidth));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(EditorGUIUtility.IconContent(inIconName), GUILayout.Width(inWidth), GUILayout.Height(inHeight)))
            onClicked?.Invoke(inPrefab, searchByChild);

        if (onExpandBtnClicked != null)
            CreateExpandIconButton(onExpandBtnClicked, m_ExpandBtnSize.x, m_ExpandBtnSize.y);

        EditorGUILayout.EndHorizontal();

        if (string.IsNullOrEmpty(inDescription) == false)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(inDescription, m_Styles[(int)eStyleIndex.TextAlignmentMiddle], GUILayout.Width(inWidth));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }

    private void CreateExpandIconButton(UnityAction onClicked, float inWidth, float inHeight)
    {
        if (GUILayout.Button(EditorGUIUtility.IconContent("d_scrollright_uielements"), GUILayout.Width(inWidth), GUILayout.Height(inHeight)))
            onClicked?.Invoke();
    }

    private void OnCanvasBtnClicked()
    {
        var parentTr = (Selection.activeGameObject == null) ? null : Selection.activeTransform;

        var newPrefab = (GameObject)PrefabUtility.InstantiatePrefab(m_CanvasPrefab);
        if(newPrefab == null)
        {
            DebugLog.Assert("Instantiate prefab failed");
            return;
        }

        var newElement = newPrefab.GetComponent<eCanvas>() ?? newPrefab.AddComponent<eCanvas>();
        if(newElement == null)
        {
            DebugLog.Assert("Canvas add component failed");
            return;
        }

        newElement.OnCreated(m_SelectedIndex, parentTr);

        if (m_UICamera == null)
            CreateUICamera();

        
        //newElement.SetCamera(m_UICamera.Camera);

        Selection.activeGameObject = newPrefab;
    }

    private void OnCanvasExpandBtnClicked()
    {

    }

    private void OnBtnClicked<T>(GameObject inPrefab, bool bSearchChild) where T : eElement
    {
        var parentTr = IsValidCanvas();

        var newPrefab = (GameObject)PrefabUtility.InstantiatePrefab(inPrefab);
        if(newPrefab == null)
        {
            DebugLog.Assert("Instantiate prefab is failed");
            return;
        }

        T newElement = null;
        if(bSearchChild)
        {
            newElement = newPrefab.GetComponentInChildren(typeof(T)) as T;
            
            if(newElement == null)
            {
                DebugLog.Assert(Utility.BuildString("Child not have {0}", typeof(T).ToString()));
                return;
            }
        }
        else
            newElement = newPrefab.GetComponent(typeof(T)) as T ?? newPrefab.AddComponent(typeof(T)) as T;
        if(newElement == null)
        {
            DebugLog.Assert(Utility.BuildString("{0} add component is failed", typeof(T).ToString()));
            return;
        }

        newElement.OnCreated(m_SelectedIndex, parentTr);
        Selection.activeGameObject = newPrefab;
    }

    private Transform IsValidCanvas()
    {
        if (Selection.activeGameObject == null)
        {
            var canvas = FindFirstObjectByType(typeof(eCanvas));
            if (canvas == null)
                OnCanvasBtnClicked();
            else
                Selection.activeTransform = ((eCanvas)canvas).transform;
            return Selection.activeTransform;
        }
        else
        {
            var target = Selection.activeGameObject.GetComponent<eElement>();
            if (target == null)
            {
                var canvas = (eCanvas)FindFirstObjectByType(typeof(eCanvas));
                if (canvas == null)
                {
                    OnCanvasBtnClicked();
                    return Selection.activeTransform;
                }
                else
                {
                    return canvas.transform;
                }
            }
            else
            {
                return target.transform;
            }
        }
    }

    private void Update()
    {
        
    }
}
