using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeController : MonoBehaviour {
    
    private Transform _thisTransform;
    private Vector2 _direction;
    private Coroutine _currentCoroutine;
    public float speed;

    public CentipedeSection _nextSection;
    public int ID { get; set; }
    public int sectionCount;
    private Dot _startDot;
    private Dot _lastDot;

    private void Start()
    {
        _thisTransform = transform;
        _direction = _thisTransform.right;
        _nextSection.SetValues(speed, ID);
        _currentCoroutine = StartCoroutine(GoRight());
        _startDot = new Dot(_thisTransform.position, Vector2.down, ID);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        if(_currentCoroutine != null)
            StartCoroutine(GoRight());
    }

    private void AddNewRotationPoint(Vector2 direction)
    {
        if (_lastDot != null && Vector3.Distance(_thisTransform.position, _lastDot.position) < 0.1f)
        {
            Debug.Log("Changed Rotation");
            _lastDot.direction = direction;
        }
        else
        {
            _lastDot = new Dot(_thisTransform.position, direction, ID);
        }
    }

    private IEnumerator GoDown()
    {
        Vector3 downPosition = _thisTransform.position - new Vector3(0, 0.5f);
        _direction = Vector2.down;

        AddNewRotationPoint(_direction);

        while (_thisTransform.position.y > downPosition.y)
        {
            _thisTransform.Translate(_direction * speed * Time.deltaTime);
            yield return null;
        }

        _thisTransform.position = downPosition;
        yield break;
    }

    private IEnumerator GoRight()
    {
        RaycastHit2D hit;
        yield return StartCoroutine(GoDown());
        _direction = Vector2.right;

        AddNewRotationPoint(_direction);

        hit = Physics2D.Raycast(_thisTransform.position, _direction, 0.2f);
        while (!hit)
        {
            _thisTransform.Translate(_direction * speed * Time.deltaTime);
            hit = Physics2D.Raycast(_thisTransform.position, _direction, 0.2f);
            yield return null;
        }
        _currentCoroutine = StartCoroutine(GoLeft());
        yield break;
    }

    private IEnumerator GoLeft()
    {
        RaycastHit2D hit;
        yield return StartCoroutine(GoDown());
        _direction = Vector2.left;

        AddNewRotationPoint(_direction);
        
        hit = Physics2D.Raycast(_thisTransform.position, _direction, 0.2f);
        while (!hit)
        {
            _thisTransform.Translate(_direction * speed * Time.deltaTime);
            hit = Physics2D.Raycast(_thisTransform.position, _direction, 0.2f);
            yield return null;
        }
        _currentCoroutine = StartCoroutine(GoRight());
        yield break;
    }
}
