﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallComponent : MonoBehaviour {

    private Vector2Int _thisFieldPosition;
    private Animator _thisAnimator;
    private BoxCollider2D _thisCollider;
    private Transform _thisTransform;
    private bool _isWall;

	void Start () {
        _thisTransform = transform;
        _thisTransform.rotation *= Quaternion.Euler(0,0,90 * Random.Range(0,4));

        _thisAnimator = GetComponent<Animator>();
        _thisCollider = gameObject.GetComponent<BoxCollider2D>();
    }

    public Vector2Int FieldCoordinates
    {
        get
        {
            return _thisFieldPosition;
        }
    }

    public Vector3 Position
    {
        get
        {
            return _thisTransform.position;
        }
    }

    public bool IsWall
    {
        get
        {
            return _isWall;
        }

        private set
        {
            _isWall = value;
        }
    }

    public void OnDeath()
    {
        _thisCollider.enabled = false;
        IsWall = false;
        _thisAnimator.SetTrigger("Explode");
    }

    public void SetValue(bool isWall)
    {
        _isWall = isWall;
    }

    public void SetFieldValues(int n, int m, bool isWall)
    {
        _thisFieldPosition = new Vector2Int(n, m);
        _isWall = isWall;
    }
}
