using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CentipedeController : MonoBehaviour {
    private float timeCoefficient;
    private float positionUpdateTime;
    private int _sectionCount;
    private Transform[] tail;
    private List<Vector3> _wayPoints;
    private Transform _thisTransform;
    private FieldController _fieldController;
    private Camera _mainCamera;
    private CentipedeSection _thisSection;
    private Vector2 _direction = Vector2.right;

    public static List<CentipedeController> controllers;
    public float TimeCoefficient { get { return timeCoefficient; } set { timeCoefficient = value; } }
    public float PositionUpdateTime { get { return positionUpdateTime; } set { positionUpdateTime = value; } }
    public Vector2 Direction { get { return _direction; } }

    private void Awake()
    {
        if (controllers == null)
            controllers = new List<CentipedeController>();

        controllers.Add(this);
        _wayPoints = new List<Vector3>();
        _thisTransform = transform;
        _mainCamera = Camera.main;
        _thisSection = GetComponent<CentipedeSection>();
        _fieldController = (FieldController)FindObjectOfType(typeof(FieldController));
        if (_thisSection != null)
            _thisSection.enabled = false;

        if (tail != null)
        {
            _sectionCount = tail.Length;
            CentipedeSection tmpSection;
            for (int i = 0; i < _sectionCount; i++)
            {
                tmpSection = tail[i].GetComponent<CentipedeSection>();
                tmpSection.Head = this;
                tmpSection.SectionIndex = i;
            }
        }
    }



    private void Update()
    {
        if (_mainCamera.WorldToViewportPoint(_thisTransform.position).y < 0)
        {
            foreach (Transform t in tail)
                Destroy(t.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(MoveCentipede());
    }

    public void StopMoving()
    {
        StopAllCoroutines();
    }

    public void SetDirection(Vector2 d)
    {
        _direction = d;
    }

    public void SetUpdateTime(float t)
    {
        positionUpdateTime = t;
    }

    public void SetTail(Transform[] newTail)
    {
        tail = newTail;
        _sectionCount = tail.Length;
        CentipedeSection tmpSection;
        for (int i = 0; i < _sectionCount; i++)
        {
            tmpSection = tail[i].GetComponent<CentipedeSection>();
            tmpSection.Head = this;
            tmpSection.SectionIndex = i;
        }
    }

    public void Split(int deadSectionIndex)
    {
        StopAllCoroutines();

        Transform[] firstTail = tail.Take(deadSectionIndex).ToArray();
        Transform[] secondTail = tail.Skip(deadSectionIndex + 1).ToArray();

        tail = firstTail;
        _sectionCount = tail.Length;
        RemoveExcessPoint();
        StartCoroutine(MoveCentipede());
        
        if(secondTail.Length > 0)
        {
            CentipedeController cc = secondTail.First().gameObject.AddComponent<CentipedeController>();
            cc.SetDirection(_direction);
            Transform[] tmpArray = new Transform[secondTail.Length];
            Array.Copy(secondTail, tmpArray, secondTail.Length);
            List<Transform> list = tmpArray.ToList();
            list.RemoveAt(0);
            cc.SetTail(list.ToArray());
            cc.SetUpdateTime(positionUpdateTime - positionUpdateTime * timeCoefficient);
        }
    }

    private IEnumerator StartMove()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveCentipede());
    }
    
    private IEnumerator MoveCentipede()
    {
        WallComponent wc = _fieldController.GetNearestPoint(_thisTransform.position);
        _thisTransform.position = wc.Position;
        WallComponent newPoint = _fieldController.GetNextPointInDirection(wc, ref _direction);
        _wayPoints.Add(newPoint.Position);
        MoveTail();

        while(true)
        {
            newPoint = _fieldController.GetNextPointInDirection(newPoint, ref _direction);
            _wayPoints.Add(newPoint.Position);
            yield return StartCoroutine
                (MoveInterpolation(_thisTransform,newPoint.Position, positionUpdateTime));
            _thisTransform.position = newPoint.Position;
            MoveTail();
        }

    }

    public void AddNewWayPoint(Vector2 position)
    {
        _wayPoints.Add(position);

        RemoveExcessPoint();
    }

    private void RemoveExcessPoint()
    {
        while (_wayPoints.Count > _sectionCount)
            _wayPoints.RemoveAt(0);
    }

    private void MoveTail()
    {
        int waysCount = _wayPoints.Count;
        Vector3 tmpPoint;
        for (int i = 0; i < waysCount && i < _sectionCount; i++)
        {
            tmpPoint = _wayPoints[waysCount - 1 - i];

            if(tail[i] != null)
                StartCoroutine(MoveInterpolation(tail[i], tmpPoint, positionUpdateTime));
        }
    }

    private IEnumerator MoveInterpolation(Transform transf, Vector3 target, float time)
    {
        if (transf == null || target == null)
            yield break;

        float startTimeValue = time;
        Vector3 startPosition = transf.position;
        while (time >= 0)
        {
            if (transf == null || target == null)
                yield break;

            transf.position = Vector3.Lerp(startPosition, target, 1.0f - (time / startTimeValue));
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public void OnDeath()
    {
        StopAllCoroutines();

        if (tail.Length > 0 && tail[0].transform != null)
        {
            Transform nextHead = tail[0];

            CentipedeController cc = nextHead.gameObject.AddComponent<CentipedeController>();
            cc.SetDirection(_direction);
            var list = tail.ToList();
            list.RemoveAt(0);
            cc.SetTail(list.ToArray());
            cc.SetUpdateTime(positionUpdateTime - positionUpdateTime * timeCoefficient);
        }

        controllers.Remove(this);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Messenger<int>.Broadcast(EventStrings.UP_HEALTH, -10);
            OnDeath();
        }
    }
}
