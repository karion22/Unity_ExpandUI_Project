using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eUICamera : MonoBehaviour
{
    protected Camera m_Camera;

    public Camera Camera { get { return m_Camera; } private set { m_Camera = value; } }

    public eUICamera(Camera inCamera) { Camera = inCamera; }
    public void SetCamera(Camera inCamera) { Camera = inCamera; }
}
