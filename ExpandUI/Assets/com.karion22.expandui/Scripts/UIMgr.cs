using KRN.Utility;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : Singleton<UIMgr>
{
    private eCanvas m_RootCanvas = null;
    public eCanvas RootCanvas 
    { 
        get { return m_RootCanvas; } 
        private set { m_RootCanvas = value; } 
    }

    List<eCanvas> m_CanvasList = new List<eCanvas>();

    public GameObject m_LongPressPrefab = null;

    [SerializeField] private GameObject m_CameraPrefab = null;
    private Camera m_Camera = null;
    public Camera Camera 
    { 
        get 
        { 
            if(m_Camera == null)
            {
                var uiCamera = FindFirstObjectByType<eUICamera>() ?? Instantiate(m_CameraPrefab).GetComponent<eUICamera>();
                m_Camera = uiCamera.Camera;
            }
            return m_Camera; 
        }
        private set { m_Camera = value; }
    }

    public void AddCanvas(eCanvas inTarget)
    {
        m_CanvasList.Add(inTarget);
    }

    public void RemoveCanvas(eCanvas inTarget)
    {
        m_CanvasList.Remove(inTarget);
    }

    public void AloneCanvas(eCanvas inTarget)
    {
        eCanvas element = null;
        for (int i = 0, end = m_CanvasList.Count; i < end; i++)
        {
            element = m_CanvasList[i];

            if (element == null) continue;
            if (element.name == inTarget.name) continue;

            m_CanvasList[i].SetActive(false);
        }
    }
}
