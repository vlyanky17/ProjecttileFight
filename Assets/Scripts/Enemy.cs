using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static Enemy;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _runRangeFormCenter;
    [SerializeField] private float _speed;
    [SerializeField] private ProjecttailOperator _operator;
    [SerializeField] private float _sphereRange;
    [SerializeField] private Player _player;
    [SerializeField] private int _sphereCount;
    private Sequence _sequence;
    private Positions _position;
    private Positions _lastPposition;
    private Vector3 _posToRun;
    private SphereTile _sphere;
    private BehaviorType _behaviorType;
    private List<PillarTile> _pillars= new List<PillarTile>();
    private Vector3 _startPos;
    private int _count;

    private void Awake()
    {
        _position = Positions.Center;
        _startPos = transform.localPosition;
        _posToRun = _startPos;
        GetRandomBehavior();
    }

    private void RunTo(Vector3 path)
    {
        _sequence = DOTween.Sequence();
        var range = path.x - transform.localPosition.x;
        var speed = 1 * (_speed*Math.Abs(range));
        _sequence.Append(transform.DOLocalMove(path, speed)).SetEase(Ease.Linear).OnComplete(GetRandomBehavior);
    }

    private void RunWithFireTo(Vector3 path)
    {
        int projecttilesToSendCount=3;
        _sequence = DOTween.Sequence();
        var range = path.x - transform.localPosition.x;
        var speed = 1 * (_speed * Math.Abs(range));
        if ((_position != Positions.Center) && (_lastPposition != Positions.Center)) projecttilesToSendCount = 5;

        Vector3[] runPath = new Vector3[projecttilesToSendCount];
        var step = range / runPath.Length;
        for (int i = 0; i < runPath.Length; i++)
        {
            runPath[i]= new Vector3(transform.localPosition.x + step * i, transform.localPosition.y, transform.localPosition.z);
        }

          _sequence.Append(transform.DOLocalPath(runPath, speed).OnStepComplete(SendSphere)).SetEase(Ease.Linear).OnComplete(GetRandomBehavior);
    }

    private void SendSphere()
    {
        _sphere = _operator.GetSphereFromPool();
        _sphere.BeginBehavior(new Vector3(transform.localPosition.x,transform.localPosition.y, _sphereRange));
    }

    private void GetRandomBehavior()
    {
        _lastPposition = _position;
        var type = UnityEngine.Random.Range(0, 8);
        switch (type)
        {
            case 0:
                if (_position != Positions.Center)
                {
                    _position = Positions.Center;
                    _behaviorType = BehaviorType.RunCenter;
                    _posToRun.x = 0;
                    RunTo(_posToRun);
                }
                else GetRandomBehavior();
                break;
            case 1:
                if (_position != Positions.Left)
                {
                    _position = Positions.Left;
                    _behaviorType = BehaviorType.RunLeft;
                    _posToRun.x = _runRangeFormCenter;
                    RunTo(_posToRun);
                }
                else GetRandomBehavior();
                break;
            case 2:
                if (_position != Positions.Right)
                {
                    _position = Positions.Right;
                    _behaviorType = BehaviorType.RunRight;
                    _posToRun.x = -_runRangeFormCenter;
                    RunTo(_posToRun);
                }
                else GetRandomBehavior();
                break;
            case 3:
                if (_position != Positions.Center)
                {
                    _position = Positions.Center;
                    _behaviorType = BehaviorType.AttackRunCenter;
                    _posToRun.x = 0;
                    RunWithFireTo(_posToRun);
                }
                else GetRandomBehavior();
                break;
            case 4:
                if (_position != Positions.Left)
                {
                    _position = Positions.Left;
                    _behaviorType = BehaviorType.AttackRunLeft;
                    _posToRun.x = _runRangeFormCenter;
                    RunWithFireTo(_posToRun);
                }
                else GetRandomBehavior();
                break;
            case 5:
                if (_position != Positions.Right)
                {
                    _position = Positions.Right;
                    _behaviorType = BehaviorType.AttackRunRight;
                    _posToRun.x = -_runRangeFormCenter;
                    RunWithFireTo(_posToRun);
                }
                else GetRandomBehavior();
                break;
            case 6:
                if (_behaviorType != BehaviorType.AttackPillars)
                {
                    _behaviorType = BehaviorType.AttackPillars;
                    CallPillars();
                }
                else GetRandomBehavior();
                break;
            case 7:
                if (_behaviorType != BehaviorType.SendSpheres)
                {
                    _position = Positions.Center;
                    _behaviorType = BehaviorType.SendSpheres;
                    SendSpheres();
                }
                else GetRandomBehavior();
                break;
        }
    }

    private void SendSpheres()
    {
        transform.localPosition = _startPos;
        _count = 0;
        StartCoroutine(CoolDown());
    }

    IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(0.2f);
        _sequence = DOTween.Sequence();
        _sphere = _operator.GetSphereFromPool();
        _sphere.BeginBehavior(new Vector3(transform.localPosition.x + UnityEngine.Random.Range(-0.35f, 0.35f), transform.localPosition.y, _sphereRange));
        _count++;
        if (_sphereCount > _count)
        {
            StartCoroutine(CoolDown());
        }
        else GetRandomBehavior();
    }

    private void CallPillars()
    {
        for (int i = 0; i < 3; i++)
        {
            var pillar = _operator.GetPillarFromPool();
            pillar.transform.parent = transform.parent;
            _pillars.Add(pillar);
        }
        _pillars[0].BeginBehavior(new Vector3(_player.transform.localPosition.x- 0.05f, _player.transform.localPosition.y-1.2f, _player.transform.localPosition.z));
        _pillars[1].BeginBehavior(new Vector3(_player.transform.localPosition.x, _player.transform.localPosition.y - 1.2f, _player.transform.localPosition.z));
        _pillars[2].BeginBehavior(new Vector3(_player.transform.localPosition.x + 0.05f, _player.transform.localPosition.y - 1.2f, _player.transform.localPosition.z));
        _pillars.Clear();
        GetRandomBehavior();
    }

    public enum Positions
    {
        Center,
        Left,
        Right
    }

    public enum BehaviorType
    {
        RunCenter,
        RunLeft,
        RunRight,
        AttackRunLeft,
        AttackRunRight,
        AttackRunCenter,
        AttackPillars,
        SendSpheres
    }
}
