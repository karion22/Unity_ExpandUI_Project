using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class eText : eElement
{
    private Text m_Text;

    public Text Text { 
        get 
        { 
            if(m_Text == null)
                m_Text = GetComponent<Text>();
            return m_Text;
        }
    }

    public void SetText(string inText)
    {
        if (Text != null) Text.text = inText;
    }

    public void SetAlignment(TextAnchor inAnchor)
    {
        if(Text != null) Text.alignment = inAnchor;
    }
}
