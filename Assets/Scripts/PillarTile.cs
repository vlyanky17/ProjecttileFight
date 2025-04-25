using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PillarTile : Projecttile
{
    private float _speed;
    private ProjecttailOperator _operator;

    public override void BeginBehavior(Vector3 vector3)
    {
        transform.localPosition = vector3;
        _speed = 1 / _speed;
        StartCoroutine(WaitAction());
    }


    IEnumerator WaitAction()
    {
        yield return new WaitForSeconds(0.5f);
                _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOLocalMove(new Vector3(transform.localPosition.x, transform.localPosition.y + 1.5f, transform.localPosition.z), _speed)).SetEase(Ease.Linear).OnComplete(EndBehavior);
    }

    public override void Init(float speed, ProjecttailOperator owner)
    {
        _speed = speed;
        _operator = owner;
    }

    protected override void EndBehavior()
    {
        _operator.GetPillarBack(this);
    }

    protected override void Hit()
    {
       
    }
}
