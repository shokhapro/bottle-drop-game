using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBall : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField] private Material material;
    [Space]
    [SerializeField] private Renderer[] materialRenderer;

    private Transform _t;
    private Tweener _tweener;

    private void Awake()
    {
        _t = transform;
    }

    private void Start()
    {
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        foreach (var mr in materialRenderer)
            if (mr != null)
                mr.material = material;
    }

    public void SetId(int value)
    {
        id = value;
    }

    public int GetId() { return id; }

    public void SetMaterial(Material value)
    {
        material = value;

        UpdateMaterial();
    }

    public void SetLocalPosition(Vector3 value, float moveTime = 0f)
    {
        if (moveTime <= 0f)
            _t.localPosition = value;
        else
        {
            if (_tweener != null) _tweener.Kill();
            _tweener = _t.DOLocalMove(value, moveTime).SetEase(Ease.OutBack);
        }
    }
}
