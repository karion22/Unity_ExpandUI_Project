using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasScaler))]
public class eCanvas : eElement
{
    #region Enum
    public enum eSortLayer
    {
        MostDown = 1,
        Down = 5,
        Default = 10,
        Top = 15,
        MostTop = 20,
        Popup = 100,
        System = 200,
        MostOfAll = 500,
    }

    public enum ePreset
    {
        MatchByHeight,
        Expand
    }

    public enum eCanvasType
    {
        Default,
        Alone,
        Unique,
    }
    #endregion

    [SerializeField] private bool m_IsRoot = false;

    [SerializeField] private bool m_IgnoreSort = false;
    public bool IsIgnoreSort { get { return m_IgnoreSort; }}

    [SerializeField] private eCanvasType m_CanvasType = eCanvasType.Default;

    [SerializeField] private eSortLayer m_SortLayer = eSortLayer.Default;
    public eSortLayer SortLayer { get { return m_SortLayer; } }

    #region Property
    private Canvas m_Canvas;
    public Canvas Canvas
    {
        get
        {
            if (object.ReferenceEquals(m_Canvas, null))
                m_Canvas = GetComponent<Canvas>();
            return m_Canvas;
        }
    }
    #endregion

    protected override void Start()
    {
        base.Start();
        
        if (Canvas.isRootCanvas)
        {
            UIMgr.Instance.AddCanvas(this);

            if (Canvas.worldCamera == null)
                Canvas.worldCamera = UIMgr.Instance.Camera;
            Canvas.sortingLayerName = "UI";
        }

        if (m_CanvasType == eCanvasType.Alone)
            UIMgr.Instance.AloneCanvas(this);
    }

    protected override void OnDestroy()
    {
        if (UIMgr.IsInstantiate)
            UIMgr.Instance.RemoveCanvas(this);
        base.OnDestroy();
    }

    #region Unity Editor
    public override void OnCreated(int inElement, Transform inParent)
    {
        //base.OnCreated(inElement, inParent);

        if (m_IsRoot)
            transform.SetParent(null);
        else
            transform.SetParent(inParent);

        var canvasScaler = Canvas.GetComponent<CanvasScaler>() ?? Canvas.AddComponent<CanvasScaler>();
        switch ((ePreset)inElement)
        {
            case ePreset.MatchByHeight:
                {
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
                    //canvasScaler.matchWidthOrHeight = 1.0f;
                }
                break;

            case ePreset.Expand:
                {
                    canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
                    //canvasScaler.matchWidthOrHeight = 1.0f;
                }
                break;
        }

        Canvas.sortingLayerName = "UI";
    }

    public void SetCamera(Camera inCamera)
    {
        Canvas.worldCamera = inCamera;
    }
    #endregion
}