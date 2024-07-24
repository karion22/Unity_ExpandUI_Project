using KRN.Utility;
using UnityEngine;
using UnityEngine.Events;

public class eProgress : eElement
{
    #region Property
    private eImage m_BackgroundImg = null;
    public eImage BackgroundImg
    {
        get
        {
            if (object.ReferenceEquals(m_BackgroundImg, null))
                m_BackgroundImg = GetComponent<eImage>();
            return m_BackgroundImg;
        }
    }

    private eImage m_FilledImage = null;
    public eImage FilledImg
    {
        get
        {
            if (object.ReferenceEquals(m_FilledImage, null))
                m_FilledImage = transform.FindEx<eImage>("Filled");
            return m_FilledImage;
        }
    }

    private eText m_Text = null;
    public eText Text
    {
        get
        {
            if (object.ReferenceEquals(m_Text, null))
                m_Text = transform.FindEx<eText>("InnerText");
            return m_Text;
        }
    }
    #endregion

    public UnityEvent onFinished = null;
    private float m_CurrValue = 0f;
    private float m_FromValue = 0f;
    private float m_ToValue = 0f;
    private float m_Timer = 0f;
    private float m_Speed = 1f;

    public void SetColor(Color inColor) { if(FilledImg != null) FilledImg.Color = inColor; }
    public void SetBGColor(Color inColor) { if(BackgroundImg != null) BackgroundImg.Color = inColor; }
    public void SetText(string inText) { if (Text != null) Text.SetText(inText); }
    public void SetTextAlignment(TextAnchor inTextAnchor) { if (Text != null) Text.SetAlignment(inTextAnchor); }

    public void SetValue(float inValue) 
    {
        m_CurrValue = inValue;
        m_FromValue = inValue;
        m_ToValue = inValue;
    }

    public float GetValue() { return m_CurrValue; }

    public void NextValue(float inValue)
    {
        m_FromValue = m_CurrValue;
        m_ToValue = inValue;
        m_Timer = 0f;
    }

    public void SetSpeed(float inValue) { m_Speed = inValue; }

    private void LateUpdate()
    {
        if(m_CurrValue != m_ToValue)
        {
            m_Timer += (Time.deltaTime * m_Speed);
            m_CurrValue = Mathf.Lerp(m_FromValue, m_ToValue, m_Timer);

            UpdateUI();

            if (m_Timer >= 1.0f)
                onFinished?.Invoke();
        }
    }

    private void UpdateUI()
    {
        if (FilledImg != null && FilledImg.Image != null)
            FilledImg.Image.fillAmount = m_CurrValue;
    }
}
