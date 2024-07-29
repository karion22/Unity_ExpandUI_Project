using KRN.Utility;
using UnityEngine;
using UnityEngine.UI;

public class eRawImage : ePanel
{
    private RawImage m_RawImage = null;
    public RawImage RawImage
    {
        get
        {
            if(object.ReferenceEquals(m_RawImage, null))
                m_RawImage = GetComponent<RawImage>();
            return m_RawImage;
        }
    }

    public void LoadTexture(string inTexturePath)
    {
        if(string.IsNullOrEmpty(inTexturePath) == false)
        {
            AssetBundleManager.LoadAsset<Texture>(inTexturePath, (tex) => {
                LoadTexture(tex);
            });
        }
    }

    public bool LoadTexture(Texture inTexture)
    {
        if(inTexture != null)
        {
            RawImage.texture = inTexture;
            return true;
        }
        return false;
    }

    public void SetColor(Color inColor) { RawImage.color = inColor; }
    public void SetAlpha(float inAlpha) { RawImage.color = new Color(RawImage.color.r, RawImage.color.g, RawImage.color.b, inAlpha); }
}
