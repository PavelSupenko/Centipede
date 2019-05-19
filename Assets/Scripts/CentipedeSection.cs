using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CentipedeSection : MonoBehaviour {
    public Transform nextSectionTransform;
    private CentipedeSection _nextSection;
    private Transform _thisTransform;
    private float _speed;
    private int _headID;
    private Vector2 _direction;

    private void Awake()
    {
        if(nextSectionTransform != null)
            _nextSection = nextSectionTransform.GetComponent<CentipedeSection>();
        _thisTransform = transform;
        _direction = Vector2.right;
    }

    private void Start()
    {
        StartCoroutine(ChangeDirection(_direction, _thisTransform.position));
    }
    
    private void Update()
    {
        _thisTransform.Translate(_direction * _speed * Time.deltaTime);
    }

    private IEnumerator ChangeDirection(Vector2 newDirection, Vector2 pointPosition)
    {
        while (true)
        {
            Dot dot = Dot.First;
            _direction = newDirection;

            if (dot != null)
            {
                var nearestDot = dot.Select(d => new
                {
                    d.direction,
                    d.position,
                    distance = Vector3.Distance(d.position, _thisTransform.position)
                }).OrderBy(d => d.distance).First();

                Debug.Log(dot.position);

                if (nearestDot.position == pointPosition)
                {
                    yield return null;
                }
                else if (_direction.Equals(Vector2.down))
                {
                    if (nearestDot.position.y >= _thisTransform.position.y)
                    {
                        _thisTransform.position = nearestDot.position;
                        StartCoroutine(ChangeDirection(nearestDot.direction, nearestDot.position));
                        yield break;
                    }
                }
                else if (_direction.Equals(Vector2.left))
                {
                    if (nearestDot.position.y >= _thisTransform.position.y)
                    {
                        _thisTransform.position = nearestDot.position;
                        StartCoroutine(ChangeDirection(nearestDot.direction, nearestDot.position));
                        yield break;
                    }
                }
                else if (_direction.Equals(Vector2.right))
                {
                    if (nearestDot.position.y <= _thisTransform.position.y)
                    {
                        _thisTransform.position = nearestDot.position;
                        StartCoroutine(ChangeDirection(nearestDot.direction, nearestDot.position));
                        yield break;
                    }
                }
            }

            yield return null;
        }
    }

    public void SetValues(float speed, int id)
    {
        _speed = speed;
        _headID = id;

        if (_nextSection != null)
            _nextSection.SetValues(speed, id);
    }
}
