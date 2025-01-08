using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIElementOnObject : MonoBehaviour
{
    [SerializeField] private Transform target;
    [Space]
    [SerializeField] private string findTargetByKey = "";
    [SerializeField] private bool findingUpdate = false;
    [Space]
    [SerializeField] private Vector2 offset = Vector2.zero;
    [Space]
    [SerializeField] private bool keepOnScreen = false;
    [SerializeField] private float keepSpacing = 10f;
    [SerializeField] private Vector2 keepRectSize = new Vector2(50f, 50f);
    [SerializeField] private string getGlobalVarKeepRectSize = "";

    private RectTransform _rt;
    private Canvas _canvas;
    private Camera _c;
    private Vector3 _anchPos;
    private RectTransform _parentRect;

    private void Awake()
    {
        _rt = GetComponent<RectTransform>();

        _canvas = GetComponentInParent<Canvas>();

        _c = Camera.main;

        _anchPos = _rt.anchoredPosition;

        _parentRect = _rt.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        FindTargetByKeyUpdate();
    }

    private void LateUpdate()
    {
        if (findingUpdate)
            FindTargetByKeyUpdate();

        RectSizeUpdate();

        PositionUpdate();

        _rt.anchoredPosition = _anchPos;
    }

    private void PositionUpdate()
    {
        if (!target) return;

        var vpos = _c.WorldToScreenPoint(target.position);

        var cpos = vpos / _canvas.scaleFactor;
        
        cpos.x += offset.x;
        cpos.y += offset.y;

        if (keepOnScreen)
        {
            var canvasSize = new Vector2(Screen.width, Screen.height) / _canvas.scaleFactor;

            cpos.x = Mathf.Clamp(cpos.x, keepRectSize.x + keepSpacing, canvasSize.x - keepRectSize.x - keepSpacing);
            cpos.y = Mathf.Clamp(cpos.y, keepRectSize.y + keepSpacing, canvasSize.y - keepRectSize.y - keepSpacing);
        }

        _anchPos = new Vector2(cpos.x, cpos.y);
    }

    private void RectSizeUpdate()
    {
        if (getGlobalVarKeepRectSize == "") return;

        var gv = GlobalVar.Get<Vector2>(getGlobalVarKeepRectSize);

        keepRectSize = gv * 0.5f;
    }

    public void SetTarget(Transform targ)
    {
        target = targ;
    }

    private void FindTargetByKeyUpdate()
    {
        if (findTargetByKey == "") return;

        foreach (var targ in UIElementOnObjectTarget.all)
        if (targ.GetKey() == findTargetByKey)
        {
            target = targ.transform;
            break;
        }
    }

/*    public void SetFindingUpdate(bool value)
    {
        findingUpdate = value;
    }*/
}
