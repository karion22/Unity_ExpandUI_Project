using KRN.Utility;
using UnityEngine;

public class eElement : MonoBehaviour
{
    protected RectTransform m_RectTransform = null;
    public RectTransform RectTransform
    {
        get
        {
            if(object.ReferenceEquals(m_RectTransform, null))
                m_RectTransform = transform as RectTransform;
            return m_RectTransform;
        }
    }

    protected virtual void Awake() { }
    protected virtual void Start() { }
    public virtual void OnCreated(int inElement, Transform inParent) 
    {
        transform.SetParent(inParent);
    }

    protected virtual void OnDestroy() { }
    protected virtual void OnEnable() { }
    protected virtual void OnDisable() { }

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
}