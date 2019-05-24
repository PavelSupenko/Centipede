using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Checking the ViewPort position of object to detect
// when object leave the screen
public class DestroyAboveScreen : MonoBehaviour {

    private Camera _camera;
    private Transform _thisTransform;

    private void Start()
    {
        _camera = Camera.main;
        _thisTransform = transform;
    }

    //Remove the object if it`s abroad the screen
    private void Update()
    {
        float yPos = _camera.WorldToViewportPoint(_thisTransform.position).y;
        if (yPos > 1.5 || yPos < -0.5)
        {
            Destroy(this.gameObject);
        }
    }
}
