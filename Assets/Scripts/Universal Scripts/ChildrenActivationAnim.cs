using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenActivationAnim : MonoBehaviour
{
    [SerializeField] private float time = 1f;

    private Transform _t;

    private void Awake()
    {
        _t = transform;
    }

    public void Prepare()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Animate()
    {
        StartCoroutine(anim());

        IEnumerator anim()
        {
            var wait = new WaitForSeconds(time / _t.childCount);

            for (var i = 0; i < _t.childCount; i++)
            {
                _t.GetChild(i).gameObject.SetActive(true);

                yield return wait;
            }
        }
    }
}
