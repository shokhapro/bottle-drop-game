using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAxisFilter : MonoBehaviour
{
    public static TouchAxisFilter Instance;

    [SerializeField] private float threshold = 0.1f;

    private bool _mouseDown = false;
    private Vector3 _firstMousePosition;

    private bool _isX = false;
    private bool _isY = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        AxisUpdate();
    }

    private void AxisUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _mouseDown = true;
            _firstMousePosition = Input.mousePosition;

            _isX = false;
            _isY = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseDown = false;
        }

        if (!_mouseDown) return;

        if (!_isX && !_isY)
        {
            var delta = Input.mousePosition - _firstMousePosition;

            if (Mathf.Abs(delta.x / Screen.width) > threshold)
            {
                _isX = true;

                return;
            }

            if (Mathf.Abs(delta.y / Screen.height) > threshold)
                _isY = true; 
        }
    }

    public bool IsX()
    {
        return _isX;
    }

    public bool IsY()
    {
        return _isY;
    }

    public static bool IsHorizontal()
    {
        if (Instance == null) return false;

        return Instance.IsX();
    }

    public static bool IsVertical()
    {
        if (Instance == null) return false;

        return Instance.IsY();
    }
}
