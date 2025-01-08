using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongBodyMesh : MonoBehaviour
{
    [SerializeField] private float bodyLength = 1.0f;
    [SerializeField] private bool fixedHead = false;
    [Space]
    [SerializeField] private Transform head;
    [SerializeField] private Transform body;
    [SerializeField] private Transform foot;
    [Space]
    [SerializeField] private float heightScale = 1f;
    [SerializeField] private float heightAdd = 1f;
    [SerializeField] private float heightMin = 1f;

    private bool _varsCached = false;
    private Vector3 headPos;
    private Vector3 bodyPos;
    private Vector3 footPos;
    private Vector3 bodyScale;

    private void Start()
    {
        SizeUpdate();
    }

    private void SizeUpdate()
    {
        if (!_varsCached)
        {
            headPos = head.localPosition;
            bodyPos = body.localPosition;
            footPos = foot.localPosition;
            bodyScale = body.localScale;

            _varsCached = true;
        }

        var bs = bodyScale;
        bs.y *= bodyLength * heightScale;
        bs.y += heightAdd;
        if (bs.y < heightMin) bs.y = heightMin;
        body.localScale = bs;

        var f = bs.y / bodyScale.y;

        if (fixedHead)
        {
            var bp = bodyPos;
            bp.y = headPos.y + (bodyPos.y - headPos.y) * f;
            body.localPosition = bp;

            var fp = headPos;
            fp.y = headPos.y + (footPos.y - headPos.y) * f;
            foot.localPosition = fp;

            head.localPosition = headPos;
        }
        else
        {
            var bp = bodyPos;
            bp.y = footPos.y + (bodyPos.y - footPos.y) * f;
            body.localPosition = bp;

            var hp = headPos;
            hp.y = footPos.y + (headPos.y - footPos.y) * f;
            head.localPosition = hp;

            foot.localPosition = footPos;
        }
    }

    public void SetBodyLength(float value)
    {
        bodyLength = value;

        SizeUpdate();
    }
}
