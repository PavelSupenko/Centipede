using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Up, Down, Right, Left};
public class LinearMove : MonoBehaviour {

    public Direction direction;
    public float speed;
    private Transform _thisTransform;

    private void Start()
    {
        _thisTransform = transform;
    }

    void Update () {
        Vector2 movement;
        switch(direction)
        {
            case Direction.Down: movement = Vector2.down; break;
            case Direction.Up: movement = Vector2.up; break;
            case Direction.Left: movement = Vector2.left; break;
            case Direction.Right: movement = Vector2.right; break;
            default: movement = Vector2.up; break;
        }

        _thisTransform.Translate(movement * speed * Time.deltaTime);
	}
}
