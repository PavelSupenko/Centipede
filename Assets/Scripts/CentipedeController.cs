using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// Controller of centipede. Attached to all heads of centipedes
public class CentipedeController : MonoBehaviour {

    private List<Vector3> _wayPoints;
    private Transform _thisTransform;
    private FieldController _fieldController;
    private Camera _mainCamera;
    private CentipedeSection _thisSection;
    private Vector2 _direction = Vector2.right;
    private float timeCoefficient;
    private float positionUpdateTime;
    private int _sectionCount;

    // The tail of centipede
    public Transform[] tail;
    // How many times faster splitted parts than original
    public float TimeCoefficient { get { return timeCoefficient; } set { timeCoefficient = value; } }
    //Time in seconds of changing centipede position
    public float PositionUpdateTime { get { return positionUpdateTime; } set { positionUpdateTime = value; } }
    // The move direction
    public Vector2 Direction { get { return _direction; } }
    
    // Array that contains all centipede heads
    public static List<CentipedeController> controllers;
    
    // Initializing variables and creating tail
    private void Awake()
    {
        if(controllers == null)
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
    
    private void Start()
    {
        if (controllers.Count <= 1)
            StartCoroutine(StartMove());
        else
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
            if (tail[i] != null)
            {
                tmpSection = tail[i].GetComponent<CentipedeSection>();
                tmpSection.Head = this;
                tmpSection.SectionIndex = i;
            }
        }
    }

    // Splitting the centipede in two relative the deadSectionIndex
    public void Split(int deadSectionIndex)
    {
        StopAllCoroutines();
        
        Transform[] firstTail = tail.Take(deadSectionIndex).ToArray();
        Transform[] secondTail = tail.Skip(deadSectionIndex).ToArray();

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

            for (int i = secondTail.Length - 1; i >= 0; i--)
                cc.AddNewWayPoint(secondTail[i].position);
            cc.AddNewWayPoint(secondTail.First().position);
        }
    }

    private IEnumerator StartMove()
    {
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(MoveCentipede());
    }

    // Saving the way centipede head have done and
    // moving the tail along this path
    private IEnumerator MoveCentipede()
    {
        if (_fieldController == null)
            yield break;
        WallComponent wc = _fieldController.GetNearestPoint(_thisTransform.position);
        _thisTransform.position = wc.Position;
        WallComponent newPoint = _fieldController.GetNextPointInDirection(wc, ref _direction);
        AddNewWayPoint(newPoint.Position);
        MoveTail();

        while(true)
        {
            newPoint = _fieldController.GetNextPointInDirection(newPoint, ref _direction);
            AddNewWayPoint(newPoint.Position);
            yield return StartCoroutine
                (MoveInterpolation(_thisTransform,newPoint.Position, positionUpdateTime));
            _thisTransform.position = newPoint.Position;
            MoveTail();
        }

    }

    // Adding new way point into centipede path
    public void AddNewWayPoint(Vector2 position)
    {
        _wayPoints.Add(position);

        RemoveExcessPoint();
    }

    // If path contains more points 
    // that enough -- remode excess points
    private void RemoveExcessPoint()
    {
        while (_wayPoints.Count > _sectionCount)
            _wayPoints.RemoveAt(0);
    }

    // Moving tail along the head path
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

    // Position Interpolaion btw two points on field matrix
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

        transf.position = target;
    }

    // Creating the wall on the current position
    public void OnDeath()
    {
        StopAllCoroutines();
        Messenger<Vector3>.Broadcast(EventStrings.CREATE_WALL, _thisTransform.position);
        if(tail.Length > 0)
            Split(0);
        controllers.Remove(this);
        Destroy(this.gameObject);
    }

    // Damage the player on collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Messenger<int>.Broadcast(EventStrings.UP_HEALTH, -10);
            OnDeath();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
