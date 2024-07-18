using UnityEngine.Events;
using UnityEngine.EventSystems;

public class eEventElement : eElement
{
    #region Events
    public UnityEvent<PointerEventData> onClickEvent = null;
    public UnityEvent<PointerEventData> onLongPressed = null;
    public UnityEvent<PointerEventData> onRightClicked = null;
    #endregion
}
