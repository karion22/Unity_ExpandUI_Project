using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class eEventElement : eElement, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    #region Events
    public UnityEvent<PointerEventData> onButtonDown = null;
    public UnityEvent<PointerEventData> onButtonUp = null;
    public UnityEvent<PointerEventData> onButtonClicked = null;

    public UnityEvent<PointerEventData> onLonePressedUp = null;
    public UnityEvent<PointerEventData> onLongPressed = null;
    #endregion

    private bool m_bLongPress = false;
    private PointerEventData m_LongPressData = default;

    [SerializeField] private bool m_bRaycastAll = false;
    [SerializeField] private bool m_bShowLongPressEffect = true;

    private Coroutine onLongPressEffect = null;
    private Coroutine onLongPressProcess = null;

    public virtual void OnPointerClick(PointerEventData inEventData)
    {
        if(m_bLongPress == false)
        {
            onButtonClicked?.Invoke(inEventData);

            if (m_bRaycastAll)
                RaycastAll(inEventData, ExecuteEvents.pointerClickHandler);
        }
    }

    public virtual void OnPointerDown(PointerEventData inEventData)
    {
        onButtonClicked?.Invoke(inEventData);

        m_bLongPress = false;
        m_LongPressData = inEventData;

        if(m_bShowLongPressEffect)
        {
            if(onLongPressed.GetPersistentEventCount() > 0 || onLonePressedUp.GetPersistentEventCount() > 0)
            {
                if(onLongPressEffect != null)
                    StopCoroutine(onLongPressEffect);
                onLongPressEffect = StartCoroutine("OnLongPressEffect");
            }
        }

        if (onLongPressProcess != null)
            StopCoroutine(onLongPressProcess);
        onLongPressProcess = StartCoroutine("OnLongPressProcess");
    }

    public virtual void OnPointerUp(PointerEventData inEventData)
    {
        StopLongPress();

        if(m_bLongPress == false)
            onButtonUp?.Invoke(inEventData);
        else
            onLongPressed?.Invoke(inEventData);
    }

    public virtual void OnPointerExit(PointerEventData inEventData)
    {
        StopLongPress();
    }

    private void StopLongPress()
    {
        StopCoroutine(onLongPressEffect);

    }

    private IEnumerator OnLongPressEffect()
    {
        float time = 0f, delayTime = 0.5f;

        while (time < delayTime)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        if(UIMgr.IsInstantiate)
        {
            Instantiate(UIMgr.Instance.m_LongPressPrefab);
        }
    }

    private IEnumerator OnLongPressProcess()
    {
        m_bLongPress = true;
        onLongPressed?.Invoke(m_LongPressData);
        yield return null;
    }

    private void RaycastAll<T>(PointerEventData inEventData, ExecuteEvents.EventFunction<T> inEventFunction) where T : IEventSystemHandler
    {
        List<RaycastResult> rayCastAll = new List<RaycastResult>();
        EventSystem.current.RaycastAll(inEventData, rayCastAll);

        RaycastResult raycast;
        GameObject target = null;
        for(int i = 0, end = rayCastAll.Count; i < end; i++)
        {
            raycast = rayCastAll[i];
            target = transform.parent.gameObject;

            while (target != null)
            {
                if (raycast.gameObject == target)
                {
                    ExecuteEvents.Execute(raycast.gameObject, inEventData, inEventFunction);
                    return;
                }
                else if (target.transform.parent != null)
                    target = target.transform.parent.gameObject;
                else
                    break;
            }
        }
    }
}
