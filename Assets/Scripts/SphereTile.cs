using DG.Tweening;
using UnityEngine;

public class SphereTile : Projecttile
{
    private float _speed;
    private int _koof=1;
    private ProjecttailOperator _operator;
    public override void BeginBehavior(Vector3 point)
    {
        _speed = 1 / _speed;
        transform.parent=transform.parent.parent;
        var currentPos = transform.localPosition;
        _sequence = DOTween.Sequence();
        var type = Random.Range(0, 2);
        if (type == 0) _koof = -1;
        Vector3[] path = new Vector3[4];
        var dif = transform.localPosition.z- point.z;
        var step = dif / path.Length;
        path[0] = new Vector3(point.x+0.075f*_koof, transform.localPosition.y, currentPos.z-step*1);
        path[1] = new Vector3(point.x + 0.15f * _koof, transform.localPosition.y, currentPos.z - step * 2);
        path[2] = new Vector3(point.x + 0.075f * _koof, transform.localPosition.y, currentPos.z - step * 3);
        path[3] = point;
        _sequence.Append(transform.DOLocalPath(path, _speed).SetEase(Ease.Linear).OnComplete(EndBehavior));
    }

    protected override void EndBehavior()
    {
        _operator.GetSphereBack(this);
    }

    public override void Init(float speed, ProjecttailOperator owner)
    {
        _speed= speed;
        _operator= owner;
    }

    protected override void Hit()
    {

    }
}
