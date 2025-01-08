using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Camera))]
public class CameraSizeByBounds : MonoBehaviour
{
    [SerializeField] private MeshRenderer rendererObject;
    [SerializeField] private Axis axis = Axis.X;
    [Space]
    [SerializeField] private float add = 0f;
    [SerializeField] private float scale = 1f;
    [Space]
    [SerializeField] private float minSize = 2f;
    //[SerializeField] private float maxSize = 10f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    public void SizeUpdate()
    {
        var bounds = rendererObject.bounds.max;

        var l = 1f;

        switch (axis)
        {
            case Axis.X:
                l = bounds.x;
                break;
            case Axis.Y:
                l = bounds.y;
                break;
            case Axis.Z:
                l = bounds.z;
                break;
        }

        var s = (l + add) * scale;

        if (s < minSize) s = minSize;

        if (s <= 0f) return;

        //cam.orthographicSize = s;
        cam.DOOrthoSize(s, 1f);
    }
}
