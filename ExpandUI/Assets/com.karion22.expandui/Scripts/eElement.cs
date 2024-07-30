using KRN.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class eElement : UIBehaviour
{
    protected RectTransform m_RectTransform = null;
    protected RectTransform RectTransform
    {
        get
        {
            if(object.ReferenceEquals(m_RectTransform, null))
                m_RectTransform = transform as RectTransform;
            return m_RectTransform;
        }
    }

    public virtual void OnCreated(int inElement, Transform inParent) 
    {
        transform.SetParent(inParent, false);
    }

    public virtual void SetActive(bool isActivate)
    {
        if(gameObject == null)
        {
            DebugLog.Assert("GameObject is null");
        }
        else
        {
            gameObject.SetActive(isActivate);
            RectTransform.parent?.GetComponent<eDockLayout>()?.UpdateLayout(false);
        }
    }

    public virtual void SetSize(Vector2 inSize)
    {
        if(RectTransform != null)
            RectTransform.sizeDelta = inSize;
    }
}