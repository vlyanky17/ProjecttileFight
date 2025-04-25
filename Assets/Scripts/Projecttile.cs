using DG.Tweening;
using UnityEngine;

public abstract class Projecttile : MonoBehaviour
{
    protected Sequence _sequence;
    protected abstract void Hit();
    public abstract void BeginBehavior( Vector3 vector3);

    public abstract void Init(float speed, ProjecttailOperator owner);

    protected abstract void EndBehavior();

}
