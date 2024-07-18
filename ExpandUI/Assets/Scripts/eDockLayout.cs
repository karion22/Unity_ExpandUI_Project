using KRN.Utility;
using UnityEngine;

[ExecuteInEditMode]
public class eDockLayout : eElement
{
    // Collapse : Active가 아닐 때 공간 무시
    // Hidden : Active가 아닐 때 해당 공간은 비워놓기
    public enum Visibility { Collapse, Hidden }

    public Visibility m_Visible = Visibility.Collapse;

    private bool m_LateUpdate = false;

    private void LateUpdate()
    {
        if(m_LateUpdate)
        {
            UpdateLayout_Impl();
            m_LateUpdate = false;
        }
    }

    public void UpdateLayout(bool immediately = true)
    {
        if (immediately)
            m_LateUpdate = true;
        else
            UpdateLayout_Impl();
    }

    private void UpdateLayout_Impl()
    {
        // 자식 객체들을 읽어온다.
        RectTransform child = null;
        eUILayout layout = null;
        eUILayout.eHorizontalAlignment ha = eUILayout.eHorizontalAlignment.None;
        eUILayout.eVerticalAlignment va = eUILayout.eVerticalAlignment.None;

        float width = RectTransform.rect.width;
        float height = RectTransform.rect.height;

        DebugLog.Print(Utility.BuildString("Process UpdateLayout : {0}", name));

        // 자식 객체들의 정렬 상태를 읽어온다.
        float top = 0f, bottom = 0f, left = 0f, right = 0f;

        for (int i = 0, end = transform.childCount; i < end; i++)
        {
            child = transform.GetChild(i) as RectTransform;
            if (child == null) continue;
            if (child.gameObject.activeSelf == false && m_Visible == Visibility.Collapse) continue;

            layout = child.GetComponent<eUILayout>();

            if(layout != null)
            {
                ha = eUILayout.CheckHorizontalAlignment(child);
                va = eUILayout.CheckVerticalAlignment(child);

                switch(ha)
                {
                    case eUILayout.eHorizontalAlignment.Left:
                        {
                            child.anchoredPosition = new Vector2(left + layout.Left, child.anchoredPosition.y);
                            left += (child.rect.width + (layout.Left + layout.Right));
                        }
                        break;

                    case eUILayout.eHorizontalAlignment.Center:
                        {
                            child.anchoredPosition = new Vector2((left + (((width - (left + right)) - child.rect.width) * 0.5f)) - ((width * 0.5f) - (child.rect.width * 0.5f) - ((layout.Left - layout.Right) * 0.5f)), child.anchoredPosition.y);
                        }
                        break;

                    case eUILayout.eHorizontalAlignment.Right:
                        {
                            child.anchoredPosition = new Vector2(-(right + layout.Right), child.anchoredPosition.y);
                            right += (child.rect.width + (layout.Left + layout.Right));
                        }
                        break;

                    case eUILayout.eHorizontalAlignment.Stretch:
                        {
                            child.offsetMin = new Vector2(left + layout.Left, child.offsetMin.y);
                            child.offsetMax = new Vector2(-(right + layout.Right), child.offsetMax.y);
                        }
                        break;
                }
            }

            switch(va)
            {
                case eUILayout.eVerticalAlignment.Top:
                    {
                        child.anchoredPosition = new Vector2(child.anchoredPosition.x, -(top + layout.Top));
                        top += (child.rect.height + (layout.Top + layout.Bottom));
                    }
                    break;

                case eUILayout.eVerticalAlignment.Middle:
                    {
                        child.anchoredPosition = new Vector2(child.anchoredPosition.x, (-top + (((height - (-top - bottom)) - child.rect.height) * 0.5f)) - ((height * 0.5f) - (child.rect.height * 0.5f)) - ((layout.Top - layout.Bottom) * 0.5f));
                    }
                    break;

                case eUILayout.eVerticalAlignment.Bottom:
                    {
                        child.anchoredPosition = new Vector2(child.anchoredPosition.y, (bottom + layout.Bottom));
                        bottom += (child.rect.height + (layout.Top + layout.Bottom));
                    }
                    break;

                case eUILayout.eVerticalAlignment.Stretch:
                    {
                        child.offsetMin = new Vector2(child.offsetMin.x, (bottom + layout.Bottom) );
                        child.offsetMax = new Vector2(child.offsetMax.x, -(top + layout.Top));
                    }
                    break;
            }
        }
    }
}
