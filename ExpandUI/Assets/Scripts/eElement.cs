using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class eElement : MonoBehaviour
{
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
        gameObject?.SetActive(isActivate);
    }

    public void Update()
    {
    }
}