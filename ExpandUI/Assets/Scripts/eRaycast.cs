using UnityEngine.UI;

public class eRaycast : Graphic
{
    public override void SetMaterialDirty() { }
    public override void SetVerticesDirty() { }

    protected override void OnPopulateMesh(VertexHelper inVertex)
    {
        inVertex.Clear();
    }
}
