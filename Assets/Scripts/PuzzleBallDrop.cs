using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBallDrop : MonoBehaviour
{
    [SerializeField] private bool isActive = true;
    [Space]
    [SerializeField] private Transform fadeinPos;
    [SerializeField] private Transform pushPos;
    [SerializeField] private Transform fadeoutPos;
    [SerializeField] private Transform transport;
    [SerializeField] private float time = 1f;

    private Transform _t;

    private PuzzleBottle _activeBottle;
    private PuzzleBall _activeBall;

    private bool _mouseDown = false;
    private Vector3 _firstMousePosition;

    private void Awake()
    {
        _t = transform;
    }

    private void Update()
    {
        TouchUpdate();
    }

    private void FixedUpdate()
    {
        BottleRaycast();
    }

    private void BottleRaycast()
    {
        RaycastHit hit;
        if (!Physics.Raycast(_t.position, Vector3.down, out hit, 50f)) return;

        PuzzleBottle bottle;
        if (!hit.collider.TryGetComponent(out bottle)) return;

        if (bottle == _activeBottle) return;

        if (_activeBottle != null) _activeBottle.gameObject.InvokeEvent("on-drop-active-false");

        _activeBottle = bottle;

        _activeBottle.gameObject.InvokeEvent("on-drop-active-true");

        this.InvokeEvent("on-find-bottle");
    }

    private void TouchUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CheckUITouch.IsPointerOverUIObject()) return;

            _mouseDown = true;
            _firstMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _mouseDown = false;
        }

        if (!_mouseDown) return;

        var tdp = Input.mousePosition - _firstMousePosition;

        if (!isActive) return;

        if (!TouchAxisFilter.IsVertical()) return;

        var deltaTouch = tdp.y / Screen.height;

        if (deltaTouch < -0.125f)
        {
            BallDrop();

            _mouseDown = false;
        }
    }

    private void BallDrop()
    {
        if (_activeBall == null) return;

        _activeBall.transform.parent = _t;

        _activeBall.SetLocalPosition(pushPos.localPosition, time);
        
        _activeBall.InvokeEvent("on-drop-drop");

        _activeBall = null;

        this.InvokeEvent("on-drop-ball");
    }

    public void FindDropBall()
    {
        var dropBall = GlobalVar.Get<PuzzleBall>("drop-ball");
        if (dropBall == null) return;
        GlobalVar.Set<PuzzleBall>("drop-ball", null);

        _activeBall = dropBall;

        _activeBall.transform.parent = _t;
    }

    public void FadeinBall()
    {
        if (_activeBall == null) return;

        _activeBall.SetLocalPosition(fadeinPos.localPosition);
        _activeBall.SetLocalPosition(Vector3.zero, time);

        _activeBall.InvokeEvent("on-drop-fadein");
    }

    public void FadeoutBall()
    {
        if (_activeBall == null) return;

        _activeBall.SetLocalPosition(fadeoutPos.localPosition, time);

        _activeBall.InvokeEvent("on-drop-fadeout");
    }

    public void TransportBall()
    {
        if (_activeBall == null) return;

        _activeBall.transform.parent = transport;

        _activeBall.SetLocalPosition(Vector3.zero, time);

        transport.gameObject.InvokeEvent("start-ball-transport");
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }
}
