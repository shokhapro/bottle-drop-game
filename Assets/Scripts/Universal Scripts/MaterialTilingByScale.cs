using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class MaterialTilingByScale : MonoBehaviour
{
    private Transform _t;
    private Vector3 _initScale;
    private Vector3 _lastScale;
    private Material _m;
    [SerializeField] private Vector2 _initTiling = new Vector2(1f, 1f);

    private void Awake()
    {
        _t = transform;

        _initScale = _t.lossyScale;

        _lastScale = _t.lossyScale;

        var mr = GetComponent<MeshRenderer>();

        _m = mr.sharedMaterial;

        //_initTiling = _m.mainTextureScale;
    }

    private void Update()
    {
        TileUpdate();
    }

    private void TileUpdate()
    {
        if (_t.lossyScale == _lastScale) return;
        _lastScale = _t.lossyScale;

        var fx = _t.lossyScale.x / _initScale.x;
        var fy = _t.lossyScale.y / _initScale.y;

        _m.mainTextureScale = new Vector2(_initTiling.x * fx, _initTiling.y * fy);
    }
}
