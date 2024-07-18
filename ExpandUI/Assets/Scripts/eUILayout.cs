using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[ExecuteInEditMode]
public class eUILayout : eElement
{
    public enum eHorizontalAlignment
    {
        None,
        Left,
        Center,
        Right,
        Stretch
    }

    public enum eVerticalAlignment
    {
        None,
        Top,
        Middle,
        Bottom,
        Stretch
    }

    [SerializeField] private float m_Left = 0f;
    [SerializeField] private float m_Right = 0f;
    [SerializeField] private float m_Top = 0f;
    [SerializeField] private float m_Bottom = 0f;

    public float Left { get { return m_Left; } set { m_Left = value; } }
    public float Right { get { return m_Right; } set { m_Right = value; } }
    public float Top { get { return m_Top; } set { m_Top = value; } }
    public float Bottom { get { return m_Bottom; } set { m_Bottom = value; } }

    public bool KeepSize { get; set; }

    private eDockLayout m_DockLayout;
    public eDockLayout DockLayout
    {
        get
        {
            if (m_DockLayout == null)
                m_DockLayout = transform.parent.GetComponent<eDockLayout>();
            return m_DockLayout;
        }
    }

    public eUILayout(float inLeft, float inRight, float inTop, float inBottom) { Set(inLeft, inRight, inTop, inBottom); }

    protected override void Start()
    {
        base.Start();
        KeepSize = true;
    }

    public void UpdateLayout(bool immediately = true)
    {
        if (DockLayout != null)
            DockLayout.UpdateLayout(immediately);
    }

    public void RefreshLayout()
    {
        if (IsStreched())
        {
            Alignment(RectTransform.pivot.x, RectTransform.pivot.y);
            AutoStretch();
        }
        else
            Alignment(RectTransform.pivot.x, RectTransform.pivot.y);
    }

    public void Set(float inLeft, float inRight, float inTop, float inBottom)
    {
        Left = inLeft;
        Right = inRight;
        Top = inTop;
        Bottom = inBottom;

        RefreshLayout();
    }

    public void Alignment(float x, float y)
    {
        Alignment_Impl(x, x, x, y, y, y);
    }

    public void Alignment(float minX, float maxX, float pivotX, float minY, float maxY, float pivotY)
    {
        Alignment_Impl(minX, maxX, pivotX, minY, maxY, pivotY);
    }

    private void Alignment_Impl(float minX, float maxX, float pivotX, float minY, float maxY, float pivotY)
    {
        if (RectTransform != null)
        {
            if (KeepSize == true)
                RectTransform.sizeDelta = new Vector2(RectTransform.rect.width, RectTransform.rect.height);

            RectTransform.anchorMin = new Vector2(minX, minY);
            RectTransform.anchorMax = new Vector2(maxX, maxY);
            RectTransform.pivot = new Vector2(pivotX, pivotY);

            eHorizontalAlignment hA = CheckHorizontalAlignment(RectTransform);
            eVerticalAlignment vA = CheckVerticalAlignment(RectTransform);

            switch (hA)
            {
                case eHorizontalAlignment.None:
                    break;

                case eHorizontalAlignment.Left:
                    RectTransform.anchoredPosition = new Vector2(m_Left, RectTransform.anchoredPosition.y);
                    break;

                case eHorizontalAlignment.Center:
                    RectTransform.anchoredPosition = new Vector2((m_Left - m_Right) * 0.5f, RectTransform.anchoredPosition.y);
                    break;

                case eHorizontalAlignment.Right:
                    RectTransform.anchoredPosition = new Vector2(-m_Right, RectTransform.anchoredPosition.y);
                    break;

                case eHorizontalAlignment.Stretch:
                    RectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
                    break;
            }

            switch (vA)
            {
                case eVerticalAlignment.None:
                    break;

                case eVerticalAlignment.Top:
                    RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, -m_Top);
                    break;

                case eVerticalAlignment.Middle:
                    RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, (m_Bottom - m_Top) * 0.5f);
                    break;

                case eVerticalAlignment.Bottom:
                    RectTransform.anchoredPosition = new Vector2(RectTransform.anchoredPosition.x, m_Bottom);
                    break;

                case eVerticalAlignment.Stretch:
                    RectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
                    break;
            }
            UpdateLayout();
        }
    }

    public void AutoStretch(bool immediatly = true)
    {
        if (RectTransform != null)
        {
            eHorizontalAlignment hA = CheckHorizontalAlignment(RectTransform);
            eVerticalAlignment vA = CheckVerticalAlignment(RectTransform);

            switch (hA)
            {
                case eHorizontalAlignment.Left:
                    {
                        RectTransform.offsetMin = new Vector2(RectTransform.offsetMin.x, m_Bottom);
                        RectTransform.offsetMax = new Vector2(RectTransform.offsetMax.x, -m_Top);

                        RectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                        RectTransform.anchorMax = new Vector2(0.0f, 1.0f);
                    }
                    break;

                case eHorizontalAlignment.Right:
                    {
                        RectTransform.offsetMin = new Vector2(RectTransform.offsetMin.x, m_Bottom);
                        RectTransform.offsetMax = new Vector2(RectTransform.offsetMax.x, -m_Top);

                        RectTransform.anchorMin = new Vector2(1.0f, 0.0f);
                        RectTransform.anchorMax = new Vector2(1.0f, 1.0f);
                    }
                    break;
            }

            switch (vA)
            {
                case eVerticalAlignment.Top:
                    {
                        RectTransform.offsetMin = new Vector2(m_Left, RectTransform.offsetMin.y);
                        RectTransform.offsetMax = new Vector2(-m_Right, RectTransform.offsetMax.y);

                        RectTransform.anchorMin = new Vector2(0.0f, 1.0f);
                        RectTransform.anchorMax = new Vector2(1.0f, 1.0f);
                    }
                    break;

                case eVerticalAlignment.Bottom:
                    {
                        RectTransform.offsetMin = new Vector2(m_Left, RectTransform.offsetMin.y);
                        RectTransform.offsetMax = new Vector2(-m_Right, RectTransform.offsetMax.y);

                        RectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                        RectTransform.anchorMax = new Vector2(1.0f, 0.0f);
                    }
                    break;
            }

            if (hA == eHorizontalAlignment.Center && vA == eVerticalAlignment.Middle)
            {
                RectTransform.offsetMin = new Vector2(m_Left, m_Bottom);
                RectTransform.offsetMax = new Vector2(-m_Right, -m_Top);

                RectTransform.anchorMin = new Vector2(0.0f, 0.0f);
                RectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            }
            else if (hA == eHorizontalAlignment.Center && vA == eVerticalAlignment.Stretch)
            {
                RectTransform.offsetMin = new Vector2(RectTransform.offsetMin.x, m_Bottom);
                RectTransform.offsetMax = new Vector2(RectTransform.offsetMax.x, -m_Top);

                RectTransform.anchorMin = new Vector2(0.5f, 0.0f);
                RectTransform.anchorMax = new Vector2(0.5f, 1.0f);
            }
            else if (hA == eHorizontalAlignment.Stretch && vA == eVerticalAlignment.Middle)
            {
                RectTransform.offsetMin = new Vector2(m_Left, RectTransform.offsetMin.y);
                RectTransform.offsetMax = new Vector2(-m_Right, RectTransform.offsetMax.y);

                RectTransform.anchorMin = new Vector2(0.0f, 0.5f);
                RectTransform.anchorMax = new Vector2(1.0f, 0.5f);
            }

            UpdateLayout(immediatly);
        }
    }

    public bool IsStreched()
    {
        eHorizontalAlignment ha = CheckHorizontalAlignment(RectTransform);
        eVerticalAlignment va = CheckVerticalAlignment(RectTransform);

        return ha == eHorizontalAlignment.Stretch || va == eVerticalAlignment.Stretch ? true : false;
    }

    public static eHorizontalAlignment CheckHorizontalAlignment(RectTransform t)
    {
        eHorizontalAlignment va = eHorizontalAlignment.None;

        if (t.anchorMin.x == 0f && t.anchorMax.x == 0f)
            va = eHorizontalAlignment.Left;
        else if (t.anchorMin.x == 0.5f && t.anchorMax.x == 0.5f)
            va = eHorizontalAlignment.Center;
        else if (t.anchorMin.x == 1f && t.anchorMax.x == 1f)
            va = eHorizontalAlignment.Right;
        else if (t.anchorMin.x == 0f && t.anchorMax.x == 1f)
            va = eHorizontalAlignment.Stretch;

        return va;
    }

    public static eVerticalAlignment CheckVerticalAlignment(RectTransform t)
    {
        eVerticalAlignment va = eVerticalAlignment.None;

        if (t.anchorMin.y == 0f && t.anchorMax.y == 0f)
            va = eVerticalAlignment.Bottom;
        else if (t.anchorMin.y == 0.5f && t.anchorMax.y == 0.5f)
            va = eVerticalAlignment.Middle;
        else if (t.anchorMin.y == 1f && t.anchorMax.y == 1f)
            va = eVerticalAlignment.Top;
        else if (t.anchorMin.y == 0f && t.anchorMax.y == 1f)
            va = eVerticalAlignment.Stretch;

        return va;
    }
}
