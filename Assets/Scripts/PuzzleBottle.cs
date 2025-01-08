using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBottle : MonoBehaviour
{
    [SerializeField] private int size = 4;
    [SerializeField] private LongBodyMesh sizeMesh;
    [Space]
    [SerializeField] private Transform ballsParent;
    [SerializeField] private float ballsOffset = 0.25f;
    [SerializeField] private float ballsInterval = 0.5f;
    [SerializeField] private float ballMoveTime = 0.2f;

    private Transform _t;

    private Queue<PuzzleBall> _balls = new Queue<PuzzleBall>(0);

    private void Awake()
    {
        _t = transform;

        SizeUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        PuzzleBall b;
        if (!other.TryGetComponent(out b)) return;

        AddBall(b);
    }

    private void SizeUpdate()
    {
        sizeMesh.SetBodyLength(size);
    }

    private Vector3 CalcBallPos(int index)
    {
        return Vector3.down * (ballsOffset + ballsInterval * (size - index));
    }

    private void AddBall(PuzzleBall newBall)
    {
        newBall.transform.parent = ballsParent;

        newBall.SetLocalPosition(CalcBallPos(_balls.Count), ballMoveTime);

        this.DelayedAction(0.05f, () =>
        {
            _balls.Enqueue(newBall);

            var popBall = _balls.Dequeue();

            popBall.SetLocalPosition(CalcBallPos(-2), ballMoveTime);

            GlobalVar.Set<PuzzleBall>("drop-ball", popBall);

            var ba = _balls.ToArray();

            for (int i = 0; i < ba.Length; i++)
                ba[i].SetLocalPosition(CalcBallPos(i), ballMoveTime);

            newBall.InvokeEvent("on-bottle-add");

            popBall.InvokeEvent("on-bottle-pop");

            this.InvokeEvent("on-add-ball");

            CheckFullMatch();
        });
    }

    private void CheckFullMatch()
    {
        var ba = _balls.ToArray();

        //if (ba.Length < size) return;

        var fbid = ba[0].GetId();

        for (int i = 1; i < ba.Length; i++)
            if (ba[i].GetId() != fbid) return;

        //_t.parent = null;

        this.InvokeEvent("on-full-match");

        for (int i = 0; i < ba.Length; i++)
            ba[i].InvokeEvent("on-bottle-match");
    }

    public void SetSize(int value)
    {
        size = value;

        SizeUpdate();
    }

    public void SetBall(PuzzleBall newBall)
    {
        if (_balls.Count >= size) return;

        newBall.transform.parent = ballsParent;

        newBall.SetLocalPosition(CalcBallPos(_balls.Count));

        _balls.Enqueue(newBall);

        newBall.InvokeEvent("on-bottle-set");
    }
}
