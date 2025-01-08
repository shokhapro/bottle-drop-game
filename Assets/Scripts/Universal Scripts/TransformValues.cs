using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformValues : MonoBehaviour
{
    [SerializeField] private float transitionTime = 0f;
    [Space]
    [SerializeField] private Vector3[] positions = new Vector3[0];
    [SerializeField] private Vector3[] rotations = new Vector3[0];
    [SerializeField] private Vector3[] scales = new Vector3[0];
    [SerializeField] private Transform[] targets = new Transform[0];

    private Transform _t;

    private void Awake()
    {
        _t = transform;
    }

    public void SetTransitionTime(float value)
    {
        transitionTime = value;
    }

    public void SetPosition(int index)
    {
        if (index < 0 || index >= positions.Length) return;

        if (transitionTime > 0)
        {
            _t.DOLocalMove(positions[index], transitionTime);

            return;
        }
        
        _t.localPosition = positions[index];
    }

    public void SetRotation(int index)
    {
        if (index < 0 || index >= rotations.Length) return;

        if (transitionTime > 0)
        {
            _t.DOLocalRotate(rotations[index], transitionTime);

            return;
        }

        _t.localEulerAngles = rotations[index];
    }

    public void SetScale(int index)
    {
        if (index < 0 || index >= scales.Length) return;

        if (transitionTime > 0)
        {
            _t.DOScale(scales[index], transitionTime);

            return;
        }

        _t.localScale = scales[index];
    }

    public void SetPositionByTarget(int index)
    {
        if (index < 0 || index >= targets.Length) return;

        if (transitionTime > 0)
        {
            _t.DOLocalMove(targets[index].position, transitionTime);

            return;
        }
        
        _t.localPosition = targets[index].position;
    }
}
