using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCylinderRotation : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    [Space]
    [SerializeField] private float slideFactor = 5f;
    [SerializeField] private float smooth = 0.8f;

    private Transform _t;
    private float _rot = 0f;
    private float _rot2 = 0f;
    private float _leaY = 0f;
    private bool _mouseDown = false;
    private Vector3 _lastMousePosition;

    private void Awake()
    {
        _t = transform;

        _leaY = _t.localEulerAngles.y;
    }

    private void Update()
    {
        RotateUpdate();

        SnapUpdate();

        _leaY = Mathf.Lerp(_rot2, _leaY, smooth);
        _t.localEulerAngles = new Vector3(0f, _leaY, 0f);
    }

    private void RotateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckUITouch.IsPointerOverUIObject()) return;

            _mouseDown = true;
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseDown = false;

            AlignAngle();
        }

        if (!_mouseDown) return;

        var tdp = Input.mousePosition - _lastMousePosition;

        _lastMousePosition = Input.mousePosition;

        if (!isActive) return;

        if (!TouchAxisFilter.IsHorizontal()) return;

        var deltaRot = tdp.x / Screen.width;

        var a = GlobalVar.GetFloat("bottle-angle");

        _rot += deltaRot * slideFactor * (a / 24f);
    }

    private void SnapUpdate()
    {
        var a = GlobalVar.GetFloat("bottle-angle");

        _rot2 = Mathf.Lerp(_rot, Mathf.Round(_rot / a) * a, 0.75f);
    }

    public void AlignAngle()
    {
        var al = GlobalVar.GetFloat("bottle-angle");

        _rot = Mathf.Round(_rot / al) * al;
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }
}
