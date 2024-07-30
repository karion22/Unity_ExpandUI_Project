using KRN.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eButton : eEventElement
{
    #region Property
    [SerializeField] private Text m_Text = null;
    public Text Text
    {
        get
        {
            if (object.ReferenceEquals(m_Text, null))
                m_Text = GetComponent<Text>();
            return m_Text;
        }
    }

    [SerializeField] private Button m_Button = null;
    public Button Button
    {
        get
        {
            if(object.ReferenceEquals(m_Button, null))
                m_Button = GetComponent<Button>();
            return m_Button;
        }
    }

    [SerializeField] private Image m_Image = null;
    public Image Image
    {
        get
        {
            if(object.ReferenceEquals(m_Image, null))
                m_Image = GetComponent<Image>();
            return m_Image;
        }
    }
    #endregion

    [SerializeField] private bool m_bEnableAlphaHitTest = false;
    [SerializeField] private float m_AlphaHitTestMinimumThreshold = 0.5f;

    protected override void Start()
    {
        base.Start();

        if(m_bEnableAlphaHitTest && Image != null)
        {
            // Image Source Sprite °ª È®ÀÎ
            if (CheckAlphaHitTest())
                Image.alphaHitTestMinimumThreshold = m_AlphaHitTestMinimumThreshold;
        }        
    }

    public void SetEnable(bool isEnable, float inDisableAlphaValue = 0.5f)
    {
        enabled = isEnable;
        Button.interactable = enabled;

        var group = GetComponent<CanvasGroup>();
        if(group != null)
        {
            if (isEnable)
                group.alpha = 1.0f;
            else
                group.alpha = inDisableAlphaValue;
        }
        else
        {
            float alpha = 1.0f;
            if (isEnable == false)
                alpha = inDisableAlphaValue;

            if (Text != null)
            {
                var prevColor = Text.color;
                Text.color = new Color(prevColor.r, prevColor.g, prevColor.b, alpha);
            }

            if(Image != null)
            {
                Image.color = isEnable ? Button.colors.normalColor : Button.colors.disabledColor;
            }

            var childrens = new List<Image>();
            gameObject.GetComponentsInChildren(true, childrens);

            foreach(var child in childrens)
            {
                var prevColor = child.color;
                child.color = new Color(prevColor.r, prevColor.g, prevColor.b, alpha);
            }
        }
    }

    public void SetText(string inText)
    {
        if (Text != null)
            Text.text = inText;
    }

    private bool CheckAlphaHitTest()
    {
        if (Image == null || Image.mainTexture == null)
            DebugLog.Warning("Image or texture is null");

        return (Image.mainTexture.isReadable == false);
    }
}
