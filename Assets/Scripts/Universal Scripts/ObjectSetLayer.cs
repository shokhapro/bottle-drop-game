using UnityEngine;

public class ObjectSetLayer : MonoBehaviour
{
    public void Set(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
