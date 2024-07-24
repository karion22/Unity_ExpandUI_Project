using UnityEngine;
using UnityEngine.UI;

public class eInputField : eEventElement
{
    #region Property
    private InputField m_InputField = null;
    public InputField InputField
    {
        get
        {
            if(object.ReferenceEquals(this, m_InputField))
                m_InputField = GetComponent<InputField>();
            return m_InputField;
        }
    }

    public string Text
    {
        get { return InputField.text; }
        set { InputField.text = value; }
    }

    private GameObject m_PlaceHolder = null;
    public GameObject Placeholder
    {
        get
        {
            if (object.ReferenceEquals(m_InputField, null) || object.ReferenceEquals(m_InputField.placeholder, null))
                return null;

            if (object.ReferenceEquals(m_PlaceHolder, null))
                m_PlaceHolder = m_InputField.placeholder.gameObject;
            return m_PlaceHolder;
        }
    }
    #endregion

    public void ShowPlaceHolder(bool isShow)
    {
        Placeholder?.SetActive(isShow);
    }

    public void ModifyMode(bool isModify)
    {
        InputField.readOnly = isModify;

        if (isModify)
            InputField.ActivateInputField();
        else
            InputField.DeactivateInputField();
    }
}
