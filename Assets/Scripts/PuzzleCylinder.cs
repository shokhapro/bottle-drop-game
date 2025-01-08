using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCylinder : MonoBehaviour
{
    [SerializeField] private Transform cylinder;
    [SerializeField] private float cylinderMinRadius = 0.25f;
    [SerializeField] private Transform bottleParent;
    [SerializeField] private float bottleRadius = 0.25f;
    [SerializeField] private Transform ballDrop;

    private void Awake()
    {
        //
    }

    private void Start()
    {
        Retransform(1);
    }

    public void Retransform(float time)
    {
        var n = bottleParent.childCount;

        if (n == 0)
        {
            GlobalEvent.InvokeGlobal("on-cylinder-no-bottles");

            return;
        }

        var a = 360f / n;

        GlobalVar.SetFloat("bottle-angle", a);

        var r2 = bottleRadius / Mathf.Sin(a/2f * Mathf.Deg2Rad);

        var r = r2 - bottleRadius;

        if (r < cylinderMinRadius)
        {
            r = cylinderMinRadius;

            r2 = r + bottleRadius;
        }

        //cylinder.localScale = new Vector3(r * 2f, 10f, r * 2f);
        cylinder.DOKill();
        cylinder.DOScale(new Vector3(r * 2f, 10f, r * 2f), time);

        for (var i = 0; i < n; i++)
        {
            var ba = a * i;

            //save bottle angles array

            var v = new Vector2(Mathf.Cos(ba * Mathf.Deg2Rad), Mathf.Sin(ba * Mathf.Deg2Rad));

            //bottleParent.GetChild(i).localPosition = new Vector3(v.x * r2, 0f, v.y * r2);
            bottleParent.DOKill();
            bottleParent.GetChild(i).DOLocalMove(new Vector3(v.x * r2, 0f, v.y * r2), time);
        }

        //ballDrop.localPosition = new Vector3(r2, 1f, 0f);
        ballDrop.DOKill();
        ballDrop.DOLocalMove(new Vector3(r2, 1f, 0f), time);

        GlobalEvent.InvokeGlobal("on-cylinder-size-update");
    }
}
